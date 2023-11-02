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

        public Task Add(string description, DateTime? dueDate = null)
        {
            var newTask = new Task(description, dueDate);
            _tasks.Add(newTask);
            return newTask;
        }

        public bool Remove(int taskId)
        {
            var task = Get(taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            return false;
        }

        public Task Get(int taskId)
        {
            return _tasks.Find(t => t.Id == taskId);  // pobiera zadanie o podanym ID z listy
        }
    }
}
