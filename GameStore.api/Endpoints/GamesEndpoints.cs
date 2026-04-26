using System;
using GameStore.api.Data;
using GameStore.api.Dtos;
using GameStore.api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Endpoints;

// Contains endpoint mappings for managing games in the GameStore API
public static class GamesEndpoints
{
    private const string GetGameEndpointName = "GetGame";

    // Maps all the endpoints related to games to the WebApplication
    public static void MapGamesEndpoints(this WebApplication app)
    {
        // GET /games
        // Retrieves a list of all games, including their genres, as a summary
        // Why: This endpoint is useful for displaying a list of games to users, such as in a store or catalog view.
        app.MapGet("/games", async (GameStoreContext dbContext) => await dbContext.Games.Include(game => game.Genre)
        .Select(game => new GameSummaryDto(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        )).AsNoTracking().ToListAsync());
        

        // GET /games/{id}
        // Retrieves detailed information about a specific game by its ID
        // Why: This endpoint allows users to view detailed information about a single game, such as when they click on a game in the catalog.
        app.MapGet("/games/{id}", async (int id, GameStoreContext dbContext) => 
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        }).WithName(GetGameEndpointName);

        // POST /games
        // Creates a new game and adds it to the database
        // Why: This endpoint is used to add new games to the store, such as when an admin adds a new product.
        app.MapPost("/games", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreID,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );
            return Results.CreatedAtRoute(
                GetGameEndpointName,
                new { id = gameDto.Id },
                gameDto
            );
        });

        // PUT /games/{id}
        // Updates an existing game by its ID
        // Why: This endpoint allows admins to modify the details of an existing game, such as updating its price or release date.
        app.MapPut("/games/{id}", async(int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE /games/{id}
        // Deletes a game by its ID
        // Why: This endpoint is used to remove games from the store, such as when a product is discontinued.
        app.MapDelete("/games/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }
}