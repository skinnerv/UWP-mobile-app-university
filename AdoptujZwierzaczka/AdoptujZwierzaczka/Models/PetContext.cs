using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AdoptujZwierzaczka.Models
{
    class PetContext : DbContext
    {
        public DbSet<Pet> Pets { get; set; }
        public PetContext() : base("PetContext")
        {
            Database.SetInitializer<PetContext>(new DropCreateDatabaseIfModelChanges<PetContext>());
        }
    }
}
