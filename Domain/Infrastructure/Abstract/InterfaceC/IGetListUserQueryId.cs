using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Abstract.InterfaceC
{
    public interface IGetListUserQueryId
    {
            Task<object> QueryGetListId
          (
             object Data
          );
    }
}
