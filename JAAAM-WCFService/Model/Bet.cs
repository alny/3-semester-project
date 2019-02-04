using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    [DataContract]
    public class Bet {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Odds { get; set; }
        [DataMember]
        public Betable Type { get; set; }
        [DataMember]
        public Team WinCondition { get; set; }
        [DataMember]
        public bool Verified { get; set; }
        public override string ToString() {
            return Id.ToString() + Amount.ToString() + Odds.ToString() + Type.GetType().Name;
        }
        /// <summary>
        /// Overide of equals to compare bet objects.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        public override bool Equals(object obj) {
            bool toReturn = false;
            if ((decimal.Compare(Amount, (obj as Bet).Amount)) == 0) {
                if ((decimal.Compare(Odds, (obj as Bet).Odds)) == 0) {
                    if (Id.Equals((obj as Bet).Id)) {
                        toReturn = true;
                    }
                }
            }
            return toReturn;
        }
    }
}
