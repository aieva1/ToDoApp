using AutoMapper;
using DailyAppAPI.DataModel;
using DailyAppAPI.DTOs;

namespace DailyAppAPI.AutoMappers
{
    public class AutoMapperSettings : Profile

    {
        public AutoMapperSettings() 
        {
            CreateMap<AccountInfoDTO,AccountInfo>().ReverseMap();
            CreateMap<ToDoDTO, ToDoInfo>().ReverseMap();
            CreateMap<MemoDTO, MemoInfo>().ReverseMap();
        }
    }
}
