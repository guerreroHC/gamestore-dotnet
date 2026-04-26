using GameStore.api.Data;
using GameStore.api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        //GET /genres
        app.MapGet("/genres", async (GameStoreContext dbContext) =>
            await dbContext.Genres.Select(g => new GenreDTO(g.Id, g.Name)).AsNoTracking().ToListAsync()
        );
    }
}
    