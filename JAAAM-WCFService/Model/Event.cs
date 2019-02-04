using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Event : Betable {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string GameName { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public bool Held { get; set; }
        [DataMember]
        public List<Match> Matches { get; set; }
        public Event() {
            Matches = new List<Match>();
        }
    }
}
