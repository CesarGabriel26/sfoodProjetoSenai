using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int IdPaises { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        [Display(Name = "Nome do produto")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Imagem é obrigatório")]
        [Display(Name = "Imagem do produto")]
        public string Imagem { get; set; }
        [Required(ErrorMessage = "preco é obrigatório")]
        [Display(Name = "preco do produto")]
        public decimal preco { get; set; }
        [Required(ErrorMessage = "obrigatório")]
        [Display(Name = "Produto em estoque")]
        public bool EmExtoque { get; set; }

        public float Rate { get; set; } = 0;

        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}

