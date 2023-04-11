using SpecFlowProject1.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject1.Drivers
{
    public class PageInitializer
    {
        public static LoginPage loginPage=null!;
        public static HomePage homePage = null!;
        public static void Initialize()
        {
            loginPage = new LoginPage();
            homePage = new HomePage();

        }
    }
}
