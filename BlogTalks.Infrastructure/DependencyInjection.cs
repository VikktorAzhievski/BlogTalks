using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure.Data.DataContext;
using BlogTalks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogTalks.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default)));


            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            return services;
        }
    }
}
