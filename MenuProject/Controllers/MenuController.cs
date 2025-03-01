using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MenuProject.Data;
using MenuProject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MenuProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuDbContext _context;

        public MenuController(MenuDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(string searchString)
        {
            var dishes = from item in _context.Dishes select item;

            if(!string.IsNullOrEmpty(searchString))
            {
                dishes = dishes.Where(x => x.Name.Contains(searchString));
            }

            return View(await dishes.ToListAsync());
        }

        // GET: /<controller>/id
        public async Task<IActionResult> Details (int? id)
        {
            var dish = await _context.Dishes
                .Include(di => di.DishIngredients)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(d => d.Id==id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: /<controller>/create
        public async Task<IActionResult> Create()
        {
            ViewData["Ingredients"] = await _context.Ingredients.ToListAsync();
            return View();
        }

        // POST: /<controller>/create
        [HttpPost]
        public IActionResult Create([Bind("Id,Name,ImageUrl,Price")] Dish dish, List<int> ingList)
        {
            if (dish.Name == null)
            {
                ModelState.AddModelError("Name", "Name cannot be null");
            }

            if (ingList.Count <= 0)
            {
                ModelState.AddModelError("Ingredients", "Ingredients should be greater than 0");
            }

            Dish newDish = new Dish();
            newDish.Name = dish.Name;
            newDish.ImageUrl = dish.ImageUrl;
            newDish.Price = dish.Price;

            if (ModelState.IsValid)
            {
                _context.Dishes.Add(newDish);
                _context.SaveChanges();
                TempData["success"] = "Dish created successfully";

                newDish.DishIngredients = new List<DishIngredient>();

                foreach (int id in ingList)
                {
                    DishIngredient dsh = new DishIngredient();
                    dsh.DishId = newDish.Id;
                    dsh.IngredientId = id;
                    newDish.DishIngredients.Add(dsh);

                    _context.DishIngredients.Add(dsh);
                    _context.SaveChanges();
                    TempData["success"] = "Dish Ingredients created successfully";
                }

                return RedirectToAction("Index");
            }

            return View(newDish);
        }

        // GET: /<controller>/edit
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _context.Dishes
                .Include(di => di.DishIngredients)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(d => d.Id == id);

            ViewData["Ingredients"] = await _context.Ingredients.ToListAsync();
            return View(dish);
        }
    }
}

