using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataModel
{
    public class MediatoriDbContext : DbContext
    {
        public MediatoriDbContext()            : base("DefaultConnection")
        {
            Database.SetInitializer<MediatoriDbContext>(new CreateDatabaseIfNotExists<MediatoriDbContext>());
               //Database.SetInitializer<MediatoriDbContext>(new DropCreateDatabaseIfModelChanges<MediatoriDbContext>());
            //Database.SetInitializer<MediatoriDbContext>(new DropCreateDatabaseAlways<MediatoriDbContext>());
            //Database.SetInitializer<MediatoriDbContext>(new SchoolDBInitializer());
        }


       

        public DbSet<Models.Anagrafiche.TestAnagrafiche> testAnagrafiche { get; set; }
    }
}
