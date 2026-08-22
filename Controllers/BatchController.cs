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
            command.CommandText = "SELECT Id, PrzedmiotId, Ilosc, Cena, Data, Status, BatchNumber FROM Partie";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Batch
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    PrzedmiotId = Convert.ToInt32(reader["PrzedmiotId"]),
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
    public void AddBatch(Batch nowy)
    {
        string connectionString = _configuration.GetConnectionString("MagazynDb");
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Partie (PrzedmiotId, Ilosc, Cena, Data, Status, BatchNumber)" +
                " VALUES (@PrzedmiotId, @Ilosc, @Cena, @Data, @Status, @BatchNumber)";
            command.Parameters.AddWithValue("@PrzedmiotId", nowy.PrzedmiotId);
            command.Parameters.AddWithValue("@Ilosc", nowy.Ilosc);
            command.Parameters.AddWithValue("@Cena", nowy.Cena);
            command.Parameters.AddWithValue("@Data", nowy.Data);
            command.Parameters.AddWithValue("@Status", nowy.Status);
            command.Parameters.AddWithValue("@BatchNumber", nowy.BatchNumber);
            command.ExecuteNonQuery();
        }
    }
}