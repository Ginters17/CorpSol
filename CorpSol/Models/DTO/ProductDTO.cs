namespace CorpSol.Models.DTO
{
    public class ProductDto
    {
        public string ItemName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPriceWithVat { get; set; }
    }
}
