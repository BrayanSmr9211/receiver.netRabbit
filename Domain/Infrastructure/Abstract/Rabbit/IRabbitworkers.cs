using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Abstract
{
    public interface IRabbitworkers
    {
        Task<object> CreatRabbit
     (
        ClaimsIdentity identity
     );
    }
}
