using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces {
    public interface IMatchController {
        int CreateMatch(Match match);
        Match GetMatch(int id);
        void EditMatch(Match match);
        void DeleteMatch(Match match);
        IEnumerable<Match> GetMatches();

        int CreateMap(Map map);
        IEnumerable<Map> GetMaps();
        void EditMap(Map map);
        void DeleteMap(Map map);
    }
}
