using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVcli.Graphics.UserPanel
{
    public class UserPanel
    {
        public void Welcome()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var welcome = @"__        __   _                          
\ \      / /__| | ___ ___  _ __ ___   ___ 
 \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \
  \ V  V /  __/ | (_| (_) | | | | | |  __/
   \_/\_/ \___|_|\___\___/|_| |_| |_|\___|";

            Console.WriteLine(welcome);
        }
    }
}
