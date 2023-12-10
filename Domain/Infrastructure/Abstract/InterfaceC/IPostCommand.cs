using WebApplication1.Models;

namespace Domain.Infrastructure.Abstract.InterfaceC

{
    public interface IPostCommand
    {
        Task<object> PostCommandData
      (
         UserModel Data
      );
    }
}