namespace Sportybuddies.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Buddy> Buddies { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        modelBuilder.Entity<IdentityUser>()
            .HasOne<Profile>()
            .WithOne(p => p.User)
            .HasForeignKey<Profile>(p => p.UserId);
        
        base.OnModelCreating(modelBuilder);
    }
}