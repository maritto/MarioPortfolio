using MarioPortfolio.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MarioPortfolio.Models.TicTacToe
{
    public class TicTacToeMatch
    {
        [Key]
        public Guid GameId { get; set; }
        [ForeignKey("TTTLeadearboards")]
        public Guid UserId { get; set; }
        public string Moves { get; set; }
        public bool Easy { get; set; }
    }
}
