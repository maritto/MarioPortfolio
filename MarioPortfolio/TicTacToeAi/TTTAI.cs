using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace MarioPortfolio.TicTacToeAi
{
    public class Coordinate
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void SetCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

    }

    public static class TTTAI
    {

        public static string CoordToString(this Coordinate coordinate) => $"{coordinate.X},{coordinate.Y}";

        public static Coordinate ToCoordinate(this string str) => new Coordinate(int.Parse(str.Split(',')[0]), int.Parse(str.Split(',')[1]));

        /// <summary>
        /// Plays a random move.
        /// </summary>
        /// <param name="map">Current game map</param>
        /// <returns>Move coordinate</returns>
        public static string EasyMove(this bool?[,] map)
        {
            Random random = new Random();
            List<Coordinate> availableSpots = new List<Coordinate>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (map[x, y] == null)
                    {
                        availableSpots.Add(new Coordinate(x, y));
                    }
                }
            }
            if (availableSpots.Count() < 1)
                return null;
            return availableSpots[random.Next(availableSpots.Count())].CoordToString();
        }

        /// <summary>
        /// Finds win or loss in 1 move. Else random valid coordinate.
        /// </summary>
        /// <param name="map">Current game map</param>
        /// <returns>Move played</returns>
        public static string NotSoEasyMove(this bool?[,] map)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (map.IsValidMove(new Coordinate(x, y)))
                    {
                        bool?[,] tmpMap = new bool?[3, 3];
                        Array.Copy(map, tmpMap, 9);
                        tmpMap[x, y] = false;
                        if (tmpMap.IsWinConditionAchieved(out _))
                        {
                            return new Coordinate(x, y).CoordToString();
                        }
                    }
                }
            }
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (map.IsValidMove(new Coordinate(x, y)))
                    {
                        bool?[,] tmpMap = new bool?[3, 3];
                        Array.Copy(map, tmpMap, 9);
                        tmpMap[x, y] = true;
                        if (tmpMap.IsWinConditionAchieved(out _))
                        {
                            return new Coordinate(x, y).CoordToString();
                        }
                    }
                }
            }
            return map.EasyMove();
        }

        /// <summary>
        /// Verifies if there is 3 of the same in a row
        /// </summary>
        /// <param name="tmpTTL">Match Details</param>
        /// <param name="winner">Returns result for X</param>
        /// <returns>Returns if a win condition was achieved.</returns>
        public static bool IsWinConditionAchieved(this bool?[,] map, out bool? winner)
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
        /// Verifies if map has the selected coordinate available.
        /// </summary>
        /// <param name="map">Map to check</param>
        /// <param name="tmp">Coordinate to check</param>
        /// <returns>Availability of the coordinate in the map</returns>
        public static bool IsValidMove(this bool?[,] map, Coordinate tmp) => map[tmp.X, tmp.Y] == null ? true : false;

        /// <summary>
        /// Builds a game map where X is true and O is false
        /// </summary>
        /// <param name="moves">List of moves in string format</param>
        /// <returns>Map array</returns>
        public static bool?[,] BuildMap(this string moves)
        {
            bool?[,] tmpMap = new bool?[3, 3];
            string[] coordinates = moves.Split('-');
            for (int count = 0; count < coordinates.Length; count++)
            {
                Coordinate coord = coordinates[count].ToCoordinate();
                tmpMap[coord.X, coord.Y] = (count == 0 || count % 2 == 0) ? true : false;
            }
            return tmpMap;
        }

    }
}
