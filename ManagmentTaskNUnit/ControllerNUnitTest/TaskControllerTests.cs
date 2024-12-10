using ManagementAPI.Controllers;
using ManagementAPI.Interfaces;
using ManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentTaskNUnit.ControllerNUnitTest
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<ITaskService> _mockTaskService;
        private TaskController _controller;
        private Mock<ILogger<TaskController>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockLogger = new Mock<ILogger<TaskController>>();
            _controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetTasks_ShouldReturnOk_WhenTasksExist()
        {
            // Arrange
            var tasks = new List<TaskEntity> { new TaskEntity { Id = 1, Title = "Test Task" } };
            _mockTaskService.Setup(x => x.GetTasksAsync(It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasks(null, null);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(tasks, okResult.Value);
        }

        [Test]
        public async Task GetTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskService.Setup(x => x.GetTaskByIdAsync(1)).ReturnsAsync((TaskEntity)null);

            // Act
            var result = await _controller.GetTask(1);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public async Task CreateTask_ShouldReturnCreatedAtAction_WhenTaskIsValid()
        {
            // Arrange
            var task = new TaskEntity { Id = 1, Title = "New Task" };
            _mockTaskService.Setup(x => x.CreateTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(task);

            // Act
            var result = await _controller.CreateTask(task);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);
            Assert.AreEqual("GetTask", createdAtActionResult.ActionName);
        }

        [Test]
        public async Task CreateTask_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var task = new TaskEntity();

            // Act
            var result = await _controller.CreateTask(task);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task UpdateTask_ShouldReturnOk_WhenTaskIsUpdatedSuccessfully()
        {
            // Arrange
            var task = new TaskEntity { Id = 1, Title = "Updated Task" };
            _mockTaskService.Setup(x => x.UpdateTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateTask(1, task);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task UpdateTask_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var task = new TaskEntity { Id = 2, Title = "Updated Task" };

            // Act
            var result = await _controller.UpdateTask(1, task);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task DeleteTask_ShouldReturnOk_WhenTaskIsDeletedSuccessfully()
        {
            // Arrange
            _mockTaskService.Setup(x => x.DeleteTaskAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task DeleteTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskService.Setup(x => x.DeleteTaskAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}