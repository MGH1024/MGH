using AutoMapper;
using MGH.Identity.Entities;
using MGH.Identity.Models;

namespace MGH.Identity.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Identity
        CreateMap<CreateUser, User>();
    }
}