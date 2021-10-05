using AutoMapper;
using Easy.Transfers.Domain.MappersProfile;

namespace Easy.Transfers.Tests.Shared.Mock.Injection.Mappers
{
    public static class MappersMock
    {
        public static IMapper GetMock()
        {
            var configuration = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<TransactionProfile>();
                });

            return new Mapper(configuration);
        }
    }
}
