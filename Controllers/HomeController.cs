﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Context;
using App.Models;
using sfood.Models;

namespace sfood.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var id = HttpContext.Session.GetInt32("UsuarioId");
        if (id != null)
        {
            Usuario UserObj = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);

            ViewBag.UserPfp = UserObj.Imagem;
        }


        return View(_context.Banners.ToList());
    }
    public IActionResult Produto()
    {
        var id = HttpContext.Session.GetInt32("UsuarioId");
        if (id != null)
        {
            Usuario UserObj = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);

            ViewBag.UserPfp = UserObj.Imagem;
        }

        var produtos = _context.Produtos.ToList();
        var categorias = _context.Categorias.ToList();

        var viewModel = new ProdutoCategoriaViewModel
        {
            Produtos = produtos,
            Categorias = categorias
        };

        // Passe o viewModel para a exibição
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        var a = HttpContext.Session.GetInt32("UsuarioId");
        if (a != null)
        {
            Usuario UserObj = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == a);

            ViewBag.UserPfp = UserObj.Imagem;
        }

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

    public async Task<IActionResult> SingUp(int? id)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SingUp([Bind("UsuarioId,Nome,CEP,Imagem,Login,Senha")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    public IActionResult Carrinho()
    {
        var id = HttpContext.Session.GetInt32("UsuarioId");
        if (id != null)
        {
            Usuario UserObj = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);

            ViewBag.UserPfp = UserObj.Imagem;
        }
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}


