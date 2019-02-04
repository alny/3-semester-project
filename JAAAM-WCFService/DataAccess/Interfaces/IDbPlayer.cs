using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces {
    public interface IDBPlayer {

        int CreatePlayer(Player player);
        Player GetPlayer(int id);
        void EditPlayer(Player player);
        void DeletePlayer(Player player);
        IEnumerable<Player> GetPlayers();
        List<Player> GetPlayersByTeam(Team team);

    }
}
