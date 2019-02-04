using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Account {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Balance { get; set; }
    }
}
