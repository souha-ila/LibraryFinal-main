using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using library.Models;

namespace library.Controllers
{
    public class EmpruntsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpruntsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //----------------------- GET: Emprunts
        public async Task<IActionResult> Index()
        {
            return _context.Emprunts != null ?
                        View(await _context.Emprunts.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Emprunts'  is null.");
        }

        //---------------------- GET: Emprunts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Emprunts == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }



        //------------------------- GET: Emprunts/Create
        // GET: Emprunts/Create
        public IActionResult Create()
        {
            // Retrieve the list of available books
            var availableBooks = _context.Livres.Where(l => !l.EstEmprunte).ToList();

            // Pass the list of available books to the view
            ViewBag.AvailableBooks = new SelectList(availableBooks, "Id", "Titre");

            // Retrieve the list of available Abonne
            var availableAbonnes = _context.Abonnes.ToList();

            // Pass the list of available abonnes to the view
            ViewBag.AvailableAbonnes = new SelectList(availableAbonnes, "Id","Id");


            return View();
        }

        // POST: Emprunts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LivreId,AbonneId,DateEmprunt,DateRetour")] Emprunt emprunt)
        {
            if (ModelState.IsValid)
            {
                // Check if the abonné has already borrowed 2 books
                var borrowedBooksCount = _context.Emprunts.Count(e => e.AbonneId == emprunt.AbonneId);
                if (borrowedBooksCount < 2)
                {
                    // Check if the selected book is not currently borrowed
                    var selectedBook = _context.Livres.Find(emprunt.LivreId);
                    if (selectedBook != null && !selectedBook.EstEmprunte)
                    {
                        // Calculate the duration of the emprunt
                        var empruntDuration = emprunt.DateRetour - emprunt.DateEmprunt;

                        // Check if the emprunt duration is within the allowed limit (2 weeks)
                        if (empruntDuration.TotalDays <= 14)
                        {
                            // Mark the book as borrowed
                            selectedBook.EstEmprunte = true;

                            // Add and save the emprunt
                            _context.Add(emprunt);
                            await _context.SaveChangesAsync();

                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("DateRetour", "The emprunt duration cannot exceed 2 weeks.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LivreId", "The selected book is not available for borrowing.");
                    }
                }
                else
                {
                    ModelState.AddModelError("AbonneId", "An abonné can borrow a maximum of 2 books at a time.");
                }
            }

            // If ModelState is not valid or the book is not available, reload the view with the available books
            var availableBooks = _context.Livres.Where(l => !l.EstEmprunte).ToList();
            ViewBag.AvailableBooks = new SelectList(availableBooks, "Id", "Titre");

            var availableAbonnes = _context.Abonnes.ToList();
            ViewBag.AvailableAbonnes = new SelectList(availableAbonnes, "Id","Id");

            return View(emprunt);
        }


        //---------------------------------- GET: Emprunts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Emprunts == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts.FindAsync(id);
            if (emprunt == null)
            {
                return NotFound();
            }
            return View(emprunt);
        }

        // POST: Emprunts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LivreId,AbonneId,DateEmprunt,DateRetour")] Emprunt emprunt)
        {
            if (id != emprunt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprunt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpruntExists(emprunt.Id))
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
            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Emprunts == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // POST: Emprunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Emprunts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Emprunts'  is null.");
            }
            var emprunt = await _context.Emprunts.FindAsync(id);
            if (emprunt != null)
            {
                _context.Emprunts.Remove(emprunt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpruntExists(int id)
        {
            return (_context.Emprunts?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        //---------------Return

        public IActionResult ReturnBook(int id)
        {
            // Récupérer l'emprunt en fonction de l'id
            var emprunt = _context.Emprunts.Include(e => e.Livre).FirstOrDefault(e => e.Id == id);

            if (emprunt == null)
            {
                return NotFound();
            }

            // Mettre à jour la date de retour
            emprunt.DateRetour = DateTime.Now;

            // Mettre à jour le statut du livre
            emprunt.Livre.EstEmprunte = false;

            // Sauvegarder les changements dans la base de données
            _context.SaveChanges();

            // Ajouter un message de réussite dans TempData
            TempData["SuccessMessage"] = "Livre retourné avec succès.";

            return RedirectToAction("Index"); // Rediriger vers la liste des emprunts après la restitution
        }


    }
}




