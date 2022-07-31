using CommunityToolkit.Mvvm.DependencyInjection;
using RadioThermWebApp.Data;
using RadioThermLib;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RadioThermLib.Services;
using RadioThermWebApp.Services;
using RadioThermLib.ViewModels;
using RadioThermLib.ProvidedServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<ISettingsService, JsonSettingService>();
builder.Services.AddSingleton<IViewService, ViewService>();
builder.Services.AddSingleton<IThermostatService, ThermostatService>();
builder.Services.AddTransient<ThermostatViewModel>();
builder.Services.AddTransient<ThermostatWidgetViewModel>();
builder.Services.AddTransient<DiscoveryWidgetViewModel>();

var app = builder.Build();

// hook up the CommunityToolkit Ioc to the .net core stuff
Ioc.Default.ConfigureServices(builder.Services.BuildServiceProvider());

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
