using AutoMapper;
using Tasks.AppData;
using TasksWebApp.ViewModels;

namespace TasksWebApp.Profiles
{
    public class TodoItemProfile  : Profile
    {
        public TodoItemProfile()
        {

            CreateMap<TodoItemData, TodoItemViewModel>();
            CreateMap<MessageData, MessageViewModel>().ReverseMap();

            // if ID is Empty in View Model, Ignore Mapping and it will pick up the newGuid() in the Data Model
            CreateMap<TodoItemViewModel, TodoItemData>()
                .ForMember(
                dest => dest.ID, 
                opt => opt.Condition(source => source.ID != Guid.Empty) );

        }

    }
}
