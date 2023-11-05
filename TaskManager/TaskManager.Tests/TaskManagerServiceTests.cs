using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic;
using Task = TaskManager.BusinessLogic.Task;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager.Tests
{
    public class TaskManagerServiceTests
    {
        [Fact]
        public void Should_AddTask_ToTaskList() //Weryfikuje, czy nowe zadanie może być dodane do listy zadań i czy jest poprawnie zwracane po dodaniu.
        {
            //Arrange
            var service = new TaskManagerService();

            //Act
            var task = service.Add("Test", DateTime.Now.AddDays(10));

            //Assert
            Assert.NotNull(task);
            Assert.Single(service.GetAll());
        }

        [Fact]
        public void Should_RemoveTask_ByTaskId() //Testuje, czy zadanie można usunąć z listy za pomocą jego identyfikatora.
        {
            //Arrange
            var service = new TaskManagerService();
            var task = service.Add("Test", DateTime.Now.AddDays(10));

            //Act
            bool result = service.Remove(task.Id);

            //Assert
            Assert.True(result);
            Assert.Empty(service.GetAll());
        }

        [Fact]
        public void Should_NotRemoveTask_WhenTaskIdDoesNotExist() //Sprawdza, czy próba usunięcia zadania o nieistniejącym ID kończy się niepowodzeniem.
        {
            //Arrange
            var service = new TaskManagerService();
            var task = service.Add("Test", DateTime.Now.AddDays(10));

            //Act
            bool result = service.Remove(345);

            //Assert
            Assert.False(result);
            Assert.Single(service.GetAll());
        }

        [Fact]
        public void Should_GetTask_ByTaskId() //Weryfikuje, czy możliwe jest pobranie zadania z listy na podstawie jego ID.
        {
            //Arrange
            var service = new TaskManagerService();
            var task = service.Add("Test", DateTime.Now.AddDays(10));

            //Act
            var result = service.Get(task.Id);
            var result2 = service.Get(6);


            //Assert
            Assert.NotNull(result);
            Assert.Null(result2);
            Assert.Equal(task.Id, result.Id);
        }

        [Fact]
        public void Should_GetAllTasks_WithNoFilter() //Testuje funkcję, która zwraca wszystkie zadania z listy.
        {
            //Arrange
            var service = new TaskManagerService();
            var task1 = service.Add("Test 1", null);
            var task2 = service.Add("Test 2", null);

            //Act
            var tasks = service.GetAll();

            //Assert
            Assert.Equal(2, tasks.Length);
        }

        [Fact]
        public void Should_GetTasks_ByStatus() //Upewnia się, że zadania mogą być filtrowane i zwracane na podstawie ich statusu.
        {
            //Arrange
            var service = new TaskManagerService();
            var task1 = service.Add("Test 1", null);
            var task2 = service.Add("Test 2", null);

            //Act
            var tasks = service.GetAll(TaskStatus.ToDo);

            //Assert
            Assert.Equal(2, tasks.Length);

            //var service = new TaskManagerService();
            // var task1 = service.Add("Test task 1", null);
            // task1.Start();
            // service.Add("Test task 2", null);
            // 
            // var inProgressTasks = service.GetAll(TaskStatus.InProgress);
            // 
            // Assert.Single(inProgressTasks);
            // Assert.Equal(task1.Id, inProgressTasks.First().Id);
        }

        [Fact]
        public void Should_GetTasks_ByDescription() //Weryfikuje, czy zadania można filtrować na podstawie słów kluczowych w ich opisie.
        {
            //Arrange
            var service = new TaskManagerService();
            service.Add("Specific test", null);
            service.Add("Typical test", null);

            //Act
            var tasks = service.GetAll("Specific");

            //Assert
            Assert.Single(tasks);
            Assert.Equal("Specific test", tasks.First().Description);
        }

        [Fact]
        public void Should_ChangeTaskStatus_WhenValid() //Sprawdza, czy status zadania można zmienić, pod warunkiem, że jest to dozwolone.
        {
            //Arrange
            var service = new TaskManagerService();
            var task = service.Add("Test", null);

            //Act
            bool statusChangeToToDo = service.ChangeStatus(task.Id, TaskStatus.ToDo);
            bool statusChangeToInProgress = service.ChangeStatus(task.Id, TaskStatus.InProgress);

            //Assert
            Assert.False(statusChangeToToDo);
            Assert.True(statusChangeToInProgress);
            Assert.Equal(TaskStatus.InProgress, task.Status);
            }

        [Fact]
        public void Should_NotChangeTaskStatus_WhenInvalidTransition() //Testuje, czy próba nieprawidłowej zmiany statusu (np. bezpośrednio z ToDo na Done) kończy się niepowodzeniem.
        {
            //Arrange
            var service = new TaskManagerService();
            var task = service.Add("Test", null);

            //Act
            bool statusChangeToDone = service.ChangeStatus(task.Id, TaskStatus.Done);

            //Assert
            Assert.False(statusChangeToDone);
            Assert.Equal(TaskStatus.ToDo, task.Status);
        }

        [Fact]
        public void Should_NotChangeTaskStatus_WhenTaskIdDoesNotExist() //Weryfikuje, czy próba zmiany statusu dla nieistniejącego zadania kończy się niepowodzeniem.
        {
            //Arrange
            var service = new TaskManagerService();
            var task = service.Add("Test", null);

            //Act
            bool status = service.ChangeStatus(300, TaskStatus.InProgress);

            //Assert
            Assert.False(status);
        }
    }
}
