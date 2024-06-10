using AutoMapper;
using Finance_Management.Data;
using Finance_Management.Models;

namespace Finance_Management.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ExpenseCreate, Expense>().ReverseMap();
            CreateMap<ExpenseUpdate, Expense>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<SubscriptionCreate, Subscription>().ReverseMap();
            CreateMap<SubscriptionUpdate, Subscription>().ReverseMap();
        }
    }
}
