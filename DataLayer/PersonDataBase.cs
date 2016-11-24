using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PersonDataBase
    {
        [Key]
        public int PersonID
        {
            get; set;
        }
        public string LastName
        {
            get; set;
        }
        public DateTime DateOfBirth
        {
            get; set;
        }
        public int Height
        {
            get;  set;
        }
        public bool IsHead
        {
            get; set;
        }
        public Nullable<int> PrevID
        {
            get; set;
        }
        public int? NextID
        {
            get; set;
        }
    }
}
