using Microsoft.Extensions.Configuration;
using System;

namespace Domain.Abstract
{
    public class IInternalService
    {
        private readonly string _connection;

        public IInternalService(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("LocalDb");
        }


    }
}
