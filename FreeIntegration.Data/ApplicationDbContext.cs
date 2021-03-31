
using FreeIntegration.Models.Login;
using FreeIntegration.Models.Settings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeIntegration.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
            
        }
        public DbSet<LoginUserLogDT> Logins { get; set; }
        public DbSet<SettingsDT> Settings { get; set; }
    }
}
