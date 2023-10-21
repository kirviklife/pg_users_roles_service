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

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseDeveloperExceptionPage();

app.UseStatusCodePagesWithRedirects("/error/{0}");
app.UseStatusCodePagesWithReExecute("/error/{0}");
app.Map("/error/{statusCode}", (int statusCode) => $"Error. Status Code: {statusCode}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
