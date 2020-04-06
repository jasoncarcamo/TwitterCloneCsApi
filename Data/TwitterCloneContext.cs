using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwitterCloneCs.Models;

namespace TwitterCloneCs.Data
{
    public class TwitterCloneContext : DbContext
    {
        public TwitterCloneContext( DbContextOptions<TwitterCloneContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<Tweet> Tweet { get; set; }

        public DbSet<Follow> Follow { get; set; }
    }
}
