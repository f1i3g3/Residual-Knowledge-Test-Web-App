using AuthorizationProtoServer.Data;
using AuthorizationProtoServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AuthorizationProtoServer.Helpers;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped(x => {
	var apiUrl = new Uri(builder.Configuration["apiUrl"]);

	// use fake backend if "fakeBackend" is "true" in appsettings.json
	if (builder.Configuration["fakeBackend"] == "true")
	{
		var fakeBackendHandler = new FakeBackendHandler(x.GetService<ILocalStorageService>());
		return new HttpClient(fakeBackendHandler) { BaseAddress = apiUrl };
	}

	return new HttpClient() { BaseAddress = apiUrl };
});

var app = builder.Build();

/*
var accountService = app.Services.GetRequiredService<IAccountService>();
await accountService.Initialize();
*/

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
