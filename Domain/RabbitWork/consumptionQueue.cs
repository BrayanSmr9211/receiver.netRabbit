using Domain.Infrastructure.Abstract.Rabbit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Domain.RabbitWork
{
    public class consumptionQueue : IQueueWork
    {
        private readonly IConfiguration _configuration;
        private readonly List<string> _receiveMessage = new List<string>();
        private readonly IRabbitcons Rabbitcons;
        object ResponseTime =  new object ();

        public consumptionQueue(IConfiguration configuration, IRabbitcons rabbitcons)
        {
            this._configuration = configuration;
            Rabbitcons = rabbitcons;
        }
        public async Task<object> QueueWork(ClaimsIdentity identity)
        {
            var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
            var Inworker = 0;
            var ObjGet = new object();
             ResponseTime = Rabbitcons.SigninoutWorker(Convert.ToInt32(id), 3);
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BDSql");
            SqlDataReader myReader;
            string query = @"select Identification FROM [Test].[dbo].[Wockers] where Identification = @id";
            if(ResponseTime == "Permission allow.")
            {
                _receiveMessage.Add("Application is blocking job timeout");
                return new JsonResult(_receiveMessage);
            }
            else
            {
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" creacion {message}");
                    _receiveMessage.Add(message);
                };
                channel.BasicConsume(queue: id,
                                     autoAck: true,
                                     consumer: consumer);
                return new JsonResult(_receiveMessage);
            }
           
        }
    }
}
