using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Kill {
        [DataMember]
        public string KillerName { get; set; }
        [DataMember]
        public string KilledName { get; set; }
    }
}