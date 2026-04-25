namespace GameStore.api.Dtos;

public record GameDetailsDto
(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate

);