using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject1.Utility
{
    public class ConfigSettings
    {


        public string BrowserType { get; set; }
       

        public Env Env { get; set; }

    }
    public class Env
    {
        public string name { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
