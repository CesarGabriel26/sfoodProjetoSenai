using Microsoft.AspNetCore.Mvc;
using App.Context;
using App.Models;
using System.Xml;
using System.Text;
using X.PagedList;

namespace App.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _BancoDados;
        private readonly string _caminhoPasta;
        public BannerController(AppDbContext BancoDados, IWebHostEnvironment pastaSite)
        {
            _BancoDados = BancoDados;
            _caminhoPasta = pastaSite.WebRootPath;
        }

        public IActionResult ListaBanners(string botao,string? txtFiltro, string? celOrdenacao, int pagina = 1) 
        {

            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");

            Usuario UserObj = _BancoDados.Usuarios.FirstOrDefault(u => u.UsuarioId == UsuarioId);

            ViewBag.UserPfp = UserObj.Imagem;


            int PageSize = 5;

            IQueryable<Banner> lista = _BancoDados.Banners;

            if (botao == "Relatorio")
            {
                PageSize = lista.Count();
            }


            if (txtFiltro != null && txtFiltro != "")
            {
                ViewData["txtFiltro"] = txtFiltro;
                lista = lista.Where(item => item.Titulo.ToLower().Contains(txtFiltro.ToLower()));
            }
            

            if (celOrdenacao == "Titulo")
            {
                lista = lista.OrderBy(item => item.Titulo.ToLower());
            }
            
            else if (celOrdenacao == "Subtitulo")
            {
                lista = lista.OrderBy(item => item.Subtitulo.ToLower());
            }
            
            /*if(botao == "XML")
            {
                return ExportarXML(lista.ToList());
            }
            else if(botao == "Json")
            {
                return ExportarJson(lista.ToList());
            }*/
            
            return View(lista.ToPagedList(pagina, PageSize));

        }

        public IActionResult CriarBanner()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CriarBanner(Banner bannerNovo, IFormFile foto)
        {
            string img = SalvarFoto(foto);
            bannerNovo.Imagem = img;
            _BancoDados.Add(bannerNovo);
            _BancoDados.SaveChanges();
            return RedirectToAction(nameof(ListaBanners));

            return View(bannerNovo);
        }

        public string SalvarFoto(IFormFile imagemSelecionada)
        {
            var nome = Guid.NewGuid().ToString() + imagemSelecionada.FileName;
            var caminhoPastaFotos = _caminhoPasta + "\\fotos";

            if (!Directory.Exists(caminhoPastaFotos))
            {
                Directory.CreateDirectory(caminhoPastaFotos);
            }

            using (var stream = System.IO.File.Create(caminhoPastaFotos + "\\" + nome))
            {
                imagemSelecionada.CopyTo(stream);
            }
            return nome;
        }

        public IActionResult EditarBanner(int id)
        {
            var banner = _BancoDados.Banners.Find(id);
            return View(banner);
        }

        [HttpPost]
        public IActionResult EditarBanner(int id, Banner banner)
        {
            if (ModelState.IsValid)
            {
                _BancoDados.Update(banner);
                _BancoDados.SaveChanges();
                return RedirectToAction(nameof(ListaBanners));
            }
            return View(banner);
        }

        public IActionResult DeletarBanner(int id)
        {
            var banner = _BancoDados.Banners.Find(id);
            System.IO.File.Delete(_caminhoPasta + "\\fotos\\" + banner.Imagem);
            _BancoDados.Banners.Remove(banner);

            _BancoDados.SaveChanges();
            return RedirectToAction(nameof(ListaBanners));
        }
    }
}