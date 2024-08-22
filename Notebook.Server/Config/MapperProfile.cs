using AutoMapper;
using Notebook.Server.Dto;
using Notebook.Server.Domain;
using Notebook.Server.Enum;
using Notebook.Server.Dto.Enum;

namespace Notebook.Server.Config
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RoleName, RoleNameModel>();
            CreateMap<Role, RoleModel>();

            CreateMap<NoteRequest, Note>();
            CreateMap<Note, NoteModel>();
            CreateMap<RestoreUser, RestoreUserModel>();
            CreateMap<User, UserModel>()
            .ForMember(dest => dest.RoleModels, opt => opt.MapFrom(src => src.Roles))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes)); ;

        }
    }
}
