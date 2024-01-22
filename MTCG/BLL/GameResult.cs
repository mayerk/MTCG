using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    public class GameResult {
        public User? Winner { get; set; }
        public User? Looser { get; set; }
        public string Log { get; set; } = "";

        public GameResult(User? winner, User? looser, string log) {
            Winner = winner;
            Looser = looser;
            Log = log;
        }
    }
}
