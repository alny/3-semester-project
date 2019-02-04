using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces {
    public interface IDBTeam {

        int CreateTeam(Team team);
        Team GetTeam(int id);
        void EditTeam(Team team);
        void DeleteTeam(Team team);
        IEnumerable<Team> GetTeams();
        List<Team> GetTeamsByMatch(Match match);
    }
}
