using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Repository: IRepository<PersonDataBase>
    {
        private PersonContext db;
        public Repository()
        {
            db = new PersonContext();
        }

        public IEnumerable<PersonDataBase> GetPersonList()
        {
            var curr = (from p in db.Persons
                       where p.IsHead == true
                       select p).FirstOrDefault();
            do
            {
                yield return curr;
                curr = (from p in db.Persons
                        where p.PrevID == curr.PersonID
                        select p).FirstOrDefault();
            } while (curr!=null);
            //return db.Persons;
        }
        public void add(PersonDataBase newPerson)
        {
            if (db.Persons.Count() == 0)
            {
                db.Persons.Add(new PersonDataBase()
                {
                    LastName = newPerson.LastName,
                    DateOfBirth = newPerson.DateOfBirth,
                    Height = newPerson.Height,
                    IsHead = true,
                    NextID = null,
                    PrevID = null
                });
                db.SaveChanges();
            }
            else
            {
                var LastPerson = (from person in db.Persons
                                  where person.NextID == null
                                  select person).First();
                db.Persons.Add(new PersonDataBase()
                {
                    LastName = newPerson.LastName,
                    DateOfBirth = newPerson.DateOfBirth,
                    Height = newPerson.Height,
                    IsHead = false,
                    NextID = null,
                    PrevID = LastPerson.PersonID
                });
                db.SaveChanges();
                var NowAddedPersonID = (from person in db.Persons
                                        where person.PrevID == LastPerson.PersonID
                                        select person).First();
                LastPerson.NextID = NowAddedPersonID.PersonID;
                db.SaveChanges();
            }
        }

        public void addByIndex(PersonDataBase newPerson, int Index)
        {
            if (Index == 1)
            {
                var head = (from p in db.Persons
                            where p.IsHead == true
                            select p).FirstOrDefault();
                db.Persons.Add(new PersonDataBase()
                {
                    LastName = newPerson.LastName,
                    DateOfBirth = newPerson.DateOfBirth,
                    Height = newPerson.Height,
                    IsHead = true,
                    NextID = head.PersonID,
                    PrevID = null
                });
                db.SaveChanges();
                var NowAddedPersonID = (from person in db.Persons
                                        where person.NextID == head.PersonID
                                        select person.PersonID).First();
                head.PrevID = NowAddedPersonID;
                head.IsHead = false;
                db.SaveChanges();
            }
            else
            {
                var curr = (from p in db.Persons
                            where p.IsHead == true
                            select p).FirstOrDefault();
                for (int i = 1; i < Index; ++i)
                {
                    curr = (from p in db.Persons
                            where p.PrevID == curr.PersonID
                            select p).FirstOrDefault();
                }
                var prev = (from p in db.Persons
                            where p.NextID == curr.PersonID
                            select p).FirstOrDefault();
                db.Persons.Add(new PersonDataBase()
                {
                    LastName = newPerson.LastName,
                    DateOfBirth = newPerson.DateOfBirth,
                    Height = newPerson.Height,
                    IsHead = true,
                    NextID = curr.PersonID,
                    PrevID = prev.PersonID
                });
                db.SaveChanges();
                var NowAddedPersonID = (from person in db.Persons
                                        where person.NextID == curr.PersonID && person.PrevID == prev.PersonID
                                        select person.PersonID).First();
                curr.PrevID = NowAddedPersonID;
                prev.NextID = NowAddedPersonID;
                db.SaveChanges();
            }
        }

        public void deleteByIndex(PersonDataBase item, int Index)
        {
            bool flag = Index == 1;
            PersonDataBase delp = (from p in db.Persons
                                   where p.IsHead == true
                                   select p).FirstOrDefault();
            --Index;
            while (Index > 0)
            {
                delp = (from p in db.Persons
                        where p.PrevID == delp.PersonID
                        select p).FirstOrDefault();
                --Index;
            }
            var next = (from p in db.Persons
                        where p.PrevID == delp.PersonID
                        select p).FirstOrDefault();
            var prev = (from p in db.Persons
                        where p.NextID == delp.PersonID
                        select p).FirstOrDefault();
            if (next != null)
                next.PrevID = prev != null ? (int?)prev.PersonID : null;
            if (prev != null)
                prev.NextID = next != null ? (int?)next.PersonID : null;
            if (flag)
                if (next != null)
                    next.IsHead = true;
            db.Persons.Remove(delp);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public PersonDataBase GetPerson(int ID)
        {
            return db.Persons.Find(ID);
        }
    }
}
