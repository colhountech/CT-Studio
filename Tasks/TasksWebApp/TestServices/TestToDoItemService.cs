using System;
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
        public void Test_InitServiceTwice()
        {
            // Arrange 
            ToDoItemService service1 = new ToDoItemService();
            ToDoItemService service2 = new ToDoItemService();

            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };

            // Act
            service1.AddItem(item1);
            service2.AddItem(item2);

            // Assert
            Assert.Collection(service1.GetItems(),
                item => Assert.Equal("Title1", item1?.Title),
                item => Assert.Equal("Title2", item2?.Title)
                );
        }


        [Fact]
        public void Test_AddItem()
        {
            // Arrange 
            var service = new ToDoItemService();

            TodoItemData item = new TodoItemData { Title = "Title", Description = "Description" };            

            // Act
            var result = service.AddItem(item);

            // Assert
            Assert.True(Guid.Empty != result);
        }

        [Fact]
        public void Test_AddTwoItems()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            // Act
            service.AddItem(item1);
            service.AddItem(item2);

            // Assert
        }

        [Fact]
        public void Test_GetItems()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            service.AddItem(item1);
            service.AddItem(item2);

            // Act
            var items = service.GetItems();

            // Assert

            foreach (var item in items)
            { 
                Assert.Contains ("Title", item.Title); 
                Assert.Contains ("Description", item.Description); 
            }
            

        }
        [Fact]
        public void Test_GetItemByID()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);

            // Act
            var result = service.GetItemByID(item1.ID);
            
            // Assert
            Assert.True(item1.Title == result?.Title);

        }
        [Fact]
        public void Test_UpdateItem()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);
            var item = service.GetItemByID(item1.ID);
            TodoItemData updateItem = new TodoItemData { Title = item1.Title, Description = item1.Description };

            // Act
            var result = service.UpdateItem(item1.ID, updateItem);

            // Assert 
            Assert.True(result);

            
        }
        [Fact] void Test_DeleteItem()
        {
            // Arrange
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);
            var item = service.GetItemByID(item1.ID);
            // Act
            var result = service.DeleteItem(item);

            // Assert
            Assert.True(result);

        }
    }

}