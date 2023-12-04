using App.Models;
namespace App.Models;
public class ProdutoBannerViewModel
{
    public IEnumerable<Produto> ListaProdutos { get; set; }
    public IEnumerable<Banner> ListaBanners { get; set; }
}