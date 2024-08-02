using System.ComponentModel.DataAnnotations;

namespace GameStore.API;

public record class NewGameDto
(
    int Id,
    [Required] [StringLength(50)]
    string Name,
    int GenreId,
    [Range(1,100)]
    decimal Price,
    DateOnly ReleaseDate
);
