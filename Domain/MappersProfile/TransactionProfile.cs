using AutoMapper;
using Easy.Transfers.Domain.Commands.FundTransfer.Create;
using Easy.Transfers.Domain.Commands.FundTransfer.CreateTransaction;
using Easy.Transfers.Domain.Entities.Mongo;
using Easy.Transfers.Domain.Publishers;
using Easy.Transfers.Domain.Queries.FundTransfer.GetFundTransferByTransactionId;
using Easy.Transfers.Domain.Services;
using System;

namespace Easy.Transfers.Domain.MappersProfile
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CreateFundTransferCommand, Transaction>()
                .ForMember(dest => dest.AccountOrigin, opt => opt.MapFrom(src => src.AccountOrigin))
                .ForMember(dest => dest.AccountDestination, opt => opt.MapFrom(src => src.AccountDestination))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.CreatedTransaction, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Transaction, GetFundTransferByTransactionIdQueryResponse>()
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                  .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.MessageError));

            CreateMap<Transaction, TransactionEvent>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId));

            CreateMap<TransactionEvent, CreateTransactionFundTransferCommand>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId));

            CreateMap<Account, TransferAccount>()
               .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber));
               
        }
    }
}
