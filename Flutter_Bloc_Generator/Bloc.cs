using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter_Bloc_Generator
{
    public class Bloc
    {
        public string BlocName { get; set; }
        public string EventName { get; set; }
        public string StateName { get; set; }
        public string FileName { get; set; }
        public List<String> EventsNames { get; set; }
        public List<String> StatesName { get; set; }
    }

    public class GeneratedBlocString
    {
        public string EventString { get; set; }
        public string StateString { get; set; }
        public string BlocString { get; set; }
    }
}
