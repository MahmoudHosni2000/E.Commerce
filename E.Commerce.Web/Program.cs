using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Context;
using E_Commerce.Persistence.DbInitializers;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Service.Abstraction;
using E_Commerce.Service.MappingProfile;
using E_Commerce.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace E.Commerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddTransient<ProductPictureUrlResolver>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new ProductProfile()));
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Initialize the database
            using var scope = app.Services.CreateScope();

            // Resolve the IDbInitializer service and call the Initialize method to seed the database
            var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            // Call the Initialize method to seed the database
            initializer.Initialize();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "E-Commerce API v1");
                    options.RoutePrefix = "swagger";
                });
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
