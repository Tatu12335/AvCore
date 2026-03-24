using AvCore.Application.Interfaces;

namespace AVcli.Graphics.UserPanel
{
    public class UserPanel
    {
        private readonly IFileScanner _fileScanner;
        public UserPanel(IFileScanner fileScanner)
        {
            _fileScanner = fileScanner;
        }
        public async Task Welcome()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var welcome = @"__        __   _                          
\ \      / /__| | ___ ___  _ __ ___   ___ 
 \ \ /\ / / _ \ |/ __/ _ \| '_ ` _ \ / _ \
  \ V  V /  __/ | (_| (_) | | | | | |  __/
   \_/\_/ \___|_|\___\___/|_| |_| |_|\___|";

            Console.WriteLine(welcome);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Enter file or a directory to scan");
            Console.Write(">");
            var filepath = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;

            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Error : filepath cant be empty");
                return;
            }

            filepath = filepath.Replace('"', ' ').Trim();

          
            var response = await _fileScanner.ScanFileAsync(filepath);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(response);



        }
    }
}
