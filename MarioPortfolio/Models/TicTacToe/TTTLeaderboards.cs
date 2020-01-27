using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MarioPortfolio.Models.TicTacToe
{
    public class TTTLeaderboards
    {
        [Key]
        public Guid UserId { get; set; }
        public int WinCountEasy { get; set; }
        public int GameCountEasy { get; set; }
        public int WinCount { get; set; }
        public int GameCount { get; set; }
    }
}