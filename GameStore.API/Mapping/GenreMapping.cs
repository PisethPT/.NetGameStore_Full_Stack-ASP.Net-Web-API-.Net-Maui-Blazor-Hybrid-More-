namespace GameStore.API;

public static class GenreMapping
{
    public static GenreDto ToGenreDto(this Genre genre)
    {
        return new GenreDto(
            genre.Id,
            genre.Name
        );
    }
}
