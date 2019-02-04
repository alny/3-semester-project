using BusinessLogic.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest {
    class Program {
        static void Main(string[] args) {

            UserController userController = new UserController();
            foreach (var user in userController.GetAllUsers()) {
                Console.WriteLine(user.FirstName);
                Console.WriteLine(user.Bets.Count);
                foreach (var bet in user.Bets) {
                    Console.WriteLine(bet.Type);
                }
            }
            Console.ReadKey();

        }
    }
}
