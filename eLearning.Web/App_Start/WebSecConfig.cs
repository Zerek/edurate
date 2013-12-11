using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace edurate.Web
{
    public class WebSecConfig
    {
        public static void RegisterWebSec()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                "Users", "UserId", "Email", autoCreateTables: true);
        }
    }
}