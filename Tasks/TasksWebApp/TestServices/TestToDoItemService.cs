using System;
using System.Linq;
using System.Threading.Tasks;
using TasksAppData;
using TasksServices.Services;
using TestServices.Fixtures;
using Xunit;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace TestServices
{
    /// <summary>
    /// Basic Tests - incomplete and of course could be better
    /// using TodoItemFileRepository.cs so db written to Database.json in debug folder
    /// Really should use an InMemoryDatabase
    /// </summary>
    /// 
    [CollectionDefinition("Dependency Injection")]
    public class TestTodoItemService : TestBed<TestFixture>
    {
        public TestTodoItemService(ITestOutputHelper testOutputHelper, TestFixture fixture)
       : base(testOutputHelper, fixture)
        {
        }

        [Fact]
        public void Test_InitService()
        {
            // Arrange

            // Act 
            ITodoItemService? service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            

            // Assert
            Assert.NotNull(service);

        }



        [Fact]
        public async Task Test_AddItem()
        {
            // Arrange 
            var service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            Assert.NotNull(service);
            
            TodoItemData item = new TodoItemData { Title = "Title", Description = "Description" };            

            // Act
            service!.AddItem(item);
            await service.SaveAsync();
            // Assert
        }

        [Fact]
        public async void Test_AddTwoItems()
        {
            // Arrange
            var service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            Assert.NotNull(service);
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            // Act
            service.AddItem(item1);
            service.AddItem(item2);
            await service.SaveAsync();

            // Assert
        }

        [Fact]
        public async Task Test_GetItemsAsync()
        {
            // Arrange
            var service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            Assert.NotNull(service);
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            service!.AddItem(item1);
            service.AddItem(item2);
            await service.SaveAsync();

            // Act
            var items =  service.GetItems(archived: false);

            // Assert
            var count = items.ToList().Count;

            foreach (var item in items)
            { 
                Assert.Contains ("Title", item.Title); 
                Assert.Contains ("Description", item.Description); 
            }
            

        }
        [Fact]
        public async Task Test_GetItemByID()
        {
            // Arrange
            var service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            Assert.NotNull(service);
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service!.AddItem(item1);
            await service.SaveAsync();

            // Act
            var result = service.GetItemByID(item1.ID);
            
            // Assert
            Assert.True(item1.Title == result?.Title);

        }
        [Fact]
        public async Task Test_UpdateItem()
        {
            // Arrange
            var service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            Assert.NotNull(service);
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service!.AddItem(item1);
            var item = service.GetItemByID(item1.ID);
            TodoItemData updateItem = new TodoItemData { Title = item1.Title, Description = item1.Description };

            // Act
            service.UpdateItem(item1.ID, updateItem);
            await service.SaveAsync();

            // Assert 

            
        }
        
        [Fact] 
        async Task Test_DeleteItem()
        {
            // Arrange
            var service = _fixture.GetService<ITodoItemService>(_testOutputHelper);
            Assert.NotNull(service);
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);
            var item = service.GetItemByID(item1.ID);
            // Act
            if (item is not null) service.DeleteItem(item);
            await service.SaveAsync();
            // Assert

        }
    }

}