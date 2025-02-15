using System;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;

namespace VeiculosApi.Extensions;

public static class DbConfigurationExtension
{
    public static void ConfigureDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        string connString = configuration.GetConnectionString("SqlServer");
        service.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connString);
        });
    }

}
