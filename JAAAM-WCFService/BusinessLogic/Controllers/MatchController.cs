using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Controllers {
    public class MatchController : IMatchController {

        private DBMatch _dbMatch;
        /// <summary>
        /// Constructor to instantiate _dbMatch.
        /// </summary>
        public MatchController() {
            _dbMatch = new DBMatch();
        }
        /// <summary>
        /// Persists map.
        /// </summary>
        /// <param name="map"></param>
        /// <returns>MapId given to persisted map</returns>
        public int CreateMap(Map map) {
            return _dbMatch.CreateMap(map);
        }
        /// <summary>
        /// Creates and persists match.
        /// </summary>
        /// <param name="match"></param>
        /// <returns>MatchId given to persisted match</returns>
        public int CreateMatch(Match match) {
            return _dbMatch.CreateMatch(match);
        }
        /// <summary>
        /// Deletes persisted match.
        /// </summary>
        /// <param name="match"></param>
        public void DeleteMatch(Match match) {
            _dbMatch.DeleteMatch(match);
        }
        /// <summary>
        /// Updates information about persisted match.
        /// </summary>
        /// <param name="match"></param>
        public void EditMatch(Match match) {
            _dbMatch.EditMatch(match);
        }
        /// <summary>
        /// Gets a Match by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found Match</returns>
        public Match GetMatch(int id) {
            return _dbMatch.GetMatch(id);
        }
        /// <summary>
        /// Updates information about persisted map
        /// </summary>
        /// <param name="map"></param>
        public void EditMap(Map map) {
            _dbMatch.EditMap(map);
        }
        /// <summary>
        /// Gets all persisted matches
        /// </summary>
        /// <returns>List of matches</returns>
        public IEnumerable<Match> GetMatches() {
            return _dbMatch.GetMatches();
        }
        /// <summary>
        /// Gets all persisted maps
        /// </summary>
        /// <returns>List of maps</returns>
        public IEnumerable<Map> GetMaps() {
            return _dbMatch.GetMaps();
        }
        /// <summary>
        /// Deletes persisted map
        /// </summary>
        /// <param name="map"></param>
        public void DeleteMap(Map map) {
            _dbMatch.DeleteMap(map);
        }
    }
}
