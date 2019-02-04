using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces {
    public interface ITeamController {

        int CreateTeam(Team team);
        Team GetTeam(int id);
        void EditTeam(Team team);
        void DeleteTeam(Team team);
        IEnumerable<Team> GetTeams();
    }
}
