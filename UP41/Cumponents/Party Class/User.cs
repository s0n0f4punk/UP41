using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UP41.Cumponents
{
    public partial class User
    {
        public int Age { get
            {
                if (BirthDate != null)
                {
                    DateTime date = (DateTime)BirthDate;
                    return (int)((DateTime.Now - date).TotalDays / 365.2425);
                }
                else return 0;
            } 
        }
        public string Tasks { get
            {
                var taskNames = App.db.UserTasks
                    .Where(x => x.Login == Login)
                    .Select(ut => ut.PerformTasks.Name)
                    .ToList();

                return string.Join(", ", taskNames);
            }
        }
        public string FIO
        {
            get
            {
                string FIO = Surname + " " + Name + " " + Patronymic;
                return FIO;
            }
        }
    }
}
