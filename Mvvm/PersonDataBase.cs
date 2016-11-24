using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm
{
    public class PersonDataBase: Person
    {
        public PersonDataBase(Person p, bool IsHead = false,int? PrevID = null, int? NextID = null) : base(p)
        {
            this.IsHead = IsHead;
            this.NextID = NextID;
            this.PrevID = PrevID;
        }
        public PersonDataBase() : base(new Person("",new DateTime(),0))
        { 
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
    }
}
