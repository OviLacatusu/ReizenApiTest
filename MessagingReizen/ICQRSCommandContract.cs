using Reizen.CommonClasses;
using Reizen.Data.Models.CQRS;

namespace ReizenApi.Contracts
{
    public interface ICQRSCommandContract<T> : ICommand<Result<T>>
    {
        public int? Id { get; }
    }
}
