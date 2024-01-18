using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using library.Models;

namespace library.Controllers
{
    public class LivreController:Controller
    {
        private readonly ApplicationDbContext _context;

        public LivreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            var livres = await _context.Livres
                .Where(l => l.Titre.Contains(searchTerm))
                .ToListAsync();

            return View("Index", livres);
        }
        public async Task<IActionResult> Index(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return _context.Livres != null ?
                            View(await _context.Livres.ToListAsync()) :
                            Problem("Entity set 'ApplicationDbContext.Livres' is null.");
            }
            else
            {
                return await Search(searchTerm);
            }
        }


        public IActionResult All()
        {
            var livres = _context.Livres.ToList();
            return View(livres);
        }



        //-----------------Ajouter----------------
        public IActionResult AjouterLivre()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjouterLivre(Livre livre)
        {
            _context.Livres.Add(livre);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        //-----------------Details----------------
        public IActionResult Details(int id)
        {
            var livre = _context.Livres.Find(id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        //-----------------DetailsAD----------------
        public IActionResult DetailsAD(int id)
        {
            var livre = _context.Livres.Find(id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }


        //-----------------EDIT----------------
        public IActionResult Edit(int id)
        {
            var livre = _context.Livres.Find(id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        [HttpPost]
        public IActionResult Edit(Livre livre)
        {
            _context.Entry(livre).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        //-----------------Delete----------------
        public IActionResult Delete(int id)
        {
            var livre = _context.Livres.Find(id);
            if (livre == null)
            {
                return NotFound();
            }

            _context.Livres.Remove(livre);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
