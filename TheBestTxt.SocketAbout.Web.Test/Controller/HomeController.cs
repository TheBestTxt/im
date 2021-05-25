using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBestTxt.SocketAbout.Common;

namespace TheBestTxt.SocketAbout.Web.Test.Controller
{
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            var clients = WebsocketClientCollection.GetAll();
            clients.ForEach(item =>
            item.SendMessageAsync("请不要传播谣言！"));
            return Ok();
        }
    }
}
