using Chat.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Identity
{
    public class ChatIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ChatIdentityDbContext()
        {
            
        }

        public ChatIdentityDbContext(DbContextOptions<ChatIdentityDbContext> options) : base(options)
        {
            
        }
    }
}
