using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using library.Models;
using System.Collections.Generic;
using System.Linq;

namespace library.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BookEm(int subscriberId)
        {
            var emprunts = await _context.Emprunts
                .Where(r => r.AbonneId == subscriberId)
                .Include(r => r.Livre)
                .OrderBy(r => r.DateEmprunt)
                .ToListAsync();

            var abonne = await _context.Abonnes
                .FirstOrDefaultAsync(a => a.Id == subscriberId);

            if (abonne == null)
            {
                return NotFound();
            }

            ViewData["SubscriberName"] = $"{abonne.Nom} {abonne.Prenom}";

            return View(emprunts);
        }


        public IActionResult MesaageList()
        {
            var messages = _context.Messages.ToList();
            return View(messages);
        }
        //-----------------Delete----------------
        public IActionResult Delete(int id)
        {
            var message = _context.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();

            return RedirectToAction("MesaageList");
        }


    }
}
