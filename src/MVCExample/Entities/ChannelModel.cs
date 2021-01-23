using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCExample.Entities
{
    [Table("M_CHANNEL")]
    public class ChannelModel
    {        
        [Key]
        [Required]
        [Column("CHANNEL_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelId { get; set; }
        
        [Required]
        [StringLength(255)]
        [Column("CHANNEL_NAME")]
        public string ChannelName { get; set; }

        [Required]
        [StringLength(255)]
        [Column("END_POINT")]
        public string Endpoint { get; set; }

        
        [Required]
        [StringLength(255)]
        [Column("SECRET_KEY")]
        public string SecretKey { get; set; }
        
        [Column("TOKEN_KEY")]
        public string TokenKey { get; set; }

        [NotMapped]
        public List<EventModel> Events { get; set; }
    }
}
