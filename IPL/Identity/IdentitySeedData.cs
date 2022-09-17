using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace IPL.Identity;
public class IdentitySeedData
{
    private static async Task SeedUsers(UserManager<AppUser> userManager) {
        if(!userManager.Users.Any()) {
            var user = new AppUser {
                PreferredName = "Bob",
                Email = "bob@test.com",
                UserName = "bob@test.com"
            };
            await userManager.CreateAsync(user, "Test1234!");
        }
    }
}
