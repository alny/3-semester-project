using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model {
    [DataContract]
    public class Match : Betable {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Format { get; set; }
        [DataMember]
        public List<Map> Maps { get; set; }
        [DataMember]
        public List<Team> Teams { get; set; }
        [DataMember]
        public Team Winner { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public bool Held { get; set; }
        public Match() {
            Maps = new List<Map>();
            Teams = new List<Team>();
        }
        /// <summary>
        /// Method to generate Name based on the teams on the match.
        /// </summary>
        /// <param name="team1"></param>
        /// <param name="team2"></param>
        public void GenerateName(Team team1, Team team2) {
            Name = $"{team1.Name} vs. {team2.Name}";
        }
    }

}
