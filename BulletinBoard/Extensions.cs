using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.Extensions
{
    public static class ExtensionsMethod
    {
        public enum State
        {
            LOADING,
            SUCCESS,
            ACCESS_BLOCKED
        }
        public static class Consts
        {
            public static readonly Guid DefaultGroupId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            public static readonly string DefaultImageFolder = "images";
            public static readonly string DefaultAvatarPath = "no_avatar.png";
            public static readonly string DefaultGroupAvatarPath = "no_group.svg";
            public static readonly int MaxImagesPerBulletin = 5;
            public static readonly int MaxFileSize = 5 * 1024 * 1024;
        }
        public static string Avatar(Image? img) => img == null ? Consts.DefaultAvatarPath : img.Path();
        public static string GroupImage(Image? img) => img == null ? Consts.DefaultGroupAvatarPath : img.Path();
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
                _ = CreateRole(services, roleName);
            }
        }
        private static async Task CreateRole(IServiceCollection services, string roleName)
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
            var img = new Image(ExtensionsMethod.Consts.DefaultGroupId)
            {
                Extension = "svg",
            };
            var group = new Group()
            {
                Id = ExtensionsMethod.Consts.DefaultGroupId,
                Name = "Main",
                Description = "Main application group",
                PublicListed = true,
                Image = img
            };   // Group 0 by default is main group (can be accesed by anyone)

            if (!dbContext.Groups.Any(g => g.Id == group.Id))
                dbContext.Groups.Add(group); ;

            dbContext.SaveChanges();
        }

    }
}
