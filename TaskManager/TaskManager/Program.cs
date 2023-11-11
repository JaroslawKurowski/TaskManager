using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager
{
    public class Program
    {
        private static TaskManagerService _taskManagerService = new TaskManagerService();
        static void Main(string[] args)
        {
            string command;
            do
            {
                Console.WriteLine("Wybierz jedną z następujących opcji: ");
                Console.WriteLine("1. Dodaj zadanie");
                Console.WriteLine("2. Usuń zadanie");
                Console.WriteLine("3. Pokaż szczegóły zadania");
                Console.WriteLine("4. Wyświetl wszystkie zadania");
                Console.WriteLine("5. Wyświetl zadania według statusu");
                Console.WriteLine("6. Szukaj zadania");
                Console.WriteLine("7. Zmień status zadania");
                Console.WriteLine("8. Zakończ");
                Console.WriteLine("----------");

                command = Console.ReadLine().Trim();

                switch (command)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        RemoveTask();
                        break;
                    case "3":
                        ShowTaskDetails();
                        break;
                    case "4":
                        DisplayAllTasks();
                        break;
                    case "5":
                        DisplayTasksByStatus();
                        break;
                    case "6":
                        SearchTask();
                        break;
                    case "7":
                        ChangeTaskStatus();
                        break;
                }
                Console.WriteLine("");
            }
            while (command != "8");
        }

        private static void AddTask()
        {
            Console.WriteLine("Podaj opis zadania.");
            string description = Console.ReadLine();

            Console.WriteLine("Podaj datę wykonania zadania (lub pozostaw pustą).");
            DateTime? dueDate = null;
            DateTime date;
            if (DateTime.TryParse(Console.ReadLine(), out date))
            {
                dueDate = date;
            }

            var task = _taskManagerService.Add(description, dueDate);
            DoneCorrectly($"Pomyślnie dodano zadanie {task}.");
        }
        private static void RemoveTask()
        {
            Console.WriteLine("Podaj identyfikator zadania do usunięcia.");
            int taskId;
            while (!int.TryParse(Console.ReadLine(), out taskId))
            {
                Console.WriteLine("Podaj identyfikator zadania do usunięcia.");
            }

            if (_taskManagerService.Remove(taskId))
            {
                DoneCorrectly($"Pomyślnie usunięto zadanie {taskId}");
            }
            else
            {
                NotDoneCorrectly($"Nie udało się usunąć zadania, ponieważ zadanie {taskId} nie istnieje.");
            }
        }

        private static void ShowTaskDetails()
        {
            Console.WriteLine("Podaj identyfikator zadania, którego szczegóły chcesz zobaczyć.");
            int taskId;
            while (!int.TryParse(Console.ReadLine(), out taskId))
            {
                Console.WriteLine("Podaj identyfikator zadania, którego szczegóły chcesz zobaczyć.");
            }

            var task = _taskManagerService.Get(taskId);

            if (task == null)
            {
                NotDoneCorrectly($"Nie da się wyświetlić szczegółów zadania {taskId}, ponieważ nie istnieje.");
            }

            var sb = new StringBuilder("Szczegóły wybranego zadania:\n");
            sb.AppendLine(task.ToString());
            sb.AppendLine($"  Data utworzenia: {task.CreationDate}");

            if (task.StartDate != null)
            {
                sb.AppendLine($"  Data rozpoczęcia: {task.StartDate}");
            }

            if (task.DueDate != null)
            {
                sb.AppendLine($"  Data do kiedy zadanie powinno zostać zakończone: {task.DueDate}");
            }

            if (task.DoneDate != null)
            {
                sb.AppendLine($"  Data zakończenia: {task.DoneDate}");
            }

            if (task.Duration != null)
            {
                sb.AppendLine($"  Data trwania zadania: {task.Duration}");
            }
            
            Console.WriteLine(sb);

        }
        private static void DisplayAllTasks()
        {
            var tasks = _taskManagerService.GetAll();

            if (tasks.Length == 0)
            {
                NotDoneCorrectly($"Lista zadań jest pusta.");
            }

            Console.WriteLine($"Liczba zadań na liście wynosi: {tasks.Length}");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }
        private static void DisplayTasksByStatus()
        {
            var statuses = string.Join(", ", Enum.GetNames<BusinessLogic.TaskStatus>());
            Console.WriteLine($"Podaj jeden z dostępnych statusów: {statuses}");
            TaskStatus status;
            while (!Enum.TryParse<TaskStatus>(Console.ReadLine(), true, out status))
            {
                NotDoneCorrectly($"Podano niewłaściwy status zadania.");
                Console.WriteLine($"Podaj jeden z dostępnych statusów {statuses}");
            }

            var tasks = _taskManagerService.GetAll(status);
            Console.WriteLine($"Liczba zadań o statusie {status} wynosi: {tasks.Length}");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }
        private static void SearchTask()
        {
            Console.WriteLine("Podaj zadanie do wyszukania.");
            string text;
            while (true)
            {
                text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text))
                {
                    NotDoneCorrectly("Podaj fragment opisu zadania do wyszukania.");
                    continue;
                }
                break;
            }

            var tasks = _taskManagerService.GetAll(text);
                Console.WriteLine($"Liczba zadań zawierających podany fragment opisu: {tasks.Length}.");
                foreach (var task in tasks)
                {
                    Console.Write(task);
                }
        }
        private static void ChangeTaskStatus()
        {
            DisplayAllTasks();
            Console.WriteLine("Podaj identyfikator zadania, które chcesz zmienić.");
            int taskId;
            while (!int.TryParse(Console.ReadLine(), out taskId))
            {
                Console.WriteLine("Podaj identyfikator zadania, które chcesz zmienić.");
            }

            var statuses = string.Join(", ", Enum.GetNames<TaskStatus>());
            Console.WriteLine($"Podaj jeden z dostępnych statusów: {statuses}");
            TaskStatus status;
            while (!Enum.TryParse<TaskStatus>(Console.ReadLine(), true, out status))
            {
                NotDoneCorrectly($"Podano niewłaściwy status zadania.");
                Console.WriteLine($"Podaj jeden z dostępnych statusów {statuses}");
            }

            if (_taskManagerService.ChangeStatus(taskId, status))
            {
                DoneCorrectly($"Nowy status zadania o identyfikatorze {taskId} to {status}");
            }
            else
            {
                NotDoneCorrectly($"Nie można zmienić statusu zadania o identyfikatorze {taskId}.");
            }
        }

        private static void DoneCorrectly(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private static void NotDoneCorrectly(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}