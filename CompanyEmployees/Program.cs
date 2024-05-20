using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

// 1. Creates an instance of WebApplicationBuilder
// 2. Adds Configuration to the project using builder.Configuration
// 3. Loggin configuration with builder.Logging
// 4. IHostBuilder and IWebHostBuilder configuration
var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();  // Adds strict-transport-security header

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
