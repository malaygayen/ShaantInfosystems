using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaantInfosystems.Web.Models;

namespace ShaantInfosystem.Web.Data
{
    public class ShaantInfosystemWebContext : DbContext
    {
        public ShaantInfosystemWebContext (DbContextOptions<ShaantInfosystemWebContext> options)
            : base(options)
        {
        }

        public DbSet<ShaantInfosystems.Web.Models.NseModel>? NseModel { get; set; }
    }
}
