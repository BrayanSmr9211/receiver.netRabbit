using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Domain.Infrastructure.Abstract.Rabbit;

namespace Domain.RabbitWork
{
    public class SigninWorker : IRabbitcons
    {
        private readonly IConfiguration _configuration;
        private readonly List<string> _receiveMessage = new List<string>();
        public SigninWorker(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<object> SigninoutWorker(int id, int queue)
        {
            var Inworker = 0;
            var ObjGet = new object();
            DateTime Todayturn = DateTime.Now;
            DateTime SingHour;
            bool stateWorker = true;

            if (queue == 1)
            {
                try
                {
                    DataTable table = new DataTable();
                    stateWorker = true;
                    string sqlDataSource = _configuration.GetConnectionString("BDSql");
                    SqlDataReader myReader;
                    string query = @" UPDATE [dbo].[Wockers] SET [Active] = @stateWorker,[Singin] = @Todayturn WHERE Identification=@id";
                    try
                    {
                        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                        {
                            myCon.Open();
                            using (SqlCommand myCommand = new SqlCommand(query, myCon))
                            {
                                myCommand.Parameters.AddWithValue("@id", id);
                                myCommand.Parameters.AddWithValue("@Todayturn", Todayturn);
                                myCommand.Parameters.AddWithValue("@stateWorker", stateWorker);
                                myReader = await myCommand.ExecuteReaderAsync();
                                table.Load(myReader);
                                myReader.Close();
                                myCon.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGet = ex.Message.ToString();
                    }

                    return new JsonResult(_receiveMessage);
                }
                catch (Exception ex)
                {
                    ObjGet = ex.Message.ToString();
                }
                return new JsonResult(_receiveMessage);
            }
            if (queue == 2)
            {
                DataTable table = new DataTable();
                stateWorker = false;
                string sqlDataSource = _configuration.GetConnectionString("BDSql");
                SqlDataReader myReader;
                string query = @" UPDATE [dbo].[Wockers] SET [Active] = @stateWorker,[Singin] = @Todayturn WHERE Identification=@id";
                try
                {
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(query, myCon))
                        {
                            myCommand.Parameters.AddWithValue("@id", id);
                            myCommand.Parameters.AddWithValue("@Todayturn", Todayturn);
                            myCommand.Parameters.AddWithValue("@stateWorker", stateWorker);
                            myReader = await myCommand.ExecuteReaderAsync();
                            table.Load(myReader);
                            myReader.Close();
                            myCon.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGet = ex.Message.ToString();
                }

                return new JsonResult(_receiveMessage);
            }
            if (queue == 3)
            {
                stateWorker = false;
                string sqlDataSource = _configuration.GetConnectionString("BDSql");
                SqlDataReader myReader;
                string query = @" select [Singin]  FROM [Test].[dbo].[Wockers] where Identification=@id";
                try
                {
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(query, myCon))
                        {
                            myCommand.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader reader = myCommand.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        _receiveMessage.Add(reader["Singin"].ToString());
                                    }
                                }
                                reader.Close();
                                myCon.Close();
                            }
                        }
                    }
                    SingHour = Convert.ToDateTime(_receiveMessage[0]);
                    TimeSpan Diferenthour = Todayturn.AddHours(8) - SingHour;
                    if (SingHour <= Todayturn.AddHours(8))
                    {
                        _receiveMessage.Clear();
                        _receiveMessage.Add("Permission allow.");
                    }
                    else
                    {
                        _receiveMessage.Clear();
                        _receiveMessage.Add("Permission denied, work time ended.");
                    }
                }
                catch (Exception ex)
                {
                    ObjGet = ex.Message.ToString();
                }
                return new JsonResult(_receiveMessage);
            }
            return new JsonResult(_receiveMessage);
        }

    }
}
