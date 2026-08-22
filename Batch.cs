public class Batch
{
   public int Id { get; set; }
   public string BatchNumber { get; set; } = "";
   public int PrzedmiotId { get; set; }
   public int Ilosc { get; set; }
   public decimal Cena { get; set; }
   public string? Data { get; set; }
   public string? Status { get; set; }
}