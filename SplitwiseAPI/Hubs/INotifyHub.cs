using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Hubs
{
    public interface INotifyHub
    {
        Task BroadcastMessage(string type, string payload);
    }
}
