using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = TaskManager.BusinessLogic.Task;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager.Tests
{
    public class TaskTests
    {
        [Fact]
        public void
            Should_CreateTask_WithAutoIncrementedId() //Testuje, czy każde nowo utworzone zadanie otrzymuje unikalny, autoinkrementowany identyfikator.
        {
            // Arrange
            var task1 = new Task("Test 1", null);
            var task2 = new Task("Test 2", null);

            // Assert
            Assert.True(task1.Id > 0);
            Assert.Equal(1, task1.Id);
            Assert.Equal(task1.Id + 1, task2.Id);
        }

        [Fact]
        public void
            Should_SetCreationDate_WhenCreatingTask() //Weryfikuje, czy przy tworzeniu zadania poprawnie ustawiana jest data jego utworzenia.
        {
            //Arrange
            var task = new Task("Test", null);
            var difference = DateTime.Now - task.CreationDate;

            //Assert
            Assert.True(difference.TotalSeconds < 1);
            // Assert.Equal(DateTime.Now, task.CreationDate); // powoduje błąd na części sekundowej - różnice w dziesiątych miejscach po przecinku
        }

        [Fact]
        public void
            Should_SetDueDate_WhenProvided() //Sprawdza, czy możliwe jest ustawienie daty zakończenia zadania podczas jego tworzenia.
        {
            //Arrange
            //var task1 = new Task("Test 1", DateTime.Today);
            var taskDate = new DateTime(2023, 11, 25);
            //var taskDate = DateTime.Now.AddDays(14) // z użyciem metody dodawania podanej liczby dni, tutaj 14
            var task = new Task("Test 2", taskDate);

            //Assert
            Assert.True(task.DueDate.HasValue);
            Assert.Equal(taskDate, task.DueDate);
        }

        [Fact]
        public void
            Should_SetStatusToTodo_WhenTaskIsCreated() //Upewnia się, że nowo utworzone zadanie domyślnie ma status ToDo.
        {
            //Arrange
            var task = new Task("Test", null);

            //Assert
            Assert.Equal(TaskStatus.ToDo, task.Status);
        }

        [Fact]
        public void
            Should_ChangeStatus_ToInProgress_WhenStartIsCalled() //Testuje funkcję rozpoczęcia zadania i weryfikuje, czy status zadania zmienia się odpowiednio.
        {
            //Arrange
            var task = new Task("Test", null);

            //Act
            bool testResult = task.Start();

            //Assert
            Assert.True(testResult);
            Assert.Equal(TaskStatus.InProgress, task.Status);
        }

        [Fact]
        public void
            Should_SetStartDate_WhenStartIsCalled() //Sprawdza, czy po rozpoczęciu zadania ustawiana jest odpowiednia data rozpoczęcia.
        {
            //Arrange
            var task = new Task("Test", null);

            //Act
            task.Start();

            //Assert
            Assert.NotNull(task.StartDate);
            var difference = DateTime.Now - task.StartDate.Value;
            Assert.True(difference.TotalSeconds < 1);
        }

        [Fact]
        public void
            Should_NotChangeStatus_ToInProgress_IfAlreadyInProgress() //Upewnia się, że zadanie, które jest już w trakcie realizacji, nie może być ponownie rozpoczęte.
        {
            //Arrange
            var task = new Task("Test", null);

            //Act
            task.Start();
            bool testResult = task.Start();

            //Assert
            Assert.False(testResult);
            Assert.Equal(TaskStatus.InProgress, task.Status);
        }

        [Fact]
        public void
            Should_ChangeStatus_ToDone_WhenDoneIsCalledAndStatusIsInProgress() //Weryfikuje, czy zadanie w trakcie realizacji można oznaczyć jako zakończone i czy status zadania zmienia się odpowiednio.
        {
            //Arrange
            var task = new Task("Test", null);
            task.Start();

            //Act
            bool testResult = task.Done();

            //Assert
            Assert.True(testResult);
            Assert.Equal(TaskStatus.Done, task.Status);
        }

        [Fact]
        public void
            Should_SetDoneDate_WhenDoneIsCalled() //Sprawdza, czy po zakończeniu zadania ustawiana jest odpowiednia data zakończenia.
        {
            //Arrange
            var task = new Task("Test", null);

            //Act
            task.Start();
            task.Done();

            //Assert
            //Assert.True(task.DoneDate.HasValue);
            Assert.NotNull(task.DoneDate);
            var difference = DateTime.Now - task.DoneDate.Value;
            Assert.True(difference.TotalSeconds < 1);
        }

        [Fact]
        public void
            Should_NotChangeStatus_ToDone_IfStatusIsNotInProgress() //Upewnia się, że zadanie, które nie zostało rozpoczęte, nie można oznaczyć jako zakończone.
        {
            //Arrange
            var task = new Task("Test", null);

            //Act
            bool testResult = task.Done();

            //Assert
            Assert.Null(task.DoneDate);
            Assert.False(testResult);
            Assert.Equal(TaskStatus.ToDo, task.Status);
        }

        [Fact]
        public void Should_CalculateDuration_WhenStatusIsInProgress() //Testuje obliczanie czasu trwania zadania, które jest w trakcie realizacji.
        {
            //Arrange
            var task = new Task("Test", null);
            
            //Act
            task.Start();
            var duration = task.Duration;

            //Assert
            Assert.NotNull(duration);
        }

        [Fact]
        public void Should_ReturnNullDuration_WhenStatusIsTodo() //Weryfikuje, że zadanie z statusem ToDo nie ma określonego czasu trwania.
        {
            //Arrange
            var task = new Task("Test", null);

            //Act
            var duration = task.Duration;

            //Assert
            Assert.Equal(TaskStatus.ToDo, task.Status);
            Assert.Null(duration);
        }
    }
}
