using WebApplication1.Models;

namespace Domain.Infrastructure.Abstract.InterfaceC
{
    public interface IPutCommand
    {
        Task<object> PutCommandData
      (
         UserModel Data
      );
    }
}
