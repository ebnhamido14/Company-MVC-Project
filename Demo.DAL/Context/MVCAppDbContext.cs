﻿using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Context
{
    public class MVCAppDbContext:IdentityDbContext<ApplicationUser>
    {
        public MVCAppDbContext(DbContextOptions<MVCAppDbContext> options):base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=>  optionsBuilder.UseSqlServer("Server=.;Database=MVCAppDb;Trusted_Connection=true;");

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        
    }
}
