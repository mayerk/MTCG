using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL.Managers {
    public interface IGameManager {
        GameResult Start(User player1, User player2);
    }
}
