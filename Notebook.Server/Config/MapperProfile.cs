using AutoMapper;
using Notebook.Server.Dto;
using Notebook.Server.Domain;

namespace Notebook.Server.Config
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<NoteRequest, Note>();
            CreateMap<Note, NoteModel>();
            CreateMap<CreateAccountRequest, Account>();
            CreateMap<Account, AccountModel>();
            CreateMap<RestoreAccount, RestoreAccountModel>();
            CreateMap<User, UserModel>();
        }
    }
}
