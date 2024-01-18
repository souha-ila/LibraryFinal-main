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
    public class AbonneController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int Id;

        public AbonneController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Abonne
        public async Task<IActionResult> Index()
        {
              return _context.Abonnes != null ? 
                          View(await _context.Abonnes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Abonnes'  is null.");
        }

        // GET: Abonne/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Abonnes == null)
            {
                return NotFound();
            }

            var abonne = await _context.Abonnes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abonne == null)
            {
                return NotFound();
            }

            return View(abonne);
        }

        // GET: Abonne/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Abonne/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom")] Abonne abonne)
        {
            if (ModelState.IsValid)
            {
                _context.Add(abonne);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(abonne);
        }

        // GET: Abonne/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonne = await _context.Abonnes.FindAsync(id);
            if (abonne == null)
            {
                return NotFound();
            }

            return View(abonne);
        }

        // POST: Abonne/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom")] Abonne abonne)
        {
            if (id != abonne.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the entry in the database
                    _context.Update(abonne);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbonneExists(abonne.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirect to the Index action after a successful update
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return to the edit view with the provided model
            return View(abonne);
        }

        // GET: Abonne/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Abonnes == null)
            {
                return NotFound();
            }

            var abonne = await _context.Abonnes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abonne == null)
            {
                return NotFound();
            }

            return View(abonne);
        }

        // POST: Abonne/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Abonnes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Abonnes'  is null.");
            }
            var abonne = await _context.Abonnes.FindAsync(id);
            if (abonne != null)
            {
                _context.Abonnes.Remove(abonne);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbonneExists(int id)
        {
          return (_context.Abonnes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
