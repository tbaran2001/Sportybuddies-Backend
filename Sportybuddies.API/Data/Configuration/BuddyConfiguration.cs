namespace Sportybuddies.API.Data.Configuration;

public class BuddyConfiguration : IEntityTypeConfiguration<Buddy>
{
    public void Configure(EntityTypeBuilder<Buddy> builder)
    {
        builder.HasOne(b => b.Profile)
            .WithMany()
            .HasForeignKey(b => b.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.MatchedProfile)
            .WithMany()
            .HasForeignKey(b => b.MatchedProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}