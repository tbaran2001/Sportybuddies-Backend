using Sportybuddies.API.Common.Exceptions.Handler;
using Sportybuddies.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCarter();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
    configuration.AddOpenBehavior(typeof(TransactionBehavior<,>));
});
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddScoped<ISportsRepository, SportsRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.MapCarter();
app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.Run();
