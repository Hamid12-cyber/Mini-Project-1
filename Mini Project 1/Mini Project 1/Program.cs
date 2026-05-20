using Mini_Project_1.Models;
using Mini_Project_1.Services;
using System.Text;

namespace Mini_Project_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
                new System.Globalization.CultureInfo("az-Latn-AZ");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture =
                new System.Globalization.CultureInfo("az-Latn-AZ");
            Minishophm minishophm = new Minishophm();
            minishophm.Run();
        }
    }
}