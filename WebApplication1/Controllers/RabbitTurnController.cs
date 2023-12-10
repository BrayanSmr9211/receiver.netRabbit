using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Infrastructure.Abstract;
using Domain.Infrastructure.Abstract.Rabbit;
using System.Linq;
using System;

namespace WebApplication1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RabbitTurnController : BaseController
    {
        private readonly ITokenJwt TokenJwt;
        private readonly IRabbitworkers Rabbitworkers;
        private readonly IQueueWork QueueWork;
        private readonly IRabbitcons Rabbitcons;

        public RabbitTurnController(IRabbitcons rabbitcons,ITokenJwt TokenJwt, IRabbitworkers rabbitworkers, IQueueWork queueWork)
        {
            this.TokenJwt = TokenJwt;
            this.Rabbitworkers = rabbitworkers;
            this.QueueWork = queueWork;
            this.Rabbitcons = rabbitcons;
        }

        [HttpGet]
        public async Task<JsonResult> CreatQueue()
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            if (ResopneToken.Result == "Token incorrect")
            {
                var id = Identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
                Rabbitcons.SigninoutWorker(Convert.ToInt32(id), 2);
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            {
                var GetData = await Rabbitworkers.CreatRabbit(Identity);
                return new JsonResult(GetData);
            }
        }
        [HttpGet("ConsumQueue")]
        public async Task<JsonResult> QueueWorker()
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            var ResopneToken = TokenJwt.ValidTokenWeb(Identity);

            if (ResopneToken.Result == "Token incorrect")
            {
                var id = Identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
                Rabbitcons.SigninoutWorker(Convert.ToInt32(id), 2);
                return new JsonResult("Error Token" + ResopneToken.Result);
            }
            else
            {
                var GetData = await QueueWork.QueueWork(Identity);
                return new JsonResult(GetData);
            }
        }
    }
}
