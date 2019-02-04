using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic {
    interface IUserController {
        void CreateUser(User user);
        User GetUser(int id);
        List<User> GetAllUsers();
        void DeleteUser(User user);
        void UpdateUser(User user);
        void UpdateAccount(User user);
        bool AddBetToUser(User user, Bet bet);
        List<Bet> GetBets(User user);
        List<User> GetBetsOnUserOnMatch(Match match);


    }
}
