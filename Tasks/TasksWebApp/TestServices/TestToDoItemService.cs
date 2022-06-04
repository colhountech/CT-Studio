using System;
using System.Linq;
using System.Threading.Tasks;
using TasksAppData;
using TasksWebApp.Services;
using Xunit;

namespace TestServices
{
    public class TestToDoItemService
    {        

        [Fact]
        public void Test_InitService()
        {
            // Arrange

            // Act 
            ToDoItemService service = new ToDoItemService();

            // Assert
            Assert.NotNull(service);

        }


        [Fact]
        public async void Test_InitServiceTwice()
        {
            // Arrange 
            ToDoItemService service1 = new ToDoItemService();
            ToDoItemService service2 = new ToDoItemService();

            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };

            // Act
            await service1.AddItemAsync(item1);
            await service2.AddItemAsync(item2);

            // Assert
            Assert.Collection(service1.GetItems(archived: false),
                item => Assert.Equal("Title1", item1?.Title),
                item => Assert.Equal("Title2", item2?.Title)
                );
        }


        [Fact]
        public async Task Test_AddItem()
        {
            // Arrange 
            var service = new ToDoItemService();

            TodoItemData item = new TodoItemData { Title = "Title", Description = "Description" };            

            // Act
            var result = await service.AddItemAsync(item);

            // Assert
            Assert.True(Guid.Empty != result);
        }

        [Fact]
        public async void Test_AddTwoItems()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            // Act
            await service.AddItemAsync(item1);
            await service.AddItemAsync(item2);

            // Assert
        }

        [Fact]
        public async void Test_GetItemsAsync()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            await service.AddItemAsync(item1);
            await service.AddItemAsync(item2);

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
        public async void Test_GetItemByID()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            await service.AddItemAsync(item1);

            // Act
            var result = service.GetItemByID(item1.ID);
            
            // Assert
            Assert.True(item1.Title == result?.Title);

        }
        [Fact]
        public async Task Test_UpdateItem()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            await service.AddItemAsync(item1);
            var item = service.GetItemByID(item1.ID);
            TodoItemData updateItem = new TodoItemData { Title = item1.Title, Description = item1.Description };

            // Act
            var result = await service.UpdateItemAsync(item1.ID, updateItem);

            // Assert 
            Assert.True(result);

            
        }
        
        [Fact] 
        async Task Test_DeleteItem()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            await service.AddItemAsync(item1);
            var item = service.GetItemByID(item1.ID);
            // Act
            if (item is not null) await service.DeleteItemAsync(item);

            // Assert

        }
    }

}