using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Incrementer.Models;

namespace Incrementer.Context
{
    public class IncrementContext : DbContext
    {
        public IncrementContext(DbContextOptions<IncrementContext> options) : base(options) { }
        
        public DbSet<KeyValue> KeyValues { get; set; }
    }
}
