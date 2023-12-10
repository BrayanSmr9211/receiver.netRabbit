using Domain.Configuration;
using Domain.Infrastructure.Abstract.InterfaceC;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace Domain.User.Command.Put
{
    public class PutCommand : IPutCommand
    {
        private readonly IConfiguration _configuration;
        private readonly AppOption options;
        public PutCommand(IConfiguration configuration, AppOption options)
        {
            _configuration = configuration;
            this.options = options;
        }

        public async Task<object> PutCommandData(UserModel Data)
        {
            var ObjGet = new object();
            ObjGet = null;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BDSql");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("sp_GroupMok", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@reference", SqlDbType.Int).Value = 3;
                    myCommand.Parameters.Add("@Identi", SqlDbType.Int).Value = Data.Identification;
                    myCommand.Parameters.Add("@DateC", SqlDbType.DateTime).Value = Convert.ToDateTime(Data.datec);
                    myCommand.Parameters.Add("@Phone", SqlDbType.Int).Value = Data.Phone;
                    myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = Data.name;
                    myCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Data.gender;
                    myCommand.Parameters.Add("@positionCompani", SqlDbType.VarChar).Value = Data.Companyposition;
                    myCommand.Parameters.Add("@posicion", SqlDbType.Int).Value = 1;
                    myCommand.Parameters.Add("@Id", SqlDbType.Int).Value = 2;
                    myCommand.Parameters.Add("@Active", SqlDbType.Bit).Value = true;
                    try
                    {

                        await myCon.OpenAsync();
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                        ObjGet = ("Creacion nuevo usuario correcto");
                    }
                    catch (Exception ex)
                    {
                        ObjGet = ("Error" + ex.Message);
                    }
                }
            }

            return ObjGet;
        }
    }

}

