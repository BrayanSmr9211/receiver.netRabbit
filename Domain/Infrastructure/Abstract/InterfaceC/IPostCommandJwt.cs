
using Core.Models;
using WebApplication1.Models;

namespace Domain.Infrastructure.Abstract.InterfaceC
{
    public interface IPostCommandJwt  
    {
        Task<object> PostCommandJwtR
      (
         LoginToken Data
      );
    }
}
