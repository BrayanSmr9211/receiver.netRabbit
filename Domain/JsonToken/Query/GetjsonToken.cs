using Core.Models;
using Domain.Infrastructure.Abstract.InterfaceC;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Domain.Infrastructure.Abstract.Rabbit;

namespace Domain.JsonToken.Query
{
    public class GetjsonToken : IPostCommandJwt
    {
        private readonly IConfiguration _configuration;
        private readonly IRabbitcons Rabbitcons;
        public GetjsonToken(IConfiguration configuration, IRabbitcons rabbitcons)
        {
            _configuration = configuration;
            Rabbitcons = rabbitcons;
        }
        public async Task<object> PostCommandJwtR(LoginToken Data)
        {
            var ObjGetjson = new object();
            string ObjGetstring = "";
            var ObjGet = new object();
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BDSql");
            SqlDataReader myReader;
            var IDUSer = Data.IdUser;
            var Password = Data.password;
            string query = @"select * FROM [Test].[dbo].[Wockers] where [password] = @PasswordUSer and Identification = @IDUSer";
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {

                        myCommand.Parameters.AddWithValue("@IDUSer", IDUSer);
                        myCommand.Parameters.AddWithValue("@PasswordUSer", Password);
                        myReader = await myCommand.ExecuteReaderAsync();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                if (table is null || table.Rows.Count == 0)
                {
                    var p = new
                    {
                        success = true,
                        essage = "Error",
                        result = "No hay data"
                    };
                    return p;
                }
                else
                {
                    var Jwt = _configuration.GetSection("Jwt").Get<UserToken>();
                    var claims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, Jwt.subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id",Data.IdUser.ToString()),
                     new Claim("User",Data.password.ToString())
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key));
                    var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        Jwt.Issuer,
                        Jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: singIn
                        );
                    Rabbitcons.SigninoutWorker(IDUSer, 1);
                    return new
                    {
                        success = true,
                        essage = "200 Ok",
                        result = new JwtSecurityTokenHandler().WriteToken(token)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                ObjGet = ex.Message.ToString();
            }
            return null;
        } 
    } 
}