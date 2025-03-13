using AutoMapper;
using CashFlowControl.Application.DTOs;
using CashFlowControl.Domain.Entities;
using CashFlowControl.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlowControl.Application.Mapping
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<TransactionDTO, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src =>
                src.Type.Equals("Crédito", StringComparison.OrdinalIgnoreCase)
                    ? TransactionType.Credit
                    : TransactionType.Debit))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src =>
                    src.Type == TransactionType.Credit ? "Crédito" : "Débito"))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
        }
    }
}
