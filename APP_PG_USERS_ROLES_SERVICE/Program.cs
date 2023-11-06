using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using DBContext = APP_PG_USERS_ROLES_SERVICE.Models.DataContext;
using PostgresContext = APP_PG_USERS_ROLES_SERVICE.Models.PostgresContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
});

builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
	CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.LoginPath = "/Home/Autorize";
		option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
	}
	);

builder.Services.AddSession();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseDeveloperExceptionPage();

app.UseStatusCodePagesWithRedirects("/error/{0}");
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Autorize}/{id?}");

app.Run();
