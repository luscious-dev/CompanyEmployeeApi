﻿namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Corspolicy", builder =>
                {
                    // In production...
                    // Instead of AllowAnyOrigin, use WithOriginss("https://example.com")
                    // Instead of AllowAnyMethod, use WithMethods("POST", "GET")
                    // Instead of AllowAnyHeader, use WithHeaders("accept", "content-type")
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        // Configure IIS integration to help us with deployment to IIS
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
                // No properties set because we are fine with the defaults
            });
        }
    }

    
}