using LendWise.Web.Data;
using LendWise.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<LendWiseDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LendWise")));
builder.Services.AddScoped<PortfolioService>();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, "data"));
    var db = scope.ServiceProvider.GetRequiredService<LendWiseDbContext>();
    await DemoDataSeeder.SeedAsync(db);
}

app.Run();

public partial class Program
{
}
