using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm
{
    public class PersonContext :DbContext
    {
        public PersonContext() : base("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ")
        {
        }
        public DbSet<PersonDataBase> Persons { get; set; }
    }
}
