using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SplitwiseAPI.Hubs;

namespace SplitwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<NotifyHub, INotifyHub> _hubContext;

        public MessageController(IHubContext<NotifyHub, INotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]Message msg)
        {
            string retMessage;
            try
            {
                _hubContext.Clients.All.BroadcastMessage(msg.Type, msg.Payload);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
    }
}