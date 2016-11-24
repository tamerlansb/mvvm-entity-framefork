using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataLayer
{
    public class PersonContext : DbContext
    {
        public PersonContext(string ConnectionString = "server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ") : 
            base(ConnectionString)
        {
        }
        public DbSet<PersonDataBase> Persons { get; set; }
    }
}
