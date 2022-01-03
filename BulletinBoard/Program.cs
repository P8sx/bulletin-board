using BulletinBoard.Model;
using BulletinBoard.Areas.Identity;
using BulletinBoard.Data;
using BulletinBoard.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Extensions;
using MudBlazor.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options
           .UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 35))), ServiceLifetime.Transient);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddTransient<IBulletinService,BulletinService>();
builder.Services.AddTransient<IUserService,UserService>();
builder.Services.AddTransient<IGroupService,GroupService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.RunAppSetup();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles( new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if(ctx.Context.Request.Path.StartsWithSegments("/images/group"))
        {
            ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
            ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ctx.Context.Response.ContentLength = 0;
            ctx.Context.Response.Body = Stream.Null;
        }
    }
});
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
