using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCExample.Entities;

namespace MVCExample.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ChannelContext _context;

        public ChannelController(ChannelContext context)
        {
            _context = context;
        }

        // GET: Channel
        public async Task<IActionResult> Index()
        {
            return View(await _context.Channels.ToListAsync());
        }

        // GET: Channel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels
                .FirstOrDefaultAsync(m => m.ChannelId == id);
            if (channelModel == null)
            {
                return NotFound();
            }

            return View(channelModel);
        }

        // GET: Channel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Channel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChannelId,ChannelName,Endpoint,SecretKey,TokenKey")] ChannelModel channelModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(channelModel);
                await _context.SaveChangesAsync();
                channelModel.TokenKey = AppHelper.GetNewToken(channelModel.ChannelId, "MVCExample", channelModel.ChannelName, channelModel.SecretKey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(channelModel);
        }

        // GET: Channel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels.FindAsync(id);
            if (channelModel == null)
            {
                return NotFound();
            }

            List<EventModel> events = await _context.Events.ToListAsync();
            List<SubscribeModel> subscribes = await _context.Subscribers.Where(f => f.ChannelId == channelModel.ChannelId).ToListAsync();
            channelModel.Events = new List<EventModel>();
            foreach (var it in events)
            {
                SubscribeModel subscribeModel = subscribes.Find(f => f.EventId == it.EventId);
                if (subscribeModel != null) it.IsActive = true;
                else it.IsActive = false;
                channelModel.Events.Add(it);
            }

            return View(channelModel);
        }

        // POST: Channel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChannelId,ChannelName,Endpoint,SecretKey,TokenKey,Events")] ChannelModel channelModel)
        {
            if (id != channelModel.ChannelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(channelModel);
                    await _context.SaveChangesAsync();

                    List<SubscribeModel> subscribes = await _context.Subscribers.Where(f => f.ChannelId == channelModel.ChannelId).ToListAsync();
                    foreach (var it in channelModel.Events)
                    {
                        SubscribeModel subscribeModel = subscribes.Find(f => f.EventId == it.EventId);
                        if (subscribeModel != null)
                        {
                            if (!it.IsActive)
                            {
                                var subscriberModel = await _context.Subscribers.FirstOrDefaultAsync(f => f.ChannelId == channelModel.ChannelId && f.EventId == it.EventId);
                                if (subscriberModel != null)
                                {
                                    _context.Subscribers.Remove(subscriberModel);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            if (it.IsActive)
                            {
                                SubscribeModel subscribe = new SubscribeModel();
                                subscribe.ChannelId = channelModel.ChannelId;
                                subscribe.EventId = it.EventId;
                                subscribe.EventName = it.EventName;
                                _context.Add(subscribe);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelModelExists(channelModel.ChannelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(channelModel);
        }

        // GET: Channel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels
                .FirstOrDefaultAsync(m => m.ChannelId == id);
            if (channelModel == null)
            {
                return NotFound();
            }

            return View(channelModel);
        }

        // POST: Channel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var channelModel = await _context.Channels.FindAsync(id);
            _context.Channels.Remove(channelModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChannelModelExists(int id)
        {
            return _context.Channels.Any(e => e.ChannelId == id);
        }
    }
}
