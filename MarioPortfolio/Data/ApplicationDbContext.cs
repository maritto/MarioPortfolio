using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MarioPortfolio.Models.TicTacToe;

namespace MarioPortfolio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.TicTacToe.TTTLeaderboards> TTTLeaderboards;

        public DbSet<MarioPortfolio.Models.TicTacToe.TTTLeaderboards> TTTLeaderboards_1 { get; set; }
    }
}
