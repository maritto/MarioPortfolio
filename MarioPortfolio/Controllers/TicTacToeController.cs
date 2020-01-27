using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MarioPortfolio.Data;
using MarioPortfolio.Models.TicTacToe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarioPortfolio.Controllers
{
    public class LeaderBoardsDisplay
    {
        public string Username { get; set; }
        public TTTLeaderboards TTTLeaderboardsEasy { get; set; }
        public TTTLeaderboards TTTLeaderboardsNotSoEasy { get; set; }
    }

    public class TicTacToeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TicTacToeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }



        [Authorize]
        public IActionResult Index()
        {
            List<LeaderBoardsDisplay> leaderBoardsDisplays = new List<LeaderBoardsDisplay>();
            try
            {
                foreach (var item in _context.TTTLeaderboards.OrderByDescending(p => p.WinCountEasy / p.GameCountEasy).Take(10).ToList())
                {
                    LeaderBoardsDisplay boardsDisplayTemp = new LeaderBoardsDisplay();
                    boardsDisplayTemp.TTTLeaderboardsEasy = item;
                    boardsDisplayTemp.Username = _context.Users.Where(p => p.Id == item.UserId.ToString()).First().UserName;
                    leaderBoardsDisplays.Add(boardsDisplayTemp);
                }
            }
            catch
            {
                Console.WriteLine("No Leaderboard users.");
            }
            try
            {
                foreach (var item in _context.TTTLeaderboards.OrderByDescending(p => p.WinCount / p.GameCount).Take(10).ToList())
                {
                    try
                    {
                        leaderBoardsDisplays
                            .Where(p => p.TTTLeaderboardsEasy.UserId == item.UserId)
                            .First()
                            .TTTLeaderboardsNotSoEasy = item;
                    }
                    catch
                    {
                        LeaderBoardsDisplay boardsDisplayTemp = new LeaderBoardsDisplay();
                        boardsDisplayTemp.TTTLeaderboardsNotSoEasy = item;
                        boardsDisplayTemp.Username = _context.Users
                            .Where(p => p.Id == item.UserId.ToString())
                            .First()
                            .UserName;
                        leaderBoardsDisplays.Add(boardsDisplayTemp);
                    }
                }
            }
            catch
            {
                Console.WriteLine("No leaderboards on the notsoeasy mode.");
            }
            return View(leaderBoardsDisplays);
        }

        [Authorize]
        public IActionResult Easy(Guid Id)
        {
            if(!CreateGame(Id, out _, true))
            {
                throw new Exception("Failed to create new game.");
            }
            return View("GameView");
        }

        [Authorize]
        public IActionResult NotSoEasy(Guid Id)
        {
            if (!CreateGame(Id, out _, false))
            {
                throw new Exception("Failed to create new game.");
            }
            return View("GameView");
        }


        [HttpPost]
        [Authorize]
        public int WinCount()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out var tmpUserId);
            try
            {
                return _context.TTTLeaderboards.Where(p => p.UserId == tmpUserId).First().WinCount;
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        [Authorize]
        public int WinCountEasy()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out var tmpUserId);
            try
            {
                return _context.TTTLeaderboards.Where(p => p.UserId == tmpUserId).First().WinCountEasy;
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        [Authorize]
        public int GameCount()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out var tmpUserId);
            try
            {
                return _context.TTTLeaderboards.Where(p => p.UserId == tmpUserId).First().GameCount;
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        [Authorize]
        public int GameCountEasy()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out var tmpUserId);
            try
            {
                return _context.TTTLeaderboards.Where(p => p.UserId == tmpUserId).First().GameCountEasy;
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        [Authorize]
        public float WinRate()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out var tmpUserId);
            try
            {
                var tmpUser = _context.TTTLeaderboards.Where(p => p.UserId == tmpUserId).First();
                return MathF.Round((float)tmpUser.WinCount / (float)tmpUser.GameCount * 100.0f, 2);
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        [Authorize]
        public float WinRateEasy()
        {
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out var tmpUserId);
            try
            {
                var tmpUser = _context.TTTLeaderboards.Where(p => p.UserId == tmpUserId).First();
                return MathF.Round((float)tmpUser.WinCountEasy / (float)tmpUser.GameCountEasy * 100.0f, 2);
            }
            catch
            {
                return 0;
            }
        }

        [BindProperty]
        public string GameId { get; set; }
        [BindProperty]
        public string Coordinates { get; set; }

        [HttpPost]
        [Authorize]
        public string PlayerAction()
        {
            Guid tempGameId = new Guid();
            TicTacToeMatch tmpTTL;
            if (!Guid.TryParse(GameId, out tempGameId))
            {
                return null;
            }
            if (!GameExists(tempGameId, out tmpTTL))
                if (!CreateGame(tempGameId, out tmpTTL))
                    return null;
            if (IsMovePossible(Coordinates, tmpTTL))
            {
                RegisterMove(Coordinates, tmpTTL);
                bool?[,] map = BuildMap(tmpTTL.Moves);
                if (IsWinConditionAchieved(map, out bool? winner))
                {
                    LockGame(tmpTTL, winner);
                    GameEnded(tmpTTL);
                }
                else
                {
                    if (tmpTTL.Moves.Split('-').Count() > 8)
                    {
                        LockGame(tmpTTL, null);
                        GameEnded(tmpTTL);
                    }
                }
                return ComputerMove(map,tmpTTL.Easy);
            }
            return null;
        }

        //TODO:
        private string ComputerMove(bool?[,] map, bool easy)
        {
            return "true";
        }

        private void GameEnded(TicTacToeMatch tmpttl)
        {
            TTTLeaderboards player;
            try
            {
                player = _context.TTTLeaderboards.Where(p => p.UserId == tmpttl.UserId).First();
                if (tmpttl.Easy)
                    player.GameCountEasy += 1;
                else
                    player.GameCount += 1;
                if (tmpttl.Moves.Contains('X'))
                {
                    if (tmpttl.Easy)
                        player.WinCountEasy += 1;
                    else
                        player.WinCount += 1;
                }
                _context.Update(player);
            }
            catch (Exception e)
            {
                player = new TTTLeaderboards();
                if (tmpttl.Moves.Contains('X'))
                {
                    if (tmpttl.Easy)
                        player.WinCountEasy += 1;
                    else
                        player.WinCount += 1;
                }
                if (tmpttl.Easy)
                    player.GameCountEasy += 1;
                else
                    player.GameCount += 1;
                player.UserId = tmpttl.UserId;
                _context.Add(player);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Adds # to the end of the move list simbolizing the game is over.
        /// </summary>
        /// <param name="tmpTTL">Match details</param>
        private void LockGame(TicTacToeMatch tmpTTL, bool? winner)
        {
            if (winner == null)
                tmpTTL.Moves = $"{tmpTTL.Moves}-#";
            else if (winner == true)
                tmpTTL.Moves = $"{tmpTTL.Moves}-#X";
            else
                tmpTTL.Moves = $"{tmpTTL.Moves}-#O";
            _context.TicTacToeMatch.Update(tmpTTL);
            _context.SaveChanges();
        }

        /// <summary>
        /// Verifies if there is 3 of the same in a row
        /// </summary>
        /// <param name="tmpTTL">Match Details</param>
        /// <param name="winner">Returns result for X</param>
        /// <returns>Returns if a win condition was achieved.</returns>
        private bool IsWinConditionAchieved(bool?[,] map, out bool? winner)
        {

            //verify all rows
            for (int x = 0; x < 3; x++)
            {
                if (map[x, 0] == map[x, 1] && map[x, 0] == map[x, 2] && map[x, 2] != null)
                {
                    winner = map[x, 0];
                    return true;
                }
            }
            //verify all collums
            for (int y = 0; y < 3; y++)
            {
                if (map[0, y] == map[1, y] && map[0, y] == map[2, y] && map[2, y] != null)
                {
                    winner = map[0, y];
                    return true;
                }
            }
            //verify diagonals
            if (map[0, 0] == map[1, 1] && map[1, 1] == map[2, 2] && map[1, 1] != null)
            {
                winner = map[1, 1];
                return true;
            }
            if (map[0, 2] == map[1, 1] && map[2, 0] == map[1, 1] && map[1, 1] != null)
            {
                winner = map[1, 1];
                return true;
            }
            //if none confirms its not over yet
            winner = null;
            return false;
        }

        /// <summary>
        /// Builds a game map where X is true and O is false
        /// </summary>
        /// <param name="moves">List of moves in string format</param>
        /// <returns>Map array</returns>
        private bool?[,] BuildMap(string moves)
        {
            bool?[,] tmpMap = new bool?[3, 3];
            var coordinates = moves.Split('-');
            for (int count = 0; count < coordinates.Length; count++)
            {
                int[] x = ParseCoordinates(coordinates[count]);
                if (count == 0 || count % 2 == 0)
                {
                    tmpMap[x[0], x[1]] = true;
                }
                else
                {
                    tmpMap[x[0], x[1]] = false;
                }
            }
            return tmpMap;
        }

        private void RegisterMove(string coordinates, TicTacToeMatch tmpTTL)
        {
            if (tmpTTL.Moves == "" || tmpTTL.Moves == null)
                tmpTTL.Moves = $"{coordinates}";
            else
                tmpTTL.Moves = $"{tmpTTL.Moves}-{coordinates}";
            _context.TicTacToeMatch.Update(tmpTTL);
            _context.SaveChanges();
        }

        private int[] ParseCoordinates(string coordinates)
        {
            int[] parsedCoords = new int[2];
            if (!int.TryParse(coordinates[0].ToString(), out parsedCoords[0]))
                throw new Exception("Not a valid coordinate was entered!");
            if (!int.TryParse(coordinates[2].ToString(), out parsedCoords[1]))
                throw new Exception("Not a valid coordinate was entered!");
            return parsedCoords;
        }

        private bool IsMovePossible(string coordinates, TicTacToeMatch tmpTTL)
        {
            int[] parsedCoords = ParseCoordinates(coordinates);
            if (parsedCoords[0] < 0 || parsedCoords[0] > 2 || parsedCoords[1] < 0 || parsedCoords[1] > 2)
                return false;
            if (tmpTTL.Moves == null)
                return true;
            if (tmpTTL.Moves.Contains('#'))
                return false;
            return !tmpTTL.Moves.Split('-').Contains(coordinates);
        }

        private bool CreateGame(Guid tempGameId, out TicTacToeMatch tmpTTl)
        {
            return CreateGame(tempGameId, out tmpTTl, false);
        }

        private bool CreateGame(Guid tempGameId, out TicTacToeMatch tmpTTl, bool easyMode)
        {
            if (GameExists(tempGameId, out tmpTTl))
            {
                return true;
            }
            tmpTTl = new TicTacToeMatch();
            tmpTTl.GameId = tempGameId;
            Guid tempuserid;
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out tempuserid))
            {
                return false;
            }
            tmpTTl.Easy = easyMode;
            tmpTTl.UserId = tempuserid;
            _context.Add(tmpTTl);
            _context.SaveChanges();
            return true;
        }

        private bool GameExists(Guid gameId, out TicTacToeMatch tTTLeaderboards)
        {
            try
            {
                tTTLeaderboards = _context.TicTacToeMatch.Where(p => p.GameId == gameId).First();
                if (tTTLeaderboards == null)
                    return false;
            }
            catch (Exception e)
            {
                tTTLeaderboards = null;
                return false;
            }
            return true;
        }
    }
}