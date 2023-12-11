using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Context;
using App.Models;
using App.Filters;
using System.Xml;
using System.Text;
using X.PagedList;

namespace sfood.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class CategoriaController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Categoria
        public IActionResult Index(string botao,string? txtFiltro, string? celOrdenacao, int pagina = 1) 
        {

            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");

            Usuario UserObj = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == UsuarioId);

            ViewBag.UserPfp = UserObj.Imagem;

            int PageSize = 5;

            IQueryable<Categoria> lista = _context.Categorias;

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
            
            if(botao == "XML")
            {
                return ExportarXML(lista.ToList());
            }
            else if(botao == "Json")
            {
                return ExportarJson(lista.ToList());
            }
            
            return View(lista.ToPagedList(pagina, PageSize));

        }

                        
        private IActionResult ExportarJson(List<Categoria> lista)
        {
            var json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine(" \"Categoria\": [");
            int total = 0;
            foreach(var item in lista)
            {
                json.AppendLine("         {");
                json.AppendLine($"         \"Nome\": \"{item.Nome}\",");
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
            "application/json", "dados_categoria.json");
        }

        private IActionResult ExportarXML(List<Categoria> lista)
        {
            var arquivo = new StringWriter();
            var xml = new XmlTextWriter(arquivo)
            {
                Formatting = Formatting.Indented
            };
            xml.WriteStartDocument();
            xml.WriteStartElement("Dados");
            xml.WriteStartElement("Categorias");

            foreach (var item in lista)
            {
                xml.WriteStartElement("Categoria");
                xml.WriteElementString("Nome", item.Nome);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
            xml.WriteEndElement();

            return File(Encoding.UTF8.GetBytes(arquivo.ToString()),
            "appilication/xml", "dados_categoria.xml");
        }


        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nome")] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nome")] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId))
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
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'AppDbContext.Categorias'  is null.");
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return (_context.Categorias?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }
    }
}
