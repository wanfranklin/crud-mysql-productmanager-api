namespace ProductManager.API.Dtos
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}
