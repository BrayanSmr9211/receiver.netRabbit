using Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Domain.Infrastructure.Abstract.InterfaceC;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace Domain.Student.Query
{
    public class PostCommand : IPostCommand
    {
        private readonly IConfiguration _configuration; 
        private readonly AppOption options;
        public PostCommand(IConfiguration configuration, AppOption  options)
        {
            _configuration = configuration;
             this.options = options;
        }
  
        public async Task<object> PostCommandData(UserModel Data)
        {
            var ObjGet = new object();
            ObjGet = null;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BDSql");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand("sp_User", myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.Add("@reference", SqlDbType.Int).Value = 2;
                    myCommand.Parameters.Add("@Id", SqlDbType.Int).Value = 2;
                    myCommand.Parameters.Add("@Identi", SqlDbType.Int).Value = Data.Identification;
                    myCommand.Parameters.Add("@DateC", SqlDbType.DateTime).Value = Convert.ToDateTime( Data.datec);
                    myCommand.Parameters.Add("@Phone", SqlDbType.Int).Value = Data.Phone;
                    myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = Data.name;
                    myCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Data.gender;
                    myCommand.Parameters.Add("@positionCompani", SqlDbType.VarChar).Value = Data.Companyposition;
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
                        ObjGet= ("Error" + ex.Message);
                    }
                }
            }
            
           return ObjGet;
        }
       
        }

       
    }    

