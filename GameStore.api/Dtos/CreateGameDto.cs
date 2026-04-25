using System.ComponentModel.DataAnnotations;

namespace GameStore.api.Dtos;

public record CreateGameDto
(
    [Required][StringLength(50)] string Name,
    [Range(1,50)]int GenreID,
    [Range(0, 100)]decimal Price,
    DateOnly ReleaseDate

);
