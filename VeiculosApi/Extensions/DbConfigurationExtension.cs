using System;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;

namespace VeiculosApi.Extensions;

public static class DbConfigurationExtension
{
    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        string connString = builder.Configuration.GetConnectionString("SqlServer");
        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connString);
        });
    }
}
