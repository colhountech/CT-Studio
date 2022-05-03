using AutoMapper;
using TasksWebApp.Services;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Profiles
{
    public class TodoItemProfile  : Profile
    {
        public TodoItemProfile()
        {

            CreateMap<TodoItemData, TodoItemViewModel>().ReverseMap();
            CreateMap<MessageData, MessageViewModel>().ReverseMap();
        }

    }
}
