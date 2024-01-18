using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public IActionResult Index()
        {
            var livres = _context.Livres.ToList();
            return View(livres);
        }

        public IActionResult About()
        {
           
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        //-----------------EnvoyerMessage----------------
        public IActionResult EnvoyerMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnvoyerMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        //------------------Blogs------------


        public async Task<IActionResult> AllBlogs()
        {
            return _context.Blogs != null ?
                        View(await _context.Blogs.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Blogs'  is null.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}