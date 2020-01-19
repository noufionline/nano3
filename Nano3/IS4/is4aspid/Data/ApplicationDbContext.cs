using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using is4aspid.Models;
using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations.Schema;

namespace is4aspid.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    public class HrContext : DbContext
    {
        public HrContext(DbContextOptions<HrContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }



    }

    public class NetSqlAzmanContext : DbContext
    {
        public NetSqlAzmanContext(DbContextOptions<NetSqlAzmanContext> options) : base(options)
        {

        }

        public DbSet<Division> Divisions {get;set; }
    }



    [Table(name: "Employees", Schema = "hrm")]
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStaff { get; set; }
        public bool IsWorking { get; set; }
    }
    [Table(name:"AbsDivisions")]
    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationType { get; set; }
    }
}
