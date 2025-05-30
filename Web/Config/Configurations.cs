﻿using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Base;
using Infrastructure.DataAccess.ADONET;
using Infrastructure.DataAccess.Dapper;
using Infrastructure.DataAccess.EntityFramework;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Web.Config
{
    public static class Configurations
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllersWithViews();

            //Choose the repository implementation

            //services.UseEfRepositories();
            services.UseAdoNetRepositories();
            //services.UseDapperRepositories();

            // Register application services
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
        }

        private static void UseAdoNetRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactoryADONET, DbConnectionFactoryADONET>(provider =>
            {
                var connString = string.Concat("Data Source=", Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Database", "database.db"));
                return new DbConnectionFactoryADONET(connString);
            });

            // Register repositories
            services.AddScoped<IEmployeeRepository, EmployeeRespositoryADO>();
            services.AddScoped<IRoomRepository, RoomRepositoryADO>();
            services.AddScoped<IReservationRepository, ReservationRepositoryADO>();
        }

        private static void UseDapperRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactoryDapper, DbConnectionFactoryDapper>(provider =>
            {
                var connString = string.Concat("Data Source=", Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Database", "database.db"));
                return new DbConnectionFactoryDapper(connString);
            });

            // Register repositories
            services.AddScoped<IEmployeeRepository, EmployeeRespositoryDapper>();
            services.AddScoped<IRoomRepository, RoomRepositoryDapper>();
            services.AddScoped<IReservationRepository, ReservationRepositoryDapper>();
        }

        private static void UseEfRepositories(this IServiceCollection services)
        {
            var connString = string.Concat("Data Source=", Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Database", "database.db"));

            services.AddDbContext<DBContext>(options =>
            {
                options.UseSqlite(connString);
            });

            // Register repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepositoryEF>();
            services.AddScoped<IRoomRepository, RoomRepositoryEF>();
            services.AddScoped<IReservationRepository, ReservationRepositoryEF>();
        }
    }
}
