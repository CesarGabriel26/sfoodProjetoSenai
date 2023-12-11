using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Context;
using App.Models;
using System.Xml;
using System.Text;
using X.PagedList;

namespace sfood.Controllers
{
    [Area("Admin")]

    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index(string botao,string? txtFiltro, string? celOrdenacao, int pagina = 1) 
        {
            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");

            Usuario UserObj = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == UsuarioId);

            ViewBag.UserPfp = UserObj.Imagem;

            
            int PageSize = 5;

            IQueryable<Produto> lista = _context.Produtos;

            if (botao == "Relatorio")
            {
                PageSize = lista.Count();
            }


            if (txtFiltro != null && txtFiltro != "")
            {
                ViewData["txtFiltro"] = txtFiltro;
                lista = lista.Where(item => item.Nome.ToLower().Contains(txtFiltro.ToLower()));
            }
            

            if (celOrdenacao == "Nome")
            {
                lista = lista.OrderBy(item => item.Nome.ToLower());
            }
            
            else if (celOrdenacao == "preco")
            {
                lista = lista.OrderBy(item => item.preco);
            }
           
            else if (celOrdenacao == "Rate")
            {
                lista = lista.OrderBy(item => item.Rate);
            }

            if(botao == "XML")
            {
                //chamando o metodo para gerar o XML da lista ja filtrada
                return ExportarXML(lista.ToList());
            }
            else if(botao == "Json")
            {
                return ExportarJson(lista.ToList());
            }
            return View(lista.ToPagedList(pagina, PageSize));
     
        }
        
        private IActionResult ExportarJson(List<Produto> lista)
        {
            var json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine(" \"EmExtoque\": [");
            int total = 0;
            foreach(var item in lista)
            {
                json.AppendLine("         {");
                json.AppendLine($"         \"Id\": {item.IdPaises},");
                json.AppendLine($"         \"Nome\": \"{item.Nome}\",");
                json.AppendLine($"         \"Imagem\": \"{item.Imagem}\"");
                json.AppendLine($"         \"preco\": \"{item.preco}\"");
                json.AppendLine($"         \"EmExtoque\": \"{item.EmExtoque}\"");
                json.AppendLine($"         \"Rate\": \"{item.Rate}\"");
                json.AppendLine($"         \"Categoria\": \"{item.Categoria}\"");
                json.AppendLine("         }");
                total++;
                if (total < lista.Count())
                {
                    json.AppendLine("             ,");
                }
            }
            json.AppendLine("        ]");
            json.AppendLine("}");
            return File(Encoding.UTF8.GetBytes(json.ToString()),
            "application/json", "dados_EmExtoque.json");
        }

        private IActionResult ExportarXML(List<Produto> lista)
        {
            var arquivo = new StringWriter();
            var xml = new XmlTextWriter(arquivo)
            {
                Formatting = Formatting.Indented
            };
            xml.WriteStartDocument();
            xml.WriteStartElement("Dados");
            xml.WriteStartElement("Usuario");

            foreach (var item in lista)
            {
                xml.WriteStartElement("Usuario");
                xml.WriteElementString("Id", item.IdPaises.ToString());
                xml.WriteElementString("Nome", item.Nome);
                xml.WriteElementString("Imagem", item.Imagem);
                xml.WriteElementString("preco", item.preco.ToString());
                xml.WriteElementString("EmExtoque", item.EmExtoque.ToString());
                xml.WriteElementString("Rate", item.Rate.ToString());
                xml.WriteElementString("Categoria", item.CategoriaId.ToString());
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
            xml.WriteEndElement();

            return File(Encoding.UTF8.GetBytes(arquivo.ToString()),
            "appilication/xml", "dados_usuario.xml");
        }


        // GET: Produto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.IdPaises == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome");
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaises,Nome,Imagem,preco,EmExtoque,Rate,CategoriaId")] Produto produto)
        {
            if (true)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaises,Nome,Imagem,preco,EmExtoque,Rate,CategoriaId")] Produto produto)
        {
            if (id != produto.IdPaises)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.IdPaises))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nome", produto.CategoriaId);
            return View(produto);
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.IdPaises == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entity set 'AppDbContext.Produtos'  is null.");
            }
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
          return (_context.Produtos?.Any(e => e.IdPaises == id)).GetValueOrDefault();
        }
    }
}
