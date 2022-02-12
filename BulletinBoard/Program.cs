using BulletinBoard.Areas.Identity;
using BulletinBoard.Data;
using BulletinBoard.Model;
using BulletinBoard.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Net;
using BulletinBoard;
using Minio.AspNetCore;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options => options
           .UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 35))), ServiceLifetime.Scoped);

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
builder.Services.AddScoped<IBulletinService, BulletinService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddTransient<IHelperService, HelperService>();
builder.Services.AddSingleton<IValidatorService, ValidatorService>();
builder.Services.AddSingleton<GlobalService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();
builder.Services.RunAppSetup();

builder.Services.AddMinio(options =>
{
    options.AccessKey = builder.Configuration.GetSection("Minio:AccessKey").Value;
    options.SecretKey = builder.Configuration.GetSection("Minio:SecretKey").Value;
    options.Endpoint = builder.Configuration.GetSection("Minio:ServiceURL").Value;
    options.Region = builder.Configuration.GetSection("Minio:Region").Value;
    options.OnClientConfiguration = client =>
    {
        client.WithSSL();
    };
});

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
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
