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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=192.168.30.31; Initial Catalog=AbsCoreDevelopment; User Id=sa;pwd=fkt");
        }
    }

    public class HrContext : DbContext
    {
        public HrContext(DbContextOptions<HrContext> options):base(options)
        {

        }

        public DbSet<Employee> Employees{get;set;}

       

    }

    [Table(name:"Employees",Schema ="hrm")]
    public class Employee
    {
        public int Id { get;  set; }
        public string Name {get;set;}
        public bool IsStaff {get;set;}
        public bool IsWorking {get;set;}
    }

    public class Division
    {

    }
}
