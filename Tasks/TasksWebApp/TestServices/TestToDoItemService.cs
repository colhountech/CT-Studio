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
            ToDoItemService service = new ToDoItemService();

            Assert.NotNull(service);

        }
        [Fact]
        public void Test_AddItem()
        {
            // 
            var service = new ToDoItemService();
            TodoItemData item = new TodoItemData { Title = "Title", Description = "Description" };            
            var result = service.AddItem(item);
            Assert.True(Guid.Empty != result);
        }

        [Fact]
        public void Test_AddTwoItems()
        {
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            service.AddItem(item1);
            service.AddItem(item2);
        }

        [Fact]
        public void Test_GetItems()
        {
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            TodoItemData item2 = new TodoItemData { Title = "Title2", Description = "Description2" };
            service.AddItem(item1);
            service.AddItem(item2);

            var items = service.GetItems();
            foreach (var item in items)
            { 
                Assert.Contains ("Title", item.Title); 
                Assert.Contains ("Description", item.Description); 
            }
            

        }
        [Fact]
        public void Test_GetItemByID()
        {
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);

            var result = service.GetItemByID(item1.ID);
            Assert.NotNull(result);
            Assert.True(item1.Title == result?.Title);

        }
        [Fact]
        public void Test_UpdateItem()
        {
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);
            var item = service.GetItemByID(item1.ID);
            TodoItemData updateItem = new TodoItemData { Title = item1.Title, Description = item1.Description };
            var result = service.UpdateItem(item1.ID, updateItem);
            Assert.True(result);

            
        }
        [Fact] void Test_DeleteItem()
        {
            var service = new ToDoItemService();
            TodoItemData item1 = new TodoItemData { Title = "Title1", Description = "Description1" };
            service.AddItem(item1);
            var item = service.GetItemByID(item1.ID);
            var result = service.DeleteItem(item);
            Assert.True(result);

        }
    }

}