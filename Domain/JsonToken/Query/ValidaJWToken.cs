using Domain.Infrastructure.Abstract;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.JsonToken.Query
{
    public class ValidaJWToken : ITokenJwt
    {
        private readonly IConfiguration _configuration;
        public ValidaJWToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<object> ValidTokenWeb(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return "Token incorrect";
                       
                }
                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
                var ObjGet = new object();
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BDSql");
                SqlDataReader myReader;
                string query = @"select Identification FROM [Test].[dbo].[User] where Identification = @id";

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    try
                        {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                       
                            myCommand.Parameters.AddWithValue("@id", id);
                            myReader = await myCommand.ExecuteReaderAsync();
                            table.Load(myReader);
                            myReader.Close();
                            myCon.Close();
                     } }

                        catch (Exception ex)
                        {
                            ObjGet = ex.Message.ToString();
                        }
                    }
               
                return new
                {
                    success = true,
                    massage = "success",
                    result = "" + table
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    massage = "Error" + ex.Message,
                    result = ""
                };
            }
        }
    }
}