using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Identity;

namespace IPL.Identity;
public class AppIdentityContext : IdentityDbContext<AppUser>
{
    public AppIdentityContext(DbContextOptions<AppIdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
    }
    
}