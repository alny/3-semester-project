using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    [KnownType(typeof(Match))]
    [KnownType(typeof(Event))]
    [KnownType(typeof(Player))]
    [KnownType(typeof(Team))]
    public abstract class Betable {

    }
}
