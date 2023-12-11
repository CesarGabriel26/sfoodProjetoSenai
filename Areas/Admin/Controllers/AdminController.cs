using Microsoft.AspNetCore.Mvc;
using App.Filters;
using App.Models;
using App.Context;
using System.Text.Json;

namespace sfood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _BancoDados;

        public AdminController(AppDbContext BancoDados)
        {
            _BancoDados = BancoDados;
        }
        
        public IActionResult Index()
        {
            var UsuarioId = HttpContext.Session.GetInt32("UsuarioId");

            Usuario UserObj = _BancoDados.Usuarios.FirstOrDefault(u => u.UsuarioId == UsuarioId);

            ViewBag.UserName = UserObj.Nome;
            ViewBag.UserCep = UserObj.CEP;
            ViewBag.UserPass = UserObj.Senha;
            ViewBag.UserLog = UserObj.Login;
            ViewBag.UserPfp = UserObj.Imagem;

            return View();
        }
    }
}