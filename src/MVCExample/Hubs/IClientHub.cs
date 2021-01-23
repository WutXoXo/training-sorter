using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCExample.Hubs
{
    public interface IClientHub
    {
        Task MessageEvent(string message);
    }
}
