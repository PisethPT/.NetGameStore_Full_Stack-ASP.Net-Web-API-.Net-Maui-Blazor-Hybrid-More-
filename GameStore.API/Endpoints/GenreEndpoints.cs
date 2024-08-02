using Microsoft.EntityFrameworkCore;

namespace GameStore.API;

public static class GenreEndpoints
{
    public static RouteGroupBuilder MapGenreEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("genres").WithParameterValidation();
        group.MapGet("/", async (DataContext dbContext) =>
        await dbContext.Genres
                       .Select(genre => genre.ToGenreDto())
                       .AsNoTracking()
                       .ToListAsync());

        return group;     
    }
}
