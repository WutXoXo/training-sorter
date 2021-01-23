using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MVCExample.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MVCExample.Hubs
{
    public class BroadcastHub : Hub<IClientHub>, IServerHub
    {
        private readonly ChannelContext _context;

        public BroadcastHub(ChannelContext context)
        {
            _context = context;
        }

        public async Task MailCollectionEvent()
        {
            var channelModels = await (from c in _context.Channels
                                        join s in _context.Subscribers on c.ChannelId equals s.ChannelId
                                        join e in _context.Events on s.EventId equals e.EventId
                                        where s.EventId == 809
                                        select new { 
                                            c.ChannelId,
                                            c.ChannelName,
                                            c.Endpoint,
                                            c.SecretKey,
                                            s.EventId,
                                            e.EventName,
                                            e.EventUrl
                                        }).ToListAsync();

            if (channelModels != null && channelModels.Count() > 0)
            {
                foreach (var sender in channelModels)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(45);
                        httpClient.BaseAddress = new Uri(sender.Endpoint);
                        string token = AppHelper.GetNewToken(sender.ChannelId, "MVCExample", sender.ChannelName, sender.SecretKey);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(new { mailing_no = "EB300236469TH", scan_datetime = DateTime.Now }), Encoding.UTF8, "application/json");
                        await httpClient.PostAsync(sender.EventUrl, content);
                    }
                }
            }

            await Clients.Client(Context.ConnectionId).MessageEvent("Ok - Mail Collection");
        }

        public async Task ProofOfDeliveryEvent()
        {
            var channelModels = await(from c in _context.Channels
                                      join s in _context.Subscribers on c.ChannelId equals s.ChannelId
                                      join e in _context.Events on s.EventId equals e.EventId
                                      where s.EventId == 815
                                      select new
                                      {
                                          c.ChannelId,
                                          c.ChannelName,
                                          c.Endpoint,
                                          c.SecretKey,
                                          s.EventId,
                                          e.EventName,
                                          e.EventUrl
                                      }).ToListAsync();

            if (channelModels != null && channelModels.Count() > 0)
            {
                foreach (var sender in channelModels)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(45);
                        httpClient.BaseAddress = new Uri(sender.Endpoint);
                        string token = AppHelper.GetNewToken(sender.ChannelId, "MVCExample", sender.ChannelName, sender.SecretKey);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(new { mailing_no = "EA432268182TH", scan_datetime = DateTime.Now }), Encoding.UTF8, "application/json");
                        await httpClient.PostAsync(sender.EventUrl, content);
                    }
                }
            }

            await Clients.Client(Context.ConnectionId).MessageEvent("Ok - Proof Of Delivery");
        }
    }
}
