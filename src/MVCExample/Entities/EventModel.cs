using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCExample.Entities
{
    [Table("ZZ_EVENT")]
    public class EventModel
    {
        [Key]
        [Column("EVENT_ID")]
        public int EventId { get; set; }

        [Column("EVENT_NAME")]
        [StringLength(255)]
        public string EventName { get; set; }

        [Column("EVENT_URL")]
        [StringLength(1000)]
        public string EventUrl { get; set; }

        [NotMapped]
        public bool IsActive { get; set; }
    }
}
