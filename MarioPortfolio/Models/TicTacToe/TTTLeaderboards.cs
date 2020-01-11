using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarioPortfolio.Models.TicTacToe
{
    public class TTTLeaderboards
    {
        public Guid Id { get; set; }
        public int WinCount { get; set; }
        public decimal WinRate { get; set; }
    }
}
