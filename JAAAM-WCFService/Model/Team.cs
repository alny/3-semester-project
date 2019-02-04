using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Team : Betable {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Player> Players { get; set; }
        public Team() {
            Players = new List<Player>();
        }
        /// <summary>
        /// Override of ToString in order to get information needed.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            return Id.ToString() + Name.ToString();
        }
    }
}
