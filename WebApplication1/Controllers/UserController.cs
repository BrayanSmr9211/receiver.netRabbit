using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using Domain.Infrastructure.Abstract.InterfaceC;
using System.Security.Claims;
using Domain.Infrastructure.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : BaseController
    {

        private readonly IConfiguration _configuration;
        private readonly IGetListUserQuery GetListUserQuery;
        private readonly IPostCommand PostCommand;
        private readonly IPutCommand PutCommand;
        private readonly IDeletCommand DeleteCommand;
        private readonly ITokenJwt TokenJwt;

        public UserController(IConfiguration configuration, IGetListUserQuery GetListUserQuery, IPostCommand PostCommand, IDeletCommand DeleteCommand, IPutCommand PutCommand, ITokenJwt TokenJwt)
        {
            _configuration = configuration;
            this.GetListUserQuery = GetListUserQuery;
            this.PostCommand = PostCommand;
            this.DeleteCommand = DeleteCommand;
            this.PutCommand = PutCommand;
            this.TokenJwt = TokenJwt;
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<JsonResult> GetTurnoAsync()
        {
            string dataJ ="";
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            if (ResopneToken.Result == "Token incorrect")
            {
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            {
                var GetData = await GetListUserQuery.QueryGetList(dataJ);
                return new JsonResult(GetData);
            }
            
        }

        [Authorize]
        [HttpGet("GetUserList/{id}")]
        public async Task<JsonResult> GetId(int id)

        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            ///ya tiene el token pero me falta validar la respuesta con el token y dar le valides para los otras apis

            if (ResopneToken.Result == "Token incorrect")
            {
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            { var GetData = await GetListUserQuery.QueryGetList(id.ToString());
            return new JsonResult(GetData);
            }

               
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Post(UserModel Est)
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            ///ya tiene el token pero me falta validar la respuesta con el token y dar le valides para los otras apis

            if (ResopneToken.Result == "Token incorrect")
            {
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            {
                var GetData = await PostCommand.PostCommandData(Est);
                return new JsonResult(GetData);

            }
              
        }
        [HttpPut]
        [Authorize]
        public async Task<JsonResult> Put(UserModel Est)
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            ///ya tiene el token pero me falta validar la respuesta con el token y dar le valides para los otras apis

            if (ResopneToken.Result == "Token incorrect")
            {
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            {
                var GetData = await PutCommand.PutCommandData(Est);
                return new JsonResult(GetData);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            ///ya tiene el token pero me falta validar la respuesta con el token y dar le valides para los otras apis

            if (ResopneToken.Result == "Token incorrect")
            {
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            {
                var GetData = await DeleteCommand.DeletCommandDat(id);
                return new JsonResult("Delete Successfully");
            }
        }
    }
}
