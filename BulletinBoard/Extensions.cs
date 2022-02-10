using BulletinBoard.Data;
using BulletinBoard.Model;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard
{
    public static class Extensions
    {

        public static void RunAppSetup(this IServiceCollection services)
        {
            CreateRoles(services);
            AddBoards(services);
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
        private static void AddBoards(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var board = new Board()
            {
                Guid = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Main",
                Description = "Main board",
                PublicListed = true,
            };   // Board 1 by default is main board (can be accessed by anyone)

            if (!dbContext.Boards.Any(g => g.Guid == board.Guid))
                dbContext.Boards.Add(board);

            dbContext.SaveChanges();
        }

    }
}
