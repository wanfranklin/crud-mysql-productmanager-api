using System.ComponentModel.DataAnnotations;

namespace ProductManager.API.Dtos
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O nome do produto deve ter no máximo 255 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }
    }
}
