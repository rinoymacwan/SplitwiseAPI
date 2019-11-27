using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SplitwiseAPI.Hubs;
using SplitwiseAPI.Models;

namespace SplitwiseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<NotifyHub, INotifyHub> _hubContext;
        private IHubContext<ChatHub> _newhub;
        private readonly IHubContext<ChatHub> _signalHubContext;

        public MessageController(IHubContext<NotifyHub, INotifyHub> hubContext, IHubContext<ChatHub> signalHubContext)
        {
            _hubContext = hubContext;
            _signalHubContext = signalHubContext;
        }

        //[HttpPost]
        //public async Task<string> Post([FromBody]Message msg)
        //{
        //    string retMessage;
        //    try
        //    {

        //        //working
        //        //await _hubContext.Clients.All.BroadcastMessage(msg.Type, msg.Payload);

        //        await _hubContext.Clients.All.SendMessage(msg.Type, msg.Payload);

        //        //ChatHub chub = new ChatHub();
        //        //await chub.Send("0d0e96dc-fdc6-405e-9ebf-d9f924a315dc", "ABCXSS");
        //        // await chub.Clients.User("0d0e96dc-fdc6-405e-9ebf-d9f924a315dc").SendAsync(msg.Type, msg.Payload);
        //        //_hubContext.Clients.User("0d0e96dc-fdc6-405e-9ebf-d9f924a315dc").BroadcastMessage(msg.Type, msg.Payload);
        //        //await _signalHubContext.Clients.All.SendAsync(msg.Type, msg.Payload);
        //        //await _newhub.Clients.All.SendAsync(msg.Type, msg.Payload);
        //        //SendMessage(msg.Type, msg.Payload);
        //        retMessage = "Success";
        //    }
        //    catch (Exception e)
        //    {
        //        retMessage = e.ToString();
        //    }
        //    return retMessage;
        //}
    }
}