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

namespace sfood.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Usuario
        public IActionResult Index(string botao,string? txtFiltro, string? celOrdenacao, int pagina = 1) 
        {
            int PageSize = 5;

            IQueryable<Usuario> lista = _context.Usuarios;

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

            else if (celOrdenacao == "CEP")
            {
                lista = lista.OrderBy(item => item.CEP);
            }
            else if (celOrdenacao == "Login")
            {
                lista = lista.OrderBy(item => item.Login);
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

                
        private IActionResult ExportarJson(List<Usuario> lista)
        {
            var json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine(" \"Usuario\": [");
            int total = 0;
            foreach(var item in lista)
            {
                json.AppendLine("         {");
                json.AppendLine($"         \"Id\": {item.UsuarioId},");
                json.AppendLine($"         \"Nome\": \"{item.Nome}\",");
                json.AppendLine($"         \"Imagem\": \"{item.Imagem}\"");
                json.AppendLine($"         \"CEP\": \"{item.CEP}\"");
                json.AppendLine($"         \"Login\": \"{item.Login}\"");
                json.AppendLine($"         \"Senha\": \"{item.Senha}\"");
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
            "application/json", "Dados_usuarios.json");
        }

        private IActionResult ExportarXML(List<Usuario> lista)
        {
            var arquivo = new StringWriter();
            var xml = new XmlTextWriter(arquivo)
            {
                Formatting = Formatting.Indented
            };
            xml.WriteStartDocument();
            xml.WriteStartElement("Dados");
            xml.WriteStartElement("Usuarios");

            foreach (var item in lista)
            {
                xml.WriteStartElement("Usuario");
                xml.WriteElementString("Id", item.UsuarioId.ToString());
                xml.WriteElementString("Nome", item.Nome);
                xml.WriteElementString("Imagem", item.Imagem);
                xml.WriteElementString("preco", item.CEP.ToString());
                xml.WriteElementString("EmExtoque", item.Login.ToString());
                xml.WriteElementString("Rate", item.Senha.ToString());
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
            xml.WriteEndElement();

            return File(Encoding.UTF8.GetBytes(arquivo.ToString()),
            "appilication/xml", "dados_usuario.xml");
        }


        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        // GET: Admin/Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Admin/Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nome,CEP,Imagem,Login,Senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Admin/Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Admin/Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nome,CEP,Imagem,Login,Senha")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            return View(usuario);
        }

        // GET: Admin/Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Admin/Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'AppDbContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }
    }
}
