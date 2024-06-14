using AutoMapper;
using Finance_Management.Data;
using Finance_Management.Models;

namespace Finance_Management.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ExpenseCreate, Expense>();
            CreateMap<Expense, ExpenseCreate>();
            CreateMap<ExpenseUpdateDTO, Expense>();
            CreateMap<Expense, ExpenseUpdateDTO>();

            CreateMap<ExpenseGetDTO, Expense>();
            CreateMap<Expense, ExpenseGetDTO>();


            CreateMap<SubscriptionCreateDTO, Subscription>();
            CreateMap<SubscriptionUpdateDTO, Subscription>();
            CreateMap<Subscription, SubscriptionCreateDTO>();
            CreateMap<Subscription, SubscriptionUpdateDTO>();

        }
    }
}
