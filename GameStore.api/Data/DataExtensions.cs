using GameStore.api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Data;

// Provides extension methods for data-related operations
public static class DataExtensions
{
    // Extension method to apply pending migrations to the database at runtime
    // Ensures the database schema is up-to-date when the application starts
    public static void MigrateDb(this WebApplication app)
    {
        // Create a scope to access scoped services
        using var scope = app.Services.CreateScope();

        // Retrieve the GameStoreContext from the service provider
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        // Apply any pending migrations to the database
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");
        builder.Services.AddScoped<GameStoreContext>();
        builder.Services.AddSqlite<GameStoreContext>(
    connString,
    optionsAction: options => options.UseSeeding((context, _) =>
    {
        if (!context.Set<Genre>().Any())
        {
            context.Set<Genre>().AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Platformer" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Sports" }
            );

            context.SaveChanges();
        }
    })
);
    }
}