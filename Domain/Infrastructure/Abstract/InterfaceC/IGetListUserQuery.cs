

namespace Domain.Infrastructure.Abstract.InterfaceC
{
    public interface IGetListUserQuery
    {
        Task<object> QueryGetList
      (
         object Data
      );

    }
}