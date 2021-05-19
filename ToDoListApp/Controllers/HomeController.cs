using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext context;
        public HomeController(ToDoContext ctx) => context = ctx;
        /*
         * public HomeController(ToDoContext ctx)
        {
            context = ctx;
        }
         */


        
        public IActionResult Index(string id)
        {
            var model = new ToDoViewModel(); //CREATE AN INSTANCE OF THE VIEWMODEL CLASS
            model.Filters = new Filters(id); //FILTER PROPERTY -- PARAMETER FOR FILTERS
            model.Categories = context.Categories.ToList(); //LIST OF CATEGORIES PROPERTY
            model.Statuses = context.Statuses.ToList(); // LIST OF STATUSES PROPERTY
            model.DueFilters = Filters.DueFilterValues; // DICTIONARY PROPERTY


            IQueryable<ToDo> query = context.ToDos.Include(c => c.Category).Include(s => s.Status); //GET ALL THE TASKS WITH CATEGORIES AND STATUSES

            if (model.Filters.HasCategory) //IF THE USER SETS A CATEGORY FILTER
            {
                query = query.Where(t => t.CategoryId == model.Filters.CategoryId); //DISPLAYS ONLY THE TASKS WHERE THEIR CATEGORY ID(STRING) IS EQUAL TO THE CATEGORY OF THE URL--ARRAY[0]/
            }
            if (model.Filters.HasStatus) // IF THE USER SETS A STATUS FILTER
            {
                query = query.Where(t => t.StatusId == model.Filters.StatusId); // DISPLAYS ONLY THE TASKS WHERE THEIR STATUS ID(STRING) IS EQUAL TO THE STATUS OF THE URL--ARRAY[2]/
            }
            if (model.Filters.HasDue)// IF THE USER SETS A DUEDATE FILTER
            {
                var today = DateTime.Today; //STORE IN VARIABLE THE TODAY'S DATE
                if (model.Filters.IsPast) //IF THE USER SEARCHES FOR VALUES OF THE PAST
                {
                    query = query.Where(t => t.DueDate < today); // DISPLAYS THE TASKS WHERE THE DATE IS LESS THAN TODAY'S DATE
                }
                else if (model.Filters.IsFuture) //IF THE USER SEARCHES FOR VALUES OF THE FUTURE
                {
                    query = query.Where(t => t.DueDate > today); // DISPLAYS THE TASKS WHERE THE DATE IS GREATER THAN TODAY'S DATE
                }
                else if(model.Filters.IsToday) //IF THE USER SEARCHES FOR VALUES OF TODAY'S DATE
                {
                    query = query.Where(t => t.DueDate == today); //DISPLAYS THE TASKS WHERE THE DATE IS TODAY'S DATE
                }
            }
            var tasks = query.OrderBy(t => t.DueDate).ToList(); // STORE IN VARIABLE ALL THE TASKS ORDERED BY DUEDATE

            model.Tasks = tasks; //ASSIGN TASKS VARIABLE TO THE MODEL SO IT CAN BE PASSED TO THE VIEW
            return View(model); //RETURN THE VIEW PASSING THE MODEL
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new ToDoViewModel();
            model.Categories = context.Categories.ToList();
            model.Statuses = context.Statuses.ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ToDoViewModel model)
        {
            if (ModelState.IsValid)
            {
                context.ToDos.Add(model.CurrentTask);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                model.Categories = context.Categories.ToList();
                model.Statuses = context.Statuses.ToList();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult EditDelete([FromRoute] string id, ToDo selected)
        {
            if(selected.StatusId == null)
            {
                context.ToDos.Remove(selected);
            }
            else
            {
                string newStatusId = selected.StatusId;
                selected = context.ToDos.Find(selected.ToDoId);
                selected.StatusId = newStatusId;
                context.ToDos.Update(selected);
            }
            context.SaveChanges();

            return RedirectToAction("Index", "Home", new { ID = id });
        }

        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);

            return RedirectToAction("Index", "Home", new { ID = id }); ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
