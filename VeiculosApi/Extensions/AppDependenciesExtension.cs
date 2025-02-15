using System;
using VeiculosApi.Services;

namespace VeiculosApi.Extensions;

public static class AppDependenciesExtension
{
    public static void ConfigureDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<CategoryService>();
        builder.Services.AddTransient<VehicleService>();
        builder.Services.AddTransient<PhotoService>();
        builder.Services.AddTransient<SavePhotoService>();
    }
}
