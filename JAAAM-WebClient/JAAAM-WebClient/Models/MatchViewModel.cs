using JAAAM_WebClient.WCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JAAAM_WebClient.Models {
    public class MatchViewModel {
        public List<Match> Matches { get; set; }
        public User User { get; set; }
    }
}