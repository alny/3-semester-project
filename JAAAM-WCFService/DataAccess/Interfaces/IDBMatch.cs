using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces {
    public interface IDBMatch {
        int CreateMatch(Match match);
        Match GetMatch(int id);
        void EditMatch(Match match);
        void DeleteMatch(Match match);
        IEnumerable<Match> GetMatches();
        List<Match> GetMatchesByEvent(Event eventt);
        IEnumerable<Map> GetMaps();
        void EditMap(Map map);
    }
}
