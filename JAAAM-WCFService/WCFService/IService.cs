using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFService {
    [ServiceContract]
    public interface IService {

        // Player OperationContracts
        [OperationContract]
        int CreatePlayer(Player player);
        [OperationContract]
        Player GetPlayer(int id);
        [OperationContract]
        void DeletePlayer(Player player);
        [OperationContract]
        void EditPlayer(Player player);
        [OperationContract]
        IEnumerable<Player> GetPlayers();
        [OperationContract]
        int CreateMap(Map map);
        // Match OperationsContact
        [OperationContract]
        int CreateMatch(Match match);
        [OperationContract]
        Match GetMatch(int id);
        [OperationContract]
        void EditMatch(Match match);
        [OperationContract]
        void DeleteMatch(Match match);
        [OperationContract]
        IEnumerable<Match> GetMatches();
        [OperationContract]
        IEnumerable<Map> GetMaps();
        [OperationContract]
        void EditMap(Map map);

        // Event OperationsContact
        [OperationContract]
        int CreateEvent(Event eventObject);
        [OperationContract]
        Event GetEvent(int id);
        [OperationContract]
        void EditEvent(Event eventObject);
        [OperationContract]
        void DeleteEvent(Event eventToDelete);
        [OperationContract]
        IEnumerable<Event> GetEvents();

        // Team OperationsContact
        [OperationContract]
        int CreateTeam(Team team);
        [OperationContract]
        Team GetTeam(int id);
        [OperationContract]
        void EditTeam(Team team);
        [OperationContract]
        void DeleteTeam(Team team);
        [OperationContract]
        IEnumerable<Team> GetTeams();

        //User OperationsContract
        [OperationContract]
        void CreateUser(User user);
        [OperationContract]
        User GetUser(int id);
        [OperationContract]
        List<User> GetAllUsers();
        [OperationContract]
        void DeleteUser(User user);
        [OperationContract]
        void UpdateUser(User user);
        [OperationContract]
        void UpdateAccount(User user);
        [OperationContract]
        bool AddBetToUser(User user, Bet bet);
        [OperationContract]
        List<Bet> GetBets(User user);
        [OperationContract]
        List<User> GetBetsOnUserOnMatch(Match match);
        [OperationContract]
        void FakeMethodeToMakeKillOddsRoundAvalibleThroughService(Kill kill, Odds odds, Round round);
    }
}
