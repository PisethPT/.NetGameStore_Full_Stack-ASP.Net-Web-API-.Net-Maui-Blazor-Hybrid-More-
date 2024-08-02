using Microsoft.EntityFrameworkCore;

namespace GameStore.API;

public static class GameEndpoints
{
    private const string GameEndpoint = "GetGame";
    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        // get all games
        group.MapGet("/", async (DataContext dbContext) =>
            await dbContext.Games
                           .Include(game => game.Genre)
                           .Select(game => game.ToGameDto())
                           .AsNoTracking()
                           .ToListAsync());

        // post game
        group.MapPost("/add", async (NewGameDto newGame, DataContext dbContext) =>
        {
            Game game = newGame.ToGameEntity();
            game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GameEndpoint, new { Id = newGame.Id }, game.ToGameDto());
        });

        // get game by id
        group.MapGet("/{Id}", async (int Id, DataContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(Id);
            return game is null ? Results.NoContent() : Results.Ok(game.ToGameDetailDto());
        }).WithName(GameEndpoint);

        //put game by id
        group.MapPut("/update/{Id}", async (int Id, NewGameDto game, DataContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(Id);
            if (existingGame is null)
                return Results.NoContent();
            dbContext.Entry(existingGame)
                     .CurrentValues
                     .SetValues(game.ToGameEntity(Id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // delete game by id
        group.MapDelete("/delete/{Id}", async (int Id, DataContext dbContext) =>
        {
            await dbContext.Games.Where(game => Id == game.Id)
                                 .ExecuteDeleteAsync();
            return Results.NoContent();
        });


        return group;
    }
}
