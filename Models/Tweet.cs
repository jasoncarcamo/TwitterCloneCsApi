using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterCloneCs.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Tweet_text { get; set; }
        public string Screen_name { get; set; }

        [ForeignKey("Users")]
        public int User_id { get; set; }
    }
}
