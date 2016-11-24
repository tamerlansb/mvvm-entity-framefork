using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApplication
{
    public class Person
    {
        #region Fields
        private string lastname;
        private DateTime dateofbirth;
        private uint height;
        #endregion

        #region Constructors
        public Person(string LastName, DateTime DateOfBirth, uint Height)
        {
            this.dateofbirth = DateOfBirth;
            this.lastname = LastName;
            this.height = Height;
        }
        public Person(Person p)
        {
            this.dateofbirth = p.DateOfBirth;
            this.lastname = p.LastName;
            this.height = (uint)p.Height;
        }
        #endregion

        #region Propeties
        public string LastName
        {
            get { return lastname; }
            set
            {
                lastname = value;
            }
        }
        public DateTime DateOfBirth
        {
            get { return dateofbirth; }
            set
            {
                dateofbirth = value;
            }
        }
        public int Height
        {
            get { return (int)height; }
            set
            {
                height = (uint)value;
            }
        }
        [Key]
        public int PersonID
        {
            get; set;
        }
        public bool IsHead
        {
            get; set;
        }
        public int? NextID
       {
               get; set;
       }
       public Nullable<int> PrevID
        {
            get; set;
        }
        public string ShortDate
        {
            get { return dateofbirth.ToShortDateString(); }
        }
        #endregion
        public override string ToString()
        {
            return lastname + "\n   birthday:" + dateofbirth.ToShortDateString() + "   height:" + height.ToString();
        }

    }
    public class PersonContext : DbContext
    {
        public PersonContext() : base("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ")
        {
          // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PersonContext>());
        }
        public DbSet<Person> Persons { get; set; }
    }
}
