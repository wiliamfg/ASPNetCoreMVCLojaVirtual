﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Libraries.Filtro;
using LojaVirtual.Libraries.Lang;
using LojaVirtual.Models;
using LojaVirtual.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class CategoriaController : Controller
    {
        private ICategoriaRepository _categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult Index(int? pagina)
        {
            var categorias = _categoriaRepository.ObterTodasCategorias(pagina);
            return View(categorias);
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm]Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaRepository.Cadastrar(categoria);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(int Id)
        {
            var categoria = _categoriaRepository.ObterCategoria(Id);

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Where(c => c.Id != Id).Select(c => new SelectListItem(c.Nome, c.Id.ToString()));

            return View(categoria);
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm]Categoria categoria, int Id)
        {
            if (ModelState.IsValid)
            {
                _categoriaRepository.Atualizar(categoria);

                TempData["MSG_S"] = Mensagem.MSG_S001;

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _categoriaRepository.ObterTodasCategorias().Where(c => c.Id != Id).Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Excluir(int Id)
        {
            _categoriaRepository.Excluir(Id);

            TempData["MSG_S"] = Mensagem.MSG_S002;

            return RedirectToAction(nameof(Index));
        }
    }
}