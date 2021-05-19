using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApp.Models
{
    public class ToDo
    {
        public int ToDoId { get; set; }

        [Required(ErrorMessage ="Please enter a description")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Please enter a due date")]
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage ="Please select a category")]
        public string  CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please select a status")]
        public string StatusId { get; set; }
        public Status Status { get; set; }

        public bool OverDue => StatusId == "open" && DueDate < DateTime.Today;  //CONDITION!! IF THE STATUS IS OPEN AND THE DUE DATE IS BEFORE THAN TODAYS DATE.. THE TASK IS OVERDUE
    }
}
