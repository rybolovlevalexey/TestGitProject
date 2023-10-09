using System;
using System.Collections.Generic;
using System.Text;

namespace TestGitProject
{
    abstract class Imitations
    {
        public string name;
        public bool making_error()  // true - ошибка, false - нет ошибки
        {
            var rnd = new Random();
            int num = Convert.ToInt32(rnd.NextDouble() * 100);
            return num <= 50;
        }

        public int making_imitation()  // 1 - good, 0 - error
        {
            Console.WriteLine($"Start working.");
            if (this.making_error())
                return 0;
            Console.WriteLine($"Work done.");
            return 1;
        }
        public string get_name() { return name; }
    }

    class ShowSplashImitation : Imitations { string name = "Show Splash"; }
    class RequestLicenseImitation : Imitations { string name = "Request License"; }
    class SetupMenusImitation : Imitations { string name = "Setup Menus"; }
    class CheckForUpdateImitation : Imitations { string name = "Check For Update"; }
    class DownloadUpdateImitation : Imitations { string name = "Download Update"; }
    class DisplayWelcomeScreenImitation : Imitations { string name = "Display Welcome Screen"; }
    class HideSplashImitation : Imitations { string name = "Hide Splash"; }
}
