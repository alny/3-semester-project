using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Round {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string WinnerSide { get; set; }
        [DataMember]
        public int TeamOneScore { get; set; }
        [DataMember]
        public int TeamTwoScore { get; set; }
        [DataMember]
        public Team Winner { get; set; }
        [DataMember]
        public Match Match { get; set; }
        [DataMember]
        public List<Kill> Kills { get; set; } = new List<Kill>();

    }
}
