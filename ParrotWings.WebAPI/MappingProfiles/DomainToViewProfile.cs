using AutoMapper;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Models.Domain.Transactions;
using ParrotWings.WebAPI.ViewModels.Account;
using ParrotWings.WebAPI.ViewModels.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.MappingProfiles
{
    public class DomainToViewProfile : Profile
    {
        public override string ProfileName => "Domain-to-view";

        public DomainToViewProfile()
        {
            CreateMap<User, UserProfileViewModel>();
            CreateMap<User, UserViewModel>();

            //Transactions
            CreateMap<Transaction, TransactionViewModel>();
            CreateMap<Transaction, TransactionDetailsViewModel>();
        }
    }
}
