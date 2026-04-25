using GameStore.api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Data;

// Represents the database context for the GameStore application
// Inherits from DbContext to provide Entity Framework Core functionality
public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    // DbSet for accessing and managing Game entities in the database
    public DbSet<Game> Games => Set<Game>();

    // DbSet for accessing and managing Genre entities in the database
    public DbSet<Genre> Genres => Set<Genre>();
}