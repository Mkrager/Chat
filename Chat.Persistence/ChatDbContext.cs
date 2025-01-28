using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);

            string userId = "845aa16b-fb09-45da-8a8a-fdb1deb48ee8";

            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = userId,
                FirstName = "Max",
                LastName = "Smaga",
                Email = "smaga.max@gmail.com",
                UserName = "Smaga"
            });

            modelBuilder.Entity<Message>().HasData(new Message
            {
                Content = "First message",
                Id = Guid.Parse("c99b6971-83a4-4b32-9a72-e7cf83f47c6f"),
                UserId = userId,               
            });

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken
            = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<Message>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SendDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
