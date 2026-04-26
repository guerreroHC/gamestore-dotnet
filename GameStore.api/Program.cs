using GameStore.api.Data;
using GameStore.api.Endpoints;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();

app.MapGenresEndpoints();

app.MigrateDb();

app.Run();
