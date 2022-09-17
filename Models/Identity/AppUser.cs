using Microsoft.AspNetCore.Identity;

namespace Models.Identity;
public class AppUser : IdentityUser
{
    public string PreferredName { get; set; }
}