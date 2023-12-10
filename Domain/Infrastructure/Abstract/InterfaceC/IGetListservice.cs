using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Abstract.InterfaceC
{
    internal interface IGetListservice
    {
        Task<object> QueryGetList
      (
        object Data
      );
    }
}
