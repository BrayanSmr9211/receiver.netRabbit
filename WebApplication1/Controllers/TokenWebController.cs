using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;
using Domain.Infrastructure.Abstract.InterfaceC;
using Microsoft.AspNetCore.Authorization;
using Core.Models;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   

    public class TokenWebController : BaseController
    {
        private readonly IPostCommand PostCommand;
        private readonly IConfiguration _configuration;
        private readonly IPostCommandJwt PostCommandJwt;

        public TokenWebController(IPostCommand PostCommand,IConfiguration configuration, IPostCommandJwt PostCommandJwt)
        {
            _configuration = configuration;
            this.PostCommandJwt = PostCommandJwt;
            this.PostCommand = PostCommand;
        }

        [HttpPost]
        public async Task<JsonResult> Post(LoginToken Est)
        {
            var GetData = await PostCommandJwt.PostCommandJwtR(Est);
            return new JsonResult(GetData);
        }

        [HttpPost("RegisterUser")]
        public async Task<JsonResult> Post(UserModel Est)
        {
                var GetData = await PostCommand.PostCommandData(Est);
                return new JsonResult(GetData);

        }
    }
}
