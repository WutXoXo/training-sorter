using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCExample.Entities
{
    [Table("M_SUBSCRIBE")]
    public class SubscribeModel
    {       
        [Key]
        [Required]
        [Column("CHANNEL_ID")]
        public int ChannelId { get; set; }

        [Key]
        [Required]
        [Column("EVENT_ID")]
        public int EventId { get; set; }

        [StringLength(255)]
        [Column("EVENT_NAME")]
        public string EventName { get; set; }
    }
}
