using System;
using MenuProject.Data;
using MenuProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuProject.Controllers
{
    public class IngredientController : Controller
    {
        private readonly MenuDbContext _context;

        public IngredientController(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ingredients = _context.Ingredients;

            return View(await ingredients.ToListAsync());
        }

        // GET: /<controller>/create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /<controller>/create
        [HttpPost]
        public IActionResult Create([Bind("Id,Name")] Ingredient ing)
        {
            if (ing.Name == null)
            {
                ModelState.AddModelError("Name", "Ingredient name cannot be null");
            }
            else
            {
                Ingredient newIngredient = new Ingredient();
                newIngredient.Name = ing.Name;

                _context.Ingredients.Add(newIngredient);
                _context.SaveChanges();
                ViewData["success"] = "Ingredient created successfully";

                return RedirectToAction("Index");
            }

            return View(ing);
        }
    }
}

