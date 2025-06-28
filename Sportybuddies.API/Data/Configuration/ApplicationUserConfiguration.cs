namespace Sportybuddies.API.Data.Configuration;

public class ApplicationUserConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne<Profile>()
            .WithOne(p=> p.User)
            .HasForeignKey<Profile>(p => p.UserId);
    }
}