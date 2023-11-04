using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class Task
    {
        private static int _id;
        public int Id { get; }
        public string Description { get; set; }
        public DateTime CreationDate { get; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? DoneDate { get; private set; }
        public TaskStatus Status { get; private set; } = TaskStatus.ToDo;
        public TimeSpan? Duration => StartDate != null ? (DoneDate ?? DateTime.Now) - StartDate.Value : null;
        //public TimeSpan Duration => StartDate != null ? (DoneDate ?? DateTime.Now) - StartDate.Value : TimeSpan.Zero;
        // null propagation - sprawdzić

        public Task(string description, DateTime? dueDate)
        {
            Id = ++_id;
            Description = description;
            CreationDate = DateTime.Now;
            DueDate = dueDate;
        }

        public bool Start()
        {
            if (Status == TaskStatus.ToDo)
            {
                Status = TaskStatus.InProgress;
                StartDate = DateTime.Now;
                DoneDate = null;
                return true;
            }
            return false;
        }

        public bool Open()
        {
            if (Status != TaskStatus.ToDo)
            {
                Status = TaskStatus.ToDo;
                StartDate = null;
                DoneDate = null;
                return true;
            }
            return false;
        }

        public bool Done()
        {
            if (Status == TaskStatus.InProgress)
            {
                Status = TaskStatus.Done;
                DoneDate = DateTime.Now;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Id} - {Description} ({Status})";
        }
    }
}
