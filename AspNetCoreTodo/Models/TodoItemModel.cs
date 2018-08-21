using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Models
{
    public class TodoItemModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public string UserId { get; set; }
    }
}
