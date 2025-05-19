using Red_and_Green_boxes.Models.Config;
using Red_and_Green_boxes.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure GitHub settings from appsettings.json
builder.Services.Configure<GitHubSettings>(
    builder.Configuration.GetSection(GitHubSettings.SectionName));

// Register services
builder.Services.AddSingleton<ConfigurationHelper>();
builder.Services.AddScoped<IGitHubService, GitHubService>();

// Add memory cache for performance optimization
builder.Services.AddMemoryCache();

// Add HTTP client factory
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
