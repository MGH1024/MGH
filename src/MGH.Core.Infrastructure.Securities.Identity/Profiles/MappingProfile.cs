using AutoMapper;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using MGH.Core.Infrastructure.Securities.Identity.Models;

namespace MGH.Core.Infrastructure.Securities.Identity.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Identity
        CreateMap<CreateUser, User>();
    }
}