using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCExample.Entities
{
    public class ChannelContext : DbContext
    {
        public ChannelContext(DbContextOptions<ChannelContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChannelModel>()
            .HasKey(o => new { o.ChannelId })
            .HasName("PK_Channel");

            modelBuilder.Entity<SubscribeModel>()
            .HasKey(o => new { o.ChannelId, o.EventId })
            .HasName("PK_Subscribe");

            modelBuilder.Entity<EventModel>()
            .HasKey(o => new { o.EventId })
            .HasName("PK_Event");

            modelBuilder.Entity<EventModel>()
            .HasData(new EventModel[] {
                new EventModel(){ EventId = 809,EventName = "รับเข้าระบบ/collection mail",EventUrl = "/api/collection/mail"},
                new EventModel(){ EventId = 815,EventName = "บันทึกผลจัดส่ง/proof of delivery",EventUrl = "/api/delivery/proof"}
            });
        }

        public DbSet<ChannelModel> Channels { get; set; }
        public DbSet<SubscribeModel> Subscribers { get; set; }
        public DbSet<EventModel> Events { get; set; }
    }
}
