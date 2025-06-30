namespace Sportybuddies.API.Modules.Profiles.Features.Commands;

public record CreateProfileCommand(Guid ProfileId, string Name, DateTimeOffset DateOfBirth, Gender Gender,Guid UserId)
    : ICommand<CreateProfileResult>;

public record CreateProfileResult(ProfileDto Profile);

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.DateOfBirth)
            .Must(dateOfBirth => DateTimeOffset.Now.Year - dateOfBirth.Year >= 18);
        RuleFor(x => x.Gender)
            .IsInEnum();
    }
}

internal class CreateProfileCommandHandler(IProfilesRepository profilesRepository)
    : ICommandHandler<CreateProfileCommand, CreateProfileResult>
{
    public async Task<CreateProfileResult> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = Profile.Create(
            command.ProfileId,
            command.Name,
            command.DateOfBirth,
            command.Gender,
            command.UserId
        );

        await profilesRepository.AddProfileAsync(profile, cancellationToken);

        var profileDto = profile.Adapt<ProfileDto>();

        return new CreateProfileResult(profileDto);
    }
}