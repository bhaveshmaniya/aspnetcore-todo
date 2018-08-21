using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers
{
    // User: 
    // test.test1@gmail.com/Test@123
    [Authorize]
    public class TodoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITodoItemService _todoItemService;

        public TodoController(UserManager<ApplicationUser> userManager,
            ITodoItemService todoItemService)
        {
            this._userManager = userManager;
            this._todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var todoItems = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            var model = new TodoItemVideModel
            {
                TodoItems = todoItems
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItemModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var success = await _todoItemService.AddItemAsync(newItem, currentUser);
            if (!success)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var success = await _todoItemService.MarkDoneAsync(id, currentUser);
            if (!success)
            {
                return BadRequest("Could not mark item as done.");
            }

            return RedirectToAction("Index");
        }
    }
}