using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace SystemCheckPoint.AppData
{
    public class EmployeeAndPerson
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronumic { get; set; }
        public string Bithday { get; set; }
        public int NumberPass { get; set; }
        public DateTime DateOfFormation { get; set; }
        public string Type { get; set; }
    }
}
