using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Extensions
{
    public enum PageState
    {
        LOADING,
        SUCCESS,
        ACCESS_BLOCKED
    }
    public static class Extensions
    {
        public static void RunAppSetup(this IServiceCollection services)
        {
            CreateRoles(services);
            AddGroups(services); 
        }
        private static void CreateRoles(this IServiceCollection services)
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
        private static void AddGroups(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var img = new Image(Guid.Parse("00000000-0000-0000-0000-000000000001"))
            {
                Extension = "svg",
            };
            var group = new Group()
            {
                Id = 1,
                Name = "Main",
                Description = "Main application group",
                Public = true,
                Image = img.SetGroup(1)
            };   // Group 0 by default is main group (can be accesed by anyone)

            if (!dbContext.Groups.Any(g => g.Id == group.Id))
                dbContext.Groups.Add(group); ;

            dbContext.SaveChanges();
        }
    }
}
