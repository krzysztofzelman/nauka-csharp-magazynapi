using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

[ApiController]
[Route("api/[controller]")]
public class PrzedmiotyController : ControllerBase
{
    [HttpGet]
        public List<Przedmiot> GetPrzedmioty()
        {
            List<Przedmiot> lista = new List<Przedmiot>();
            string connectionString = @"Data Source=D:\Dane\Projekty\NaukaCSharp\Magazyn\magazyn.db";

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
}
    