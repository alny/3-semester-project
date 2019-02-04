using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JAAAM_WebClient.Models {
    public class BetData {
        public string Odds { get; set; }
        public int TeamId { get; set; }
        public int MatchId { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; }
    }
}