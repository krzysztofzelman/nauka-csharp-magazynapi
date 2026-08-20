using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

[ApiController]
[Route("api/[controller]")]
public class PrzedmiotyController : ControllerBase
{
    private readonly IConfiguration _config;

    public PrzedmiotyController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public List<Przedmiot> GetPrzedmioty() // GET: api/przedmioty
    {
        List<Przedmiot> lista = new List<Przedmiot>();
        string connectionString = _config.GetConnectionString("MagazynDb");

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Nazwa, Ilosc, Cena FROM Przedmioty";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Przedmiot
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nazwa = Convert.ToString(reader["Nazwa"]) ?? "",
                    Ilosc = Convert.ToInt32(reader["Ilosc"]),
                    Cena = Convert.ToDecimal(reader["Cena"])
                });
            }
        }
        return lista;
    }
    [HttpPost]
    public void DodajPrzedmiot(Przedmiot nowy) // POST: api/przedmioty
    {
        string connectionString = _config.GetConnectionString("MagazynDb");
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Przedmioty (Nazwa, Ilosc, Cena) VALUES (@Nazwa, @Ilosc, @Cena)";
            command.Parameters.AddWithValue("@Nazwa", nowy.Nazwa);
            command.Parameters.AddWithValue("@Ilosc", nowy.Ilosc);
            command.Parameters.AddWithValue("@Cena", nowy.Cena);
            command.ExecuteNonQuery();
        }
    }

    [HttpPut("{id}")]
    public void EdytujPrzedmiot(int id, Przedmiot nowy) // PUT: api/przedmioty/5
    {
        string connectionString = _config.GetConnectionString("MagazynDb");

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Przedmioty SET Nazwa =@Nazwa, Ilosc = @Ilosc, Cena = @Cena WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Nazwa", nowy.Nazwa);
            command.Parameters.AddWithValue("@Ilosc", nowy.Ilosc);
            command.Parameters.AddWithValue("@Cena", nowy.Cena);
            command.ExecuteNonQuery();
        }
    }

    [HttpDelete("{id}")]
    public void UsunPrzedmiot(int id)
    {
        string connectionString = _config.GetConnectionString("MagazynDb");
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Przedmioty WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}
    