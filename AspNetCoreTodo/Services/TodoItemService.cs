using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TodoItemService(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AddItemAsync(TodoItemModel todoItemModel, ApplicationUser user)
        {
            todoItemModel.Id = Guid.NewGuid();
            todoItemModel.IsDone = false;
            todoItemModel.DueAt = DateTime.UtcNow.AddDays(3);
            todoItemModel.UserId = user.Id;

            _applicationDbContext.TodoItems.Add(todoItemModel);

            var saveResult = await _applicationDbContext.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<TodoItemModel[]> GetIncompleteItemsAsync(ApplicationUser user)
        {
            // Hard-coded values
            //var todoItems = new List<TodoItemModel>
            //{
            //    new TodoItemModel
            //    {
            //        Title = "Learn ASP.NET Core",
            //        DueAt = DateTimeOffset.Now.AddDays(1)
            //    },
            //    new TodoItemModel
            //    {
            //        Title = "Build awesome apps",
            //        DueAt = DateTimeOffset.Now.AddDays(2)
            //    }
            //};

            //return Task.FromResult(todoItems?.ToArray());

            return await _applicationDbContext.TodoItems
                .Where(x => x.IsDone == false && x.UserId == user.Id)
                .ToArrayAsync();
        }

        public async Task<bool> MarkDoneAsync(Guid id, ApplicationUser user)
        {
            var item = await _applicationDbContext.TodoItems
                                                  .Where(x => x.Id == id && x.UserId == user.Id)
                                                  .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;
            var saveResult = await _applicationDbContext.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }
    }
}
