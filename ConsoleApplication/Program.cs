using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubledLinkedList;
using Mvvm;
using System.Data.SqlClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace ConsoleApplication
{
   
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started\n");
            //var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            using (PersonContext db = new PersonContext())
            {
                Person p1 = new Person("Sidorov", new DateTime(1996, 01, 03), 179); 
                 Person p2 = new Person("Ivanov", new DateTime(1995, 03, 03), 170);
                    db.Persons.Add(p1);
                    db.Persons.Add(p2);
               //    p1.NextID = null; p2.PrevID = null;
             //      p2.NextID = null; p2.PrevID = null;
                    db.SaveChanges();
                IQueryable<string> names = (from person in db.Persons select person.LastName);
                foreach (var name in names)
                {
                    Console.WriteLine(name);
                    name.Replace("S", "t");
                }
                db.SaveChanges();
               
            }
            Console.WriteLine("Ended");
            Console.ReadLine();
        }
    }
}
