using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAAAM_WebClient.WCFService {
    public partial class Match {
        private string mapNames;

        public string MapNames {
            get {
                string result ="";
                foreach (Map m in Maps) {
                    result += m.Name + " ";
                }
                return result; }
            set { mapNames = value; }
        }

        public void GenerateName(Team team1, Team team2) {
            Name = $"{team1.Name} vs. {team2.Name}";
        }

        public override string ToString() {
            return $"{Name}";
        }
    }
}
