using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestGitProject
{
    class DataClass
    {
        public int current_id = 0;
        public List<CancellationToken> tokens = new List<CancellationToken>();
        public List<CancellationTokenSource>  canceled_tokens = new List<CancellationTokenSource>();
        public int count;
        public DataClass(int count)
        {
            this.count = count;
            for (int ind = 0; ind < count; ind += 1)
            {
                canceled_tokens[ind] = new CancellationTokenSource();
                tokens[ind] = canceled_tokens[ind].Token;
            }
        }
}
    abstract class Imitations
    {
        public string name;
        private bool making_error(ref DataClass data)  // true - ошибка, false - нет ошибки
        {
            var rnd = new Random();
            int number = Convert.ToInt32(rnd.NextDouble() * 100);
            if (data.current_id == 0 || data.current_id == data.count)  
                return false;
            if (number > 20)
            {
                data.canceled_tokens[data.current_id].Cancel();  // отмена токена ввиду появления ошибки
                return false;
            }
            return true;
        }

        public int making_imitation(ref DataClass data)  // 1 - good, 0 - error
        {
            Console.WriteLine($"Start working.");
            var result = this.making_error(ref data);
            if (data.tokens[data.current_id].IsCancellationRequested)
                data.tokens[data.current_id].ThrowIfCancellationRequested();
            data.current_id += 1;
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
