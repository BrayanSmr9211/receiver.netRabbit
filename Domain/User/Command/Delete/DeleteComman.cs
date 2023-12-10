using Domain.Infrastructure.Abstract.InterfaceC;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Domain.User.Command.Delete
{
    public class DeleteComman : IDeletCommand
    {
        private readonly IConfiguration _configuration;
        public DeleteComman(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<object> DeletCommandDat(object Data)
        {
            var ObjGet = new object();
            string sqlDataSource = _configuration.GetConnectionString("BDSql");
            SqlDataReader myReader;
            DataTable table = new DataTable();
            string query = @"DELETE FROM[Test].[dbo].[User] where [Id]=@Data";
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                try
                {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Data", Data);
                    myReader = await myCommand.ExecuteReaderAsync();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
                }
                catch (Exception ex)
                {
                    ObjGet = ("Error" + ex.Message);
                }
            }
            return ObjGet;
        }
            
    }

}