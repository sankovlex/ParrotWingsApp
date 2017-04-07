using AutoMapper;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Models.Domain.Transactions;
using ParrotWings.WebAPI.ViewModels.Account;
using ParrotWings.WebAPI.ViewModels.Transactions;

namespace ParrotWings.WebAPI.MappingProfiles
{
    public class ViewToDomainProfile : Profile
    {
        public override string ProfileName => "View-to-domain";

        public ViewToDomainProfile()
        {
            CreateMap<UserLoginPostModel, User>();
            CreateMap<UserRegisterPostModel, User>();

            //transactions
            CreateMap<TransactionPostModel, Transaction>();
        }
    }
}
