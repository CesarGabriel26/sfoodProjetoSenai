using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models;

namespace sfood.Models;
public class ProdutoCategoriaViewModel
{
    public IEnumerable<Produto> Produtos { get; set; }
    public IEnumerable<Categoria> Categorias { get; set; }
}