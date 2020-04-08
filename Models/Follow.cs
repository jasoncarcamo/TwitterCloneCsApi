using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterCloneCs.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public int Follows { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("Users")]
        public int User_id { get; set; }
    }
}
