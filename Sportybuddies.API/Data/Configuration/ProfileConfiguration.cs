namespace Sportybuddies.API.Data.Configuration;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder
            .HasMany(u => u.Sports)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ProfileSports",
                j => j
                    .HasOne<Sport>()
                    .WithMany()
                    .HasForeignKey("SportId"),
                j => j
                    .HasOne<Profile>()
                    .WithMany()
                    .HasForeignKey("ProfileId")
            );

        builder.OwnsOne(p => p.Preferences, preferencesBuilder =>
        {
            preferencesBuilder.Property(p => p.MinAge);
            preferencesBuilder.Property(p => p.MaxAge);
            preferencesBuilder.Property(p => p.MaxDistance);
            preferencesBuilder.Property(p => p.PreferredGender)
                .HasConversion(
                    gender => gender.ToString(),
                    dbGender => (Gender)Enum.Parse(typeof(Gender), dbGender));
        });

        builder.OwnsOne(p => p.Location, locationBuilder =>
        {
            locationBuilder.Property(l => l.Latitude);
            locationBuilder.Property(l => l.Longitude);
            locationBuilder.Property(l => l.Address);
        });

        /*builder
            .HasMany(u=>u.Messages)
            .WithOne(m=>m.Sender)
            .HasForeignKey(m=>m.SenderId);

        builder
            .HasMany(u=>u.Conversations)
            .WithOne(c=>c.Creator)
            .HasForeignKey(c=>c.CreatorId);*/
    }
}