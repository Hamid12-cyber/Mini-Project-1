using Mini_Project_1.Models;
using Mini_Project_1.Services;

namespace Mini_Project_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Minishophm minishophm = new Minishophm();
            minishophm.Run();
        }
    }
}
