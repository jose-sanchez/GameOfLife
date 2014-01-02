using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameOfLife.Models;
namespace GameOfLife.Helpers
{
    public class GridHelper
    {
        // Dictionary to hold list of reachable cells co-ordinates for specified cell type
        public static Dictionary<CellType, List<Coordinates>> ReachableCells;
        /// <summary>
        /// initialize all reachable cells in Dictionary(Key=> CellType, Value => List of Reachable cell co-ordinates
        /// </summary>
        public static void InitializeReachableCells()
        {
            CellType Cell;
            ReachableCells = new Dictionary<CellType, List<Coordinates>>();

            // Add Reachable adjacent cell co-ordinates for Top Left corner cell into Dictionary with TopLeft CellType as key
            Cell = CellType.TopLeft;
            List<Coordinates> TopLeftCoOrdinateList = new List<Coordinates>();
            TopLeftCoOrdinateList.Add(new Coordinates(0, 1));
            TopLeftCoOrdinateList.Add(new Coordinates(1, 1));
            TopLeftCoOrdinateList.Add(new Coordinates(1, 0));
            ReachableCells.Add(Cell, TopLeftCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for Top right corner cell into Dictionary with TopRightCorner CellType as key
            Cell = CellType.TopRight;
            List<Coordinates> TopRightCoOrdinateList = new List<Coordinates>();
            TopRightCoOrdinateList.Add(new Coordinates(1, 0));
            TopRightCoOrdinateList.Add(new Coordinates(1, -1));
            TopRightCoOrdinateList.Add(new Coordinates(0, -1));
            ReachableCells.Add(Cell, TopRightCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for bottom left corner cell into Dictionary with BottomLeftCorner CellType as key
            Cell = CellType.BottomLeft;
            List<Coordinates> BottomLeftCoOrdinateList = new List<Coordinates>();
            BottomLeftCoOrdinateList.Add(new Coordinates(-1, 0));
            BottomLeftCoOrdinateList.Add(new Coordinates(-1, 1));
            BottomLeftCoOrdinateList.Add(new Coordinates(0, 1));
            ReachableCells.Add(Cell, BottomLeftCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for bottom right corner cell into Dictionary with BottomRight CellType as key
            Cell = CellType.BottomRight;
            List<Coordinates> BottomRightCoOrdinateList = new List<Coordinates>();
            BottomRightCoOrdinateList.Add(new Coordinates(0, -1));
            BottomRightCoOrdinateList.Add(new Coordinates(-1, -1));
            BottomRightCoOrdinateList.Add(new Coordinates(-1, 0));
            ReachableCells.Add(Cell, BottomRightCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for top side cell into Dictionary with BottomRight TopSide as key
            Cell = CellType.TopSide;
            List<Coordinates> TopSideCoOrdinateList = new List<Coordinates>();
            TopSideCoOrdinateList.Add(new Coordinates(0, 1));
            TopSideCoOrdinateList.Add(new Coordinates(1, 1));
            TopSideCoOrdinateList.Add(new Coordinates(1, 0));
            TopSideCoOrdinateList.Add(new Coordinates(1, -1));
            TopSideCoOrdinateList.Add(new Coordinates(0, -1));
            ReachableCells.Add(Cell, TopSideCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for bottom side cell into Dictionary with BottomRight BottomSide as key
            Cell = CellType.BottomSide;
            List<Coordinates> BottomSideCoOrdinateList = new List<Coordinates>();
            BottomSideCoOrdinateList.Add(new Coordinates(0, -1));
            BottomSideCoOrdinateList.Add(new Coordinates(-1, -1));
            BottomSideCoOrdinateList.Add(new Coordinates(-1, 0));
            BottomSideCoOrdinateList.Add(new Coordinates(-1, 1));
            BottomSideCoOrdinateList.Add(new Coordinates(0, 1));
            ReachableCells.Add(Cell, BottomSideCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for left side cell into Dictionary with BottomRight LeftSide as key
            Cell = CellType.LeftSide;
            List<Coordinates> LeftSideCoOrdinateList = new List<Coordinates>();
            LeftSideCoOrdinateList.Add(new Coordinates(-1, 0));
            LeftSideCoOrdinateList.Add(new Coordinates(-1, 1));
            LeftSideCoOrdinateList.Add(new Coordinates(0, 1));
            LeftSideCoOrdinateList.Add(new Coordinates(1, 1));
            LeftSideCoOrdinateList.Add(new Coordinates(1, 0));
            ReachableCells.Add(Cell, LeftSideCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for right side cell into Dictionary with BottomRight RightSide as key
            Cell = CellType.RightSide;
            List<Coordinates> RightSideCoOrdinateList = new List<Coordinates>();
            RightSideCoOrdinateList.Add(new Coordinates(1, 0));
            RightSideCoOrdinateList.Add(new Coordinates(1, -1));
            RightSideCoOrdinateList.Add(new Coordinates(0, -1));
            RightSideCoOrdinateList.Add(new Coordinates(-1, -1));
            RightSideCoOrdinateList.Add(new Coordinates(-1, 0));
            ReachableCells.Add(Cell, RightSideCoOrdinateList);

            // Add Reachable adjacent cell co-ordinates for Center cell into Dictionary with BottomRight Center as key
            Cell = CellType.Center;
            List<Coordinates> CenterCoOrdinateList = new List<Coordinates>();
            CenterCoOrdinateList.Add(new Coordinates(-1, 0));
            CenterCoOrdinateList.Add(new Coordinates(-1, 1));
            CenterCoOrdinateList.Add(new Coordinates(0, 1));
            CenterCoOrdinateList.Add(new Coordinates(1, 1));
            CenterCoOrdinateList.Add(new Coordinates(1, 0));
            CenterCoOrdinateList.Add(new Coordinates(1, -1));
            CenterCoOrdinateList.Add(new Coordinates(0, -1));
            CenterCoOrdinateList.Add(new Coordinates(-1, -1));
            ReachableCells.Add(Cell, CenterCoOrdinateList);

        }
        /// <summary>
        /// Return the type of Cell acording to the Coordinates within the Grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="coor"></param>
        /// <returns></returns>
        public static CellType GetCellType(Grid grid, Coordinates coor)
        {
            if ((coor.X < 0 || coor.X > grid.GridWidth) || (coor.Y < 0 || coor.Y > grid.GridHeight) || (grid.GridHeight<1 || grid.GridWidth<1))
            {
                throw new ArgumentOutOfRangeException();
            }
            CellType cellType =CellType.BottomLeft;
            if (coor.X == 0 && coor.Y == 0)
                cellType = CellType.TopLeft;
            else if (coor.X == 0 && coor.Y == grid.GridWidth - 1)
                cellType = CellType.TopRight;
            else if (coor.X == 0 && (coor.Y > 0 && coor.Y < grid.GridWidth - 1))
                cellType = CellType.TopSide;
            else if (coor.X == grid.GridHeight - 1 && coor.Y == 0)
                cellType = CellType.BottomLeft;
            else if (coor.X == grid.GridHeight - 1 && coor.Y == grid.GridWidth - 1)
                cellType = CellType.BottomRight;
            else if (coor.X == grid.GridHeight - 1 && (coor.Y > 0 && coor.Y < grid.GridWidth - 1))
                cellType = CellType.BottomSide;
            else if ((coor.X > 0 && coor.X < grid.GridHeight - 1) && coor.Y == 0)
                cellType = CellType.LeftSide;
            else if ((coor.X > 0 && coor.X < grid.GridHeight - 1) && coor.Y == grid.GridWidth - 1)
                cellType = CellType.RightSide;
            else if ((coor.X > 0 && coor.X < grid.GridHeight - 1) && (coor.Y > 0 && coor.Y < grid.GridWidth - 1))
                cellType = CellType.Center;

            return cellType;
        }
        /// <summary>
        /// Return true if a cell in a specific coordinate change the status
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="coor"></param>
        /// <returns></returns>
        public static bool ChangeStatus(Grid grid, Coordinates coor)
        {
            //Calculate number of Alive Neighbour
            int Neighbours = GridHelper.CountAliveNeighbours(grid,coor);
            //Rules
            if (grid[coor.X, coor.Y].IsAlive)          
            {
                if (Neighbours != 2 && Neighbours != 3) return true;
            }
            if (!grid[coor.X, coor.Y].IsAlive)
            {
                if (Neighbours == 3) return true;               
            }
            return false;
        }
        /// <summary>
        /// Count live adjacent cells for specified cell co-ordinates
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="coor"></param>
        /// <returns>returns number of live neighbours</returns>
        private static int CountAliveNeighbours(Grid grid, Coordinates coor)
        {
            int liveNeighbours = 0;
            // Get the Cell type of current cell
            CellType celltype = grid[coor.X,coor.Y].type;
            List<Coordinates> reachableCells = new List<Coordinates>();
            // populate reachable cells from current cell for easier traversing
            GridHelper.ReachableCells.TryGetValue(celltype, out reachableCells);
            if (reachableCells.Count == 0) throw new ArgumentNullException("Cannot find reachable co-ordinates");
            foreach (Coordinates coord in reachableCells)
            {
                liveNeighbours += IsAliveNeighbour(grid, coor, coord);
            }
            return liveNeighbours;
        }

        /// <summary>
        /// Check if the Neighbour cell is alive
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="cell"></param>
        /// <param name="neighbour"></param>
        /// <returns>returns 1 if alive otherwise 0</returns>
        private static int IsAliveNeighbour(Grid grid, Coordinates cell, Coordinates neighbour)
        {
            int live = 0; // set default as 0
            int x = cell.X + neighbour.X; // get x axis of neighbour
            int y = cell.Y + neighbour.Y; // get y axis of neighbour
            // check the computed bound is within range of grid, if it is not within bounds live is 0 as default
            if ((x >= 0 && x < grid.GridHeight) && y >= 0 && y < grid.GridWidth)
            {
                // if reachable neighbour cell is alive then set live to 1 otherwise 0
                live = grid[x, y].IsAlive ? 1 : 0;
            }
            return live;
        }

    }
}