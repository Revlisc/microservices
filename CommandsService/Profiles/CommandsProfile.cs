using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // src -> target
            CreateMap<PlatformID, PlatformReadDTO>();
            CreateMap<CommandCreateDTO, CommandCreateDTO>();
            CreateMap<Command, CommandReadDTO>();
        }
    }
}