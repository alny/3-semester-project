using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Player : Betable {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public string Age { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public int TeamId { get; set; }
        /// <summary>
        /// Override of ToString in order to get information needed.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            return Id.ToString() + NickName.ToString() + Age.ToString() + Role.ToString() + TeamId.ToString() ;
        }
    }
    
}
