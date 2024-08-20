using HotelBooking.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<BloggingContext>(options =>
    options.UseNpgsql("postgresql://Applikation:riDk5X6BYPhb@ep-bitter-salad-a2i17tas.eu-central-1.aws.neon.tech/H2%20HotelBooking?sslmode=require"));

builder.Services.AddTransient<DatabaseInitializer>(provider =>
    new DatabaseInitializer("postgresql://Applikation:riDk5X6BYPhb@ep-bitter-salad-a2i17tas.eu-central-1.aws.neon.tech/H2%20HotelBooking?sslmode=require"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Initialize the database
using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
var databaseInitializer = serviceScope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
databaseInitializer.Initialize();

app.Run();