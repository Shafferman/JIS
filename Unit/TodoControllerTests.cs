using System.Collections.Generic;
using JISTesting.Controllers;
using JISTesting.Core.Interfaces;
using JISTesting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TodoControllerTests
{
    public class ControllerTests
    {
        private readonly Mock<ITodoRepository> repoMock;
        private readonly Mock<IUserManager> userManagerMock;

        private readonly TodoController sut;

        public ControllerTests()
        {
            repoMock = new Mock<ITodoRepository>();
            userManagerMock = new Mock<IUserManager>();

            sut = new TodoController(repoMock.Object, userManagerMock.Object);
        }

        [Fact]
        public void WhenGetAllIsCalledThenAllTodoItemsInRepoAreReturned()
        {
            repoMock.Setup(x => x.GetAll()).Returns(new List<TodoItem>() 
            {
                new TodoItem { Id = 1, Name = "Something"}
            });

            var actual = sut.GetAll();

            Assert.Equal(1, actual.Value.Count);
        }

        [Fact]
        public void GivenIdWhenIdIsFoundThenItemIsReturned()
        {
            long id = 5;
            var item = new TodoItem { Id = 1, Name = "Something" };

            repoMock.Setup(x => x.GetById(id)).Returns(item);

            var actual = sut.GetById(id);

            Assert.Equal(1, actual.Value.Id);
            Assert.Equal("Something", actual.Value.Name);
        }

        [Fact]
        public void GivenIdWhenIdIsNotFoundThenNotFoundResultIsReturned()
        {
            long id = 5;

            var actual = sut.GetById(id);

            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public void GivenItemWhenItemIsCreatedThenCreatedOnRouteResultIsReturned()
        {
            var item = new TodoItem { Id = 1, Name = "Something" };

            var result = sut.Create(item);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public void GivenIdWhenIdIsNotFoundThenNotFoundResultIsRetured()
        {
            long id = 5;

            var actual = sut.Update(id, null);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void GivenIdWhenIdIsFoundThenItemIsUpdated()
        {
            long id = 5;
            var item = new TodoItem { IsComplete = false, Name = "Something" };
            repoMock.Setup(x => x.GetById(id)).Returns(item);
            repoMock.Setup(x => x.Update(item)).Verifiable();

            var itemUpdate = new TodoItem { IsComplete = true, Name = "New" };

            var actual = sut.Update(id, itemUpdate);

            repoMock.Verify();
        }

        [Fact]
        public void GivenIdWhenIdIsFoundThenNoContentIsReturned()
        {
            long id = 5;
            var item = new TodoItem { IsComplete = false, Name = "Something" };
            repoMock.Setup(x => x.GetById(id)).Returns(item);

            var itemUpdate = new TodoItem { IsComplete = true, Name = "New" };

            var actual = sut.Update(id, itemUpdate);

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void GivenIdIsNotFoundWhenRemovingItemThenNotFoundResultIsReturned()
        {
            long id = 5;

            var actual = sut.Delete(id);

            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public void GivenIdIsFoundWhenRemovingItemThenRepoRemovesItem()
        {
            long id = 5;
            var item = new TodoItem { Id = 1, Name = "Something" };
            repoMock.Setup(x => x.GetById(id)).Returns(item);
            repoMock.Setup(x => x.Remove(item)).Verifiable();

            var actual = sut.Delete(id);

            repoMock.Verify();
        }

        [Fact]
        public void GivenIdIsFoundWhenRemovingItemThenNoContentResultIsReturned()
        {
            long id = 5;
            var item = new TodoItem { Id = 1, Name = "Something" };
            repoMock.Setup(x => x.GetById(id)).Returns(item);

            var actual = sut.Delete(id);

            Assert.IsType<NoContentResult>(actual);
        }
    }
}
