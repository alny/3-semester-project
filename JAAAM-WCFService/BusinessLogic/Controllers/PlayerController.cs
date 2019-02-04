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
    public class PlayerController : IPlayerController {

        private DBPlayer _dbPlayer;
        /// <summary>
        /// Constructor to instantiate _dpPlayer
        /// </summary>
        public PlayerController() {
            _dbPlayer = new DBPlayer();
        }
        /// <summary>
        /// Persists player
        /// </summary>
        /// <param name="player"></param>
        /// <returns>PlayerId given to the persisted player</returns>
        public int CreatePlayer(Player player) {
            return _dbPlayer.CreatePlayer(player);
        }
        /// <summary>
        /// Deletes persisted player
        /// </summary>
        /// <param name="player"></param>
        public void DeletePlayer(Player player) {
            _dbPlayer.DeletePlayer(player);
        }
        /// <summary>
        /// Updates information about persisted player.
        /// </summary>
        /// <param name="player"></param>
        public void EditPlayer(Player player) {
            _dbPlayer.EditPlayer(player);
        }
        /// <summary>
        /// Gets a persisted player by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found Player</returns>
        public Player GetPlayer(int id) {
            return _dbPlayer.GetPlayer(id);
        }
        /// <summary>
        /// Gets all players
        /// </summary>
        /// <returns>List of players</returns>
        public IEnumerable<Player> GetPlayers() {
            return _dbPlayer.GetPlayers();
        }
    }
}
