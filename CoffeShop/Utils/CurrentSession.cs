using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Model;

namespace CoffeeShop.Utils
{
    internal static class CurrentSession
    {
        public static UserInfo? CurrentUser { get; private set; }

        public static bool IsLoggedIn
        {
            get
            {
                return !(CurrentUser == null);
            }
        }
        public static void StartSession(UserInfo user)
        {
            if (CurrentUser == null)
            {
                CurrentUser = user;
            }
        }

        public static void ClearSession()
        {
            CurrentUser = null;
        }
    }
}
