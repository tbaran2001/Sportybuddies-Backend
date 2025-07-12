namespace Sportybuddies.API.Modules.Identity;

public class TestTokenRequest
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = "test.user@example.com";
    public string UserName { get; set; } = "test.user@example.com";
    public List<string> Roles { get; set; } = new();
}

public record UserCreatedEvent(Guid UserId, string Email) : INotification;

public class AuthModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapGet("/google-login",
            (SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor) =>
            {
                var httpContext = httpContextAccessor.HttpContext;
                var redirectUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/auth/google-response";
                var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
                return Results.Challenge(properties, ["Google"]);
            });

        group.MapGet("/google-response", async (
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration config,
            IMediator mediator) =>
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Results.BadRequest("Error loading external login information.");
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var roles = await userManager.GetRolesAsync(user);
                var jwtToken = GenerateJwtToken(user, roles.ToList(), config);
                return Results.Ok(new { Token = jwtToken });
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return Results.BadRequest("Email claim not received from Google.");
            }

            var userToCreate = await userManager.FindByEmailAsync(email);
            if (userToCreate == null)
            {
                userToCreate = new ApplicationUser { UserName = email, Email = email };
                var createUserResult = await userManager.CreateAsync(userToCreate);

                if (!createUserResult.Succeeded)
                {
                    return Results.ValidationProblem(
                        createUserResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
                }

                var userCreatedEvent = new UserCreatedEvent(userToCreate.Id, userToCreate.Email);
                await mediator.Publish(userCreatedEvent);
            }

            await userManager.AddLoginAsync(userToCreate, info);
            var userRoles = await userManager.GetRolesAsync(userToCreate);
            var token = GenerateJwtToken(userToCreate, userRoles.ToList(), config);
            return Results.Ok(new { Token = token });
        });

        group.MapPost("/generate-and-create-test-user", async (
            TestTokenRequest request,
            IConfiguration config,
            IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IMediator mediator) =>
        {
            if (!env.IsDevelopment())
            {
                return Results.NotFound();
            }

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = request.UserId,
                    Email = request.Email,
                    UserName = request.UserName,
                    EmailConfirmed = true
                };

                var createUserResult = await userManager.CreateAsync(user);

                if (!createUserResult.Succeeded)
                {
                    return Results.ValidationProblem(
                        createUserResult.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
                }

                var userCreatedEvent = new UserCreatedEvent(user.Id, user.Email);
                await mediator.Publish(userCreatedEvent);
            }

            if (request.Roles.Any())
            {
                foreach (var roleName in request.Roles)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                    }
                }

                await userManager.AddToRolesAsync(user, request.Roles);
            }


            var roles = await userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles.ToList(), config);

            return Results.Ok(new
            {
                Message = "User found or created successfully.",
                UserId = user.Id,
                Token = token
            });
        }).WithTags("Testing");
    }

    private static string GenerateJwtToken(ApplicationUser user, List<string> roles, IConfiguration config)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}