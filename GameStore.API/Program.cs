using GameStore.API;

var builder = WebApplication.CreateBuilder(args);

var stringConnection = builder.Configuration.GetConnectionString("DbGameStore");
builder.Services.AddSqlite<DataContext>(stringConnection);

var app = builder.Build();

app.MapGameEndpoints();
app.MapGenreEndpoint();

await app.MigrateDbAsync();

app.Run();
