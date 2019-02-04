using Model;
using DataAccess;
using BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Controllers {
    public class TeamController : ITeamController {

        private DBTeam _dBTeam;
        /// <summary>
        /// constructor to instantiate _dbTeam
        /// </summary>
        public TeamController() {
            _dBTeam = new DBTeam();
        }
        /// <summary>
        /// persists Team
        /// </summary>
        /// <param name="team"></param>
        /// <returns>TeamId given to persisted team</returns>
        public int CreateTeam(Team team) {
            return _dBTeam.CreateTeam(team);
        }
        /// <summary>
        /// Deletes persisted team
        /// </summary>
        /// <param name="team"></param>
        public void DeleteTeam(Team team) {
            _dBTeam.DeleteTeam(team);
        }
        /// <summary>
        /// Updates information about team given by parameters
        /// </summary>
        /// <param name="team"></param>
        public void EditTeam(Team team) {
            _dBTeam.EditTeam(team);
        }
        /// <summary>
        /// Gets team by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Team</returns>
        public Team GetTeam(int id) {
            return _dBTeam.GetTeam(id);
        }
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <returns>List of teams</returns>
        public IEnumerable<Team> GetTeams() {
            return _dBTeam.GetTeams();
        }
    }
}
