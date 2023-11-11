using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BusinessLogic
{
    public class TaskManagerService
    {
        private List<Task> _tasks = new List<Task>();

        public Task Add(string description, DateTime? dueDate)
        {
            var task = new Task(description, dueDate);
            _tasks.Add(task);
            return task;
        }

        public bool Remove(int taskId)
        {
            var taskToRemove = _tasks.Find(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                _tasks.Remove(taskToRemove);
                return true;
            }
            return false;
        }

        public Task Get(int taskId)
        {
            return _tasks.Find(t => t.Id == taskId);  // pobiera zadanie o podanym ID z listy
        }

        public Task[] GetAll()
        {
            return _tasks.ToArray();
        }

        public Task[] GetAll(TaskStatus status)
        {
            return _tasks.FindAll(t => t.Status == status).ToArray();
        }

        public Task[] GetAll(string description)
        {
            return _tasks.FindAll(t => t.Description.Contains(description, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }

        public bool ChangeStatus(int taskId, TaskStatus newStatus)
        {
            var taskToChange = Get(taskId);
            if (taskToChange == null || taskToChange?.Status == newStatus)
            {
                return false;
            }

            switch (newStatus)
            {
                case TaskStatus.ToDo:
                    return taskToChange.Open();
                case TaskStatus.InProgress:
                    return taskToChange.Start();
                case TaskStatus.Done:
                    return taskToChange.Done();
                default:
                    return false;
            }
        }

        //throw new NotImplementedException("Na razie nie obsługujemy tej funkcji. Do zaimplementowania później.");
    }
}
