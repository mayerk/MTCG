using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL {
    public class GameResult {
        public User? Winner { get; set; }
        public string Log { get; set; } = "";

        public GameResult(User? winner, string log) {
            Winner = winner;
            Log = log;
        }
    }
}
