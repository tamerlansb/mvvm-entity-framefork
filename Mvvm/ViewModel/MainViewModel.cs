using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubledLinkedList;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows;
using DataLayer;

namespace Mvvm
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private DoubledLinkedList<Person> people;
        private Person newPerson, currPerson;
        IRepository<PersonDataBase> db;
        #endregion

        #region Constructor
        public MainViewModel()
        {
            people = new DoubledLinkedList<Person>();
            db = new Repository();
            /* newPerson = new Person("Sidorov", new DateTime(1996, 01, 03), 179); this.AddPerson();
             newPerson = new Person("Ivanov", new DateTime(1995, 03, 03), 170); this.AddPerson();
             newPerson = new Person("Petrov", new DateTime(1996, 02, 03), 172); this.AddPerson();
             newPerson = new Person("Sadovnichyi", new DateTime(1996, 12, 12), 178); this.AddPerson();*/
            Download();
            newPerson = new Person("aaa", new DateTime(2001,01,01), 0);
        }
        #endregion

        #region Properties
        public DoubledLinkedList<Person> People
        {
            get { return people; }
            private set { people = value;  }
        }
        public Person NewPerson
        {
            get { return newPerson; }
            set {
                newPerson = value;
                NotifityPropetyChanged("NewPerson");
            }
        }
        public Person CurrentPerson
        {
            get { return currPerson; }
            set
            {
                currPerson = value;
                NotifityPropetyChanged("CurrentPerson");
            }
        }
        public uint IndexForAdd
        {
            get; set;
        }
        #endregion

        #region Commands
     
        #region ClearList
        private DelegateCommand clearListCom;
        public ICommand ClearListCom
        {
            get
            {
                if (clearListCom == null)
                {
                    clearListCom = new DelegateCommand(ClearListExecute, ClearListCanExecute);
                }
                return clearListCom;
            }
        }
        public void ClearListExecute()
        {
            people.ClearList();
            var conn = new SqlConnection("server = SMSK01DB09\\DEV; " + "Trusted_Connection=yes;" + "database=TrainingDatabase; ");
            try
            {
                conn.Open();
                var cmd = new SqlCommand("TRUNCATE TABLE PersonDataBases", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool ClearListCanExecute()
        {
            return people.Count > 0;
        }
        #endregion

        #region Sort By LastName
        private DelegateCommand sortByLastName;
        public ICommand SortByLastName
        {
            get
            {
                if (sortByLastName == null)
                {
                    sortByLastName = new DelegateCommand(SortExecute, SortCanExecute);
                }
                return sortByLastName;
            }
        } 
        public void SortExecute()
        {
            people.SortByPred((Person x, Person y) => { return x.LastName.CompareTo(y.LastName); });
        }
        public bool SortCanExecute()
        {
            return people.Count > 0;
        }
        #endregion

        #region Delete
        private DelegateCommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new DelegateCommand(DeletePerson,CanExecuteDelete);
                }
                return deleteCommand;
            }
        }
        private void DeletePerson()
        {
            if (CurrentPerson != null)
            {
                int index  = people.RemoveByPredicate((Person x) => { return x == CurrentPerson; });
                db.deleteByIndex(new PersonDataBase()
                {
                    LastName = currPerson.LastName,
                    DateOfBirth = currPerson.DateOfBirth,
                    Height = currPerson.Height,
                }, index);
            }
            CurrentPerson = null;
        }
        private bool CanExecuteDelete()
        {
            return people.Count > 0 && CurrentPerson != null;
        }
        #endregion

        #region Add By Index
        private DelegateCommand addCommandByIndex;
        public ICommand AddCommandByIndex
        {
            get
            {
                if (addCommandByIndex == null)
                {
                    addCommandByIndex = new DelegateCommand(AddPersonByIndex, CanExecuteAddByIndex);
                }
                return addCommandByIndex;
            }
        }
        private void AddPersonByIndex()
        {
            people.AddByIndex(newPerson,IndexForAdd);
            db.addByIndex(new PersonDataBase()
            {
                LastName = newPerson.LastName,
                DateOfBirth = newPerson.DateOfBirth,
                Height = newPerson.Height,
            }, (int)IndexForAdd);
            NewPerson = new Person("", new DateTime(2001,01,01), 0);
        }
        private bool CanExecuteAddByIndex()
        {
            return CanExecuteAdd() && IndexForAdd > 0 && IndexForAdd <= people.Count;
        }
        #endregion

        #region Add
        private DelegateCommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new DelegateCommand(AddPerson,CanExecuteAdd);
                }
                return addCommand;
            }
        }
        private void AddPerson()
        {
            people.Add(newPerson);
            db.add(new PersonDataBase()
            {
                LastName = newPerson.LastName,
                DateOfBirth = newPerson.DateOfBirth,
                Height = newPerson.Height,
            });
            NewPerson = new Person("", new DateTime(2001, 01, 01), 0);    
        }
        private bool CanExecuteAdd()
        {
            bool flag;
            flag = newPerson.DateOfBirth <= DateTime.Today && newPerson.LastName != "" && newPerson.LastName.Length <= 25 && NewPerson.Height > 0 && newPerson.Height < 300;
            return flag;
        }
        #endregion

        #region
        private void Download()
        {
            foreach (var p in db.GetPersonList())
                people.Add(new Person() {
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Height = p.Height
                });
        }
        #endregion
        #endregion
    }
}
