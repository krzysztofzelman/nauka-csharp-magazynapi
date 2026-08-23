using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

[ApiController]
[Route("api/[controller]")]
public class BatchController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public BatchController(IConfiguration config)
    {
        _configuration = config;
    }

    [HttpGet]
    public List<Batch> GetBatches()
    {
        List<Batch> lista = new List<Batch>();
        string connectionString = _configuration.GetConnectionString("MagazynDb");

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Partie.Id, Partie.PrzedmiotId, Przedmioty.Nazwa, Partie.Ilosc," +
                " Partie.Cena, Partie.Data, Partie.Status, Partie.BatchNumber FROM Partie JOIN Przedmioty" +
                " ON Partie.PrzedmiotId = Przedmioty.Id";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Batch
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    PrzedmiotId = Convert.ToInt32(reader["PrzedmiotId"]),
                    Nazwa = Convert.ToString(reader["Nazwa"]) ?? "",
                    Ilosc = Convert.ToInt32(reader["Ilosc"]),
                    Cena = Convert.ToDecimal(reader["Cena"]),
                    Data = Convert.ToString(reader["Data"]),
                    Status = Convert.ToString(reader["Status"]),
                    BatchNumber = Convert.ToString(reader["BatchNumber"]) ?? ""
                });
            }
        }
        return lista;
    }
    [HttpPost]
    public void AddBatch(Batch batch)
    {
        string connectionString = _configuration.GetConnectionString("MagazynDb");
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Partie (PrzedmiotId, Ilosc, Cena, Data, Status, BatchNumber)" +
                " VALUES (@PrzedmiotId, @Ilosc, @Cena, @Data, @Status, @BatchNumber)";
            command.Parameters.AddWithValue("@PrzedmiotId", batch.PrzedmiotId);
            command.Parameters.AddWithValue("@Ilosc", batch.Ilosc);
            command.Parameters.AddWithValue("@Cena", batch.Cena);
            command.Parameters.AddWithValue("@Data", batch.Data);
            command.Parameters.AddWithValue("@Status", batch.Status);
            command.Parameters.AddWithValue("@BatchNumber", batch.BatchNumber);
            command.ExecuteNonQuery();
        }
    }
    [HttpPut("{id}")]
    public void UpdateBatch(int id, Batch batch)
    {
        string connectionString = _configuration.GetConnectionString("MagazynDb");
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Partie SET PrzedmiotId=@PrzedmiotId, Ilosc=@Ilosc, Cena=@Cena, Data=@Data, Status=@Status, BatchNumber=@BatchNumber WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@PrzedmiotId", batch.PrzedmiotId);
            command.Parameters.AddWithValue("@Ilosc", batch.Ilosc);
            command.Parameters.AddWithValue("@Cena", batch.Cena);
            command.Parameters.AddWithValue("@Data", batch.Data);
            command.Parameters.AddWithValue("@Status", batch.Status);
            command.Parameters.AddWithValue("@BatchNumber", batch.BatchNumber);
            command.ExecuteNonQuery();
        }
    }
    [HttpDelete("{id}")]
    public void DeleteBatch(int id)
    {
        string connectionString = _configuration.GetConnectionString("MagazynDb");
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Partie WHERE Id=@Id";
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}