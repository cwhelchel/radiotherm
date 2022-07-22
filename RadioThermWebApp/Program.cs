using RadioThermWebApp.Data;
using RadioThermLib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RadioThermLib.Services;
using RadioThermWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<ISettingsService, JsonSettingService>();
builder.Services.AddSingleton<IViewService, ViewService>();
builder.Services.AddTransient<RadioThermLib.ViewModels.DiscoveryWidgetViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
