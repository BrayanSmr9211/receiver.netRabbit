using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Claims;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Domain.Infrastructure.Abstract;
using Domain.Infrastructure.Abstract.Rabbit;

namespace Domain.RabbitWork
{
    public class Rabbitworker: IRabbitworkers
    {
        private readonly List<string> _receiveMessage = new List<string>();
        private readonly IConfiguration _configuration;
        private readonly IRabbitcons Rabbitcons;

        public Rabbitworker(IConfiguration configuration, IRabbitcons rabbitcons)
        {
            _configuration = configuration;
            Rabbitcons = rabbitcons;
        }
        public async Task<object> CreatRabbit(ClaimsIdentity identity)
        {
            var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
            var Inworker = 0;
            var ObjGet = new object();
            Rabbitcons.SigninoutWorker(Convert.ToInt32(id), 3);
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BDSql");
            SqlDataReader myReader;
            string query = @"select Identification FROM [Test].[dbo].[Wockers] where Identification = @id";
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                try
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@id", id);
                        myReader = await myCommand.ExecuteReaderAsync();
                        myReader.Close();
                        myCon.Close();
                    }
                }
                catch (Exception ex)
                {
                    ObjGet = ex.Message.ToString();
                }
            }

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: id,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        
            return new JsonResult(_receiveMessage);
        }
    }
}
