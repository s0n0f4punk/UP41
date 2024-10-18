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
                DateTime date = (DateTime)BirthDate;
                return (int)((DateTime.Now - date).TotalDays / 365.2425);
            } 
        }
        public string Tasks { get
            {
                var taskNames = App.db.User_Tasks
                    .Where(x => x.Id_User == Id)
                    .Select(ut => ut.PerformTasks.Title)
                    .ToList();

                return string.Join(", ", taskNames);
            }
        }
    }
}
