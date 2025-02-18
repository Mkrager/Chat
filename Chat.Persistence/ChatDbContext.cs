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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatDbContext).Assembly);

            modelBuilder.Entity<Message>().HasData(new Message
            {
                Content = "First message",
                Id = Guid.Parse("c99b6971-83a4-4b32-9a72-e7cf83f47c6f"),
                UserId = "d385ac98-8c90-4946-9ab3-27f821fd7623",
                ReceiverUserId = "6e02e7bd-8f2e-4c25-9696-dad78a1307cb"
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
