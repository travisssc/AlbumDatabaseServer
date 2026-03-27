using MudBlazor.Services;
using AlbumDatabaseServer.Areas.Identity;
using AlbumDatabaseServer.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MudBlazor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration["waveformd_connection"];
builder.Services.AddMudServices();
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=waveformd.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<AlbumService>();
builder.Services.AddSingleton<SongService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<GenreService>();


var app = builder.Build();
// Auto-login for demo
app.Use(async (context, next) =>
{
    var signInManager = context.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
    var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();

    var user = await userManager.FindByEmailAsync("traviscampbell7777@gmail.com");
    if (user != null && !context.User.Identity.IsAuthenticated)
    {
        await signInManager.SignInAsync(user, isPersistent: true);
    }

    await next();
});
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
