using InspectorLogWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InspectorLogWebApplication.Data
{
    public class InspectorLogContext : DbContext
    {
        public InspectorLogContext(DbContextOptions<InspectorLogContext> options) : base(options)
        {

        }
        public DbSet<Inspector>? Inspectors { get; set; }
        public DbSet<ViolationType>? ViolationTypes { get; set; }
        public DbSet<OwnershipType>? OwnershipTypes { get; set; }
        public DbSet<Enterprise>? Enterprises { get; set; }
        public DbSet<Inspection>? Inspections { get; set; }
        public DbSet<Violation>? Violations { get; set; }
    }
}
