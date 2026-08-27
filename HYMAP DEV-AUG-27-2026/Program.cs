using HYMAP_DEV_AUG_27_2026.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Tambahan untuk SQLite dan Repositories
builder.Services.AddDbContext<Hymap.Infrastructure.Data.AppDbContext>(options =>
    options.UseSqlite("Data Source=hymap_local.db"));

builder.Services.AddScoped<Hymap.Application.Interfaces.IUserRepository, Hymap.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Hymap.Application.Services.AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
