using Domain.Configuration;
using Domain.Infrastructure.Abstract;
using Domain.Infrastructure.Abstract.InterfaceC;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace Domain.User.Query
{
        public class GetListUserId : IGetListUserQuery
        {
            private readonly IConfiguration _configuration;
            public GetListUserId(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task<object> QueryGetList(object Data)
            {
                var ObjGet = new object();
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("BDSql");
                SqlDataReader myReader;
                int idnum = 0, reference;
                if (Data is not null)
                {
                    idnum = Convert.ToInt32(Data);
                    reference = 4;

                }
                else { idnum = 1; reference = 1; }
                try
                {
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        using (SqlCommand myCommand = new SqlCommand("sp_User", myCon))
                        {
                            myCommand.CommandType = CommandType.StoredProcedure;
                            myCommand.Parameters.Add("@reference", SqlDbType.Int).Value = reference;
                            myCommand.Parameters.Add("@Identi", SqlDbType.Int).Value = 1;
                            myCommand.Parameters.Add("@DateC", SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now);
                            myCommand.Parameters.Add("@Phone", SqlDbType.Int).Value = 1;
                            myCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = "";
                            myCommand.Parameters.Add("@Gender", SqlDbType.VarChar).Value = "";
                            myCommand.Parameters.Add("@positionCompani", SqlDbType.VarChar).Value = "";
                            myCommand.Parameters.Add("@Id", SqlDbType.Int).Value = idnum;
                            myCommand.Parameters.Add("@Active", SqlDbType.Bit).Value = true;


                            await myCon.OpenAsync();
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            myReader.Close();
                            myCon.Close();
                        }
                    }
                }
                catch (Exception ex) { return "error BD" + ex; }

                return table;
            }
        }
    }