using System;
using GameStore.api.Data;
using GameStore.api.Dtos;
using GameStore.api.Models;

namespace GameStore.api.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
    new(
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new(
        2,
        "Final Fantasy VII",
        "RPG",
        69.99M,
        new DateOnly(2024,2,29)
    ),
    new(
        3,
        "Astro Bot",
        "Platformer",
        59.99M,
        new DateOnly(2024,9,6)
    )
];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        //GET /games
        app.MapGet("/games", () => games);

        //GET /games/{id}
        app.MapGet("/games/{id}", (int id) => 
        {
            var game = games.Find(games => games.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        //POST /games
        app.MapPost("/games", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreID,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

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

        //PUT /games/1
        app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(games => games.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
             id,
             updatedGame.Name,
             updatedGame.Genre,
             updatedGame.Price,
             updatedGame.ReleaseDate
         );
         return Results.NoContent();
     });

     //DELETE /games/1
     app.MapDelete("/games/{id}", (int id) =>
     {
         games.RemoveAll(games => games.Id == id); 
         return Results.NoContent();
     });
    }

}