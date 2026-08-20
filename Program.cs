using Microsoft.Data.Sqlite;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

string cs = builder.Configuration.GetConnectionString("MagazynDb");
using (SqliteConnection connection = new SqliteConnection(cs))
{
    connection.Open();
    SqliteCommand command = connection.CreateCommand();
    command.CommandText = "CREATE TABLE IF NOT EXISTS Partie (Id INTEGER PRIMARY KEY AUTOINCREMENT, PrzedmiotId INTEGER, Ilosc INTEGER, Cena REAL, Data TEXT, Status TEXT)";
    command.ExecuteNonQuery();
}

app.Run();
