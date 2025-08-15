namespace Sportybuddies.API.Data.Configuration;

public class SportConfiguration: IEntityTypeConfiguration<Sport>
{
    public void Configure(EntityTypeBuilder<Sport> builder)
    {
        builder.HasData(
            new { Id = new Guid("a0a0a0a0-0001-4000-8000-000000000001"), Name = "Gym", Description = "Fitness training and strength building activities in a fitness center environment." },
            new { Id = new Guid("a0a0a0a0-0002-4000-8000-000000000002"), Name = "Boxing", Description = "Combat sport involving striking with fists while wearing protective gloves." },
            new { Id = new Guid("a0a0a0a0-0003-4000-8000-000000000003"), Name = "Surfing", Description = "Water sport of riding waves on a surfboard in ocean or sea environments." },
            new { Id = new Guid("a0a0a0a0-0004-4000-8000-000000000004"), Name = "Basketball", Description = "Team sport played on a rectangular court with a hoop at each end." },
            new { Id = new Guid("a0a0a0a0-0005-4000-8000-000000000005"), Name = "Snowboarding", Description = "Winter sport descending snow-covered slopes on a snowboard." },
            new { Id = new Guid("a0a0a0a0-0006-4000-8000-000000000006"), Name = "Hiking", Description = "Outdoor activity of walking on trails and paths in natural environments." },
            new { Id = new Guid("a0a0a0a0-0007-4000-8000-000000000007"), Name = "Yoga", Description = "Physical, mental, and spiritual practice combining poses, breathing, and meditation." },
            new { Id = new Guid("a0a0a0a0-0008-4000-8000-000000000008"), Name = "Cycling", Description = "Sport of riding bicycles for competition or recreation on road or off-road." },
            new { Id = new Guid("a0a0a0a0-0009-4000-8000-000000000009"), Name = "Swimming", Description = "Individual or team racing sport that requires the use of one's entire body to move through water." },
            new { Id = new Guid("a0a0a0a0-0010-4000-8000-000000000010"), Name = "Tennis", Description = "Racquet sport played individually against a single opponent or between two teams of two players each." },
            new { Id = new Guid("a0a0a0a0-0011-4000-8000-000000000011"), Name = "Running", Description = "Athletic sport involving running over various distances on track, road, or trails." },
            new { Id = new Guid("a0a0a0a0-0012-4000-8000-000000000012"), Name = "Skiing", Description = "Winter sport of sliding down snow-covered slopes on skis." }
        );
    }
}