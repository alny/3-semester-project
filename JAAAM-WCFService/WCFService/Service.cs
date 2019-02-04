using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Controllers;
using Model;

namespace WCFService {
    public class Service : IService {

        private PlayerController playerController;
        private TeamController teamController;
        private EventController eventController;
        private MatchController matchController;
        private UserController userController;
        /// <summary>
        /// Constructor to instantiate Controllers
        /// </summary>
        public Service() {
            playerController = new PlayerController();
            teamController = new TeamController();
            eventController = new EventController();
            matchController = new MatchController();
            userController = new UserController();
        }

        // Player Operationcontracts Implemented

        /// <summary>
        /// Persists Player object
        /// </summary>
        /// <param name="player"></param>
        /// <returns>PlayerId given to persisted player</returns>
        public int CreatePlayer(Player player) {
            return playerController.CreatePlayer(player);
        }
        /// <summary>
        /// Gets player by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found Player</returns>
        public Player GetPlayer(int id) {
            return playerController.GetPlayer(id);
        }
        /// <summary>
        /// Updates information about persisted player
        /// </summary>
        /// <param name="player"></param>
        public void EditPlayer(Player player) {
            playerController.EditPlayer(player);
        }
        /// <summary>
        /// Deletes persisted player
        /// </summary>
        /// <param name="player"></param>
        public void DeletePlayer(Player player) {
            playerController.DeletePlayer(player);
        }
        /// <summary>
        /// Gets all players
        /// </summary>
        /// <returns>List of players</returns>
        public IEnumerable<Player> GetPlayers() {
            return playerController.GetPlayers();
        }

        // Match Operationcontracts Implemented

        /// <summary>
        /// Persists match object
        /// </summary>
        /// <param name="match"></param>
        /// <returns>MatchId given to persisted match</returns>
        public int CreateMatch(Match match) {
            return matchController.CreateMatch(match);
        }
        /// <summary>
        /// Gets match by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>match</returns>
        public Match GetMatch(int id) {
            return matchController.GetMatch(id);
        }
        /// <summary>
        /// Updates information about persisted match
        /// </summary>
        /// <param name="match"></param>
        public void EditMatch(Match match) {
            matchController.EditMatch(match);
        }
        /// <summary>
        /// Deletes persisted match
        /// </summary>
        /// <param name="match"></param>
        public void DeleteMatch(Match match) {
            matchController.DeleteMatch(match);
        }
        /// <summary>
        /// Gets all matches
        /// </summary>
        /// <returns>List of matches</returns>
        public IEnumerable<Match> GetMatches() {
            return matchController.GetMatches();
        }
        /// <summary>
        /// Gets all map
        /// </summary>
        /// <returns>List of Matches</returns>
        public IEnumerable<Map> GetMaps() {
            return matchController.GetMaps();
        }
        /// <summary>
        /// Updates information about persisted Map
        /// </summary>
        /// <param name="map"></param>
        public void EditMap(Map map) {
            matchController.EditMap(map);
        }

        // Team Operationcontracts Implemented

        /// <summary>
        /// Persists team
        /// </summary>
        /// <param name="team"></param>
        /// <returns>TeamId given to persisted team</returns>
        public int CreateTeam(Team team) {
            return teamController.CreateTeam(team);
        }
        /// <summary>
        /// Gets team by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found team</returns>
        public Team GetTeam(int id) {
            return teamController.GetTeam(id);
        }
        /// <summary>
        /// Updates information about team
        /// </summary>
        /// <param name="team"></param>
        public void EditTeam(Team team) {
            teamController.EditTeam(team);
        }
        /// <summary>
        /// Deletes persisted team
        /// </summary>
        /// <param name="team"></param>
        public void DeleteTeam(Team team) {
            teamController.DeleteTeam(team);
        }
        /// <summary>
        /// Gets all teams
        /// </summary>
        /// <returns>List of Teams</returns>
        public IEnumerable<Team> GetTeams() {
            return teamController.GetTeams();
        }

        // Event Operationcontracts Implemented

        /// <summary>
        /// Persists Event object
        /// </summary>
        /// <param name="eventObject"></param>
        /// <returns>EventId given to persisted event</returns>
        public int CreateEvent(Event eventObject) {
            return eventController.CreateEvent(eventObject);
        }
        /// <summary>
        /// Get event by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found event</returns>
        public Event GetEvent(int id) {
            return eventController.GetEvent(id);
        }
        /// <summary>
        /// Updates information about persisted event
        /// </summary>
        /// <param name="eventObject"></param>
        public void EditEvent(Event eventObject) {
            eventController.EditEvent(eventObject);
        }
        /// <summary>
        /// Deletes persisted event
        /// </summary>
        /// <param name="eventToDelete"></param>
        public void DeleteEvent(Event eventToDelete) {
            eventController.DeleteEvent(eventToDelete);
        }
        /// <summary>
        /// Gets all events
        /// </summary>
        /// <returns>List of events</returns>
        public IEnumerable<Event> GetEvents() {
            return eventController.GetEvents();
        }
        /// <summary>
        /// Persists Map object
        /// </summary>
        /// <param name="map"></param>
        /// <returns>MapId given to persisted map</returns>
        public int CreateMap(Map map) {
            return matchController.CreateMap(map);
        }
        // User OperationsContracts Implemented

        /// <summary>
        /// Persists User Object
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user) {
            userController.CreateUser(user);
        }
        /// <summary>
        /// Get user by unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(int id) {
            return userController.GetUser(id);
        }
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers() {
            return userController.GetAllUsers();
        }
        /// <summary>
        /// Deletes persisted user
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user) {
            userController.DeleteUser(user);
        }
        /// <summary>
        /// Updates information about persisted user
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user) {
            userController.UpdateUser(user);
        }
        /// <summary>
        /// Updates Account.Balance on Persisted user
        /// </summary>
        /// <param name="user"></param>
        public void UpdateAccount(User user) {
            userController.UpdateAccount(user);
        }
        /// <summary>
        /// Persists Bet made by a user. Returns true if transaction succeeds.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bet"></param>
        /// <returns>bool</returns>
        public bool AddBetToUser(User user, Bet bet) {
            return userController.AddBetToUser(user, bet);
        }
        /// <summary>
        /// Gets all bets made by a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List of bets</returns>
        public List<Bet> GetBets(User user) {
            return userController.GetBets(user);
        }

        /// <summary>
        /// Gets all users with won bets for a given match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>List of Users</returns>
        public List<User> GetBetsOnUserOnMatch(Match match) {
            return userController.GetBetsOnUserOnMatch(match);
        }
        /// <summary>
        /// Method used to serialize Kill, Odds and Round. Do not use!
        /// </summary>
        /// <param name="kill"></param>
        /// <param name="odds"></param>
        /// <param name="round"></param>
        public void FakeMethodeToMakeKillOddsRoundAvalibleThroughService(Kill kill, Odds odds, Round round) {
            throw new NotImplementedException();
        }
    }
}
