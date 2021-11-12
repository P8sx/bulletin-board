using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard
{
    public static class Extensions
    {
        public static void CreateRoles(this IServiceCollection services)
        {
            foreach (var roleName in Enum.GetNames(typeof(RoleValue)))
            {
                CreateRole(services, roleName);
            }
        }
        private static async void CreateRole(IServiceCollection services, string roleName)
        {
            var serviceProvider = services.BuildServiceProvider();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new Role(roleName));
            }
        }
    }
}
