using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;

namespace TaskManager
{
    public class Program
    {
        private static TaskManagerService _taskManagerService = new TaskManagerService();
        static void Main(string[] args)
        {
            int command;
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

                if(int.TryParse(Console.ReadLine(), out command));  //brakuje else w sytuacji gdy parsowanie się nie powiedzie i użytkowmik poda np. literę, może jednak zamiast int dać string command
                {
                    switch (command)
                    {
                        case 1:
                            AddTask();
                            break;
                        case 2:
                            RemoveTask();
                            break;
                        case 3:
                            ShowTaskDetails();
                            break;
                        case 4:
                            DisplayAllTasks();
                            break;
                        case 5:
                            DisplayTasksByStatus();
                            break;
                        case 6:
                            SearchTask();
                            break;
                        case 7:
                            ChangeTaskStatus();
                            break;
                        /*default:
                            Console.WriteLine("Nieprawidłowy wybór. Wprowadź ponownie.");
                            break;*/
                    }
                }
                Console.WriteLine();
            } while (command != 8);
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Pomyślnie dodano zadanie {task}.");
            Console.ResetColor();
            //Console.WriteLine(task.CreationDate);
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
                CorrectlyTask($"Pomyślnie usunięto zadanie {taskId}");
            }
            else
            {
                IncorrectlyTask($"Nie udało się usunąć zadania, ponieważ zadanie {taskId} nie istnieje.");
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
                IncorrectlyTask($"Nie da się wyświetlić szczegółów zadania {taskId}, ponieważ nie istnieje.");
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
            throw new NotImplementedException("Na razie nie obsługujemy tej funkcji. Do zaimplementowania później.");
        }
        private static void DisplayTasksByStatus()
        {
            throw new NotImplementedException("Na razie nie obsługujemy tej funkcji. Do zaimplementowania później.");
        }
        private static void SearchTask()
        {
            throw new NotImplementedException("Na razie nie obsługujemy tej funkcji. Do zaimplementowania później.");
        }
        private static void ChangeTaskStatus()
        {
            throw new NotImplementedException("Na razie nie obsługujemy tej funkcji. Do zaimplementowania później.");
        }

        private static void CorrectlyTask(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private static void IncorrectlyTask(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}