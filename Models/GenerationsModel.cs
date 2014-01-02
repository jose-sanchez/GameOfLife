using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using GameOfLife.Helpers;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Collections.Concurrent;
namespace GameOfLife.Models
{
    public enum CellType
    {
        TopSide,
        TopRight,
        TopLeft,
        BottomSide,
        BottomLeft,
        BottomRight,
        LeftSide,
        RightSide,
        Center
    }
    public interface ICellRepository
    {
        /// <summary>
        /// Save a Game into the database
        /// </summary>
        /// <param name="game"></param>
        void SaveGame(Game game);
        /// <summary>
        /// Save a Cell into the database
        /// </summary>
        /// <param name="cell"></param>
        void SaveCell(Cell[] Cellarray);
    }

    public interface IGameofLife
    {
        /// <summary>
        /// Initialize the Grid
        /// </summary>
        /// <param name="GridWidth"></param>
        /// <param name="GridHeight"></param>
        Cell[] Start(Coordinates[] ListInitialCells, Game game);
        /// <summary>
        /// Return a list with the cell that will change the status from Alive to Dead and viceversa 
        /// </summary>
        /// <returns></returns>
        Cell[] NextGenerationGrid();
        /// <summary>
        /// Change the status of the Cells within ChangeStatusCell.
        /// </summary>
        /// <param name="ChangeStatusCell"></param>
        void UpdateGrid(Cell[] ChangeStatusCell);


    }
    /// <summary>
    /// Store the logic to initialize the grid and pass from one generation to another.
    /// </summary>
    public class GenerationsModel : IGameofLife
    {
        private Grid inputGrid;
        //
        //ConcurrentBag<Cell> ListAmendedCell;
        List<Cell> ListAmendedCell;
        private int GenerationCount = 0;
        public GenerationsModel(int GridWidth, int GridHeight)
        {
            if (GridWidth < 1 || GridHeight < 1)
                throw new ArgumentException("Param must be greater than 0");
            inputGrid = new Grid(GridWidth, GridHeight);
            //ListAmendedCell = new ConcurrentBag<Cell>();
            ListAmendedCell = new List<Cell>();
            GridHelper.InitializeReachableCells();

        }
        public Cell[] Start(Coordinates[] ListInitialCells, Game game)
        {
            inputGrid = new Grid(inputGrid.GridWidth, inputGrid.GridHeight);
            for (int x = 0; x < inputGrid.GridHeight; x++)
            {
                Row newRow = new Row();

                for (int y = 0; y < inputGrid.GridWidth; y++)
                {
                    Cell newCell = new Cell(new Coordinates(x, y), 0, false, game.StartGame_Id);
                    newCell.type = GridHelper.GetCellType(inputGrid, newCell.Coor);
                    newRow.CellsList.Add(newCell);
                }
                inputGrid.RowsList.Add(newRow);
            }
            foreach (Coordinates coor in ListInitialCells)
            {
                //inputGrid[coor.X, coor.Y].IsAlive = true;
                ListAmendedCell.Add(inputGrid[coor.X, coor.Y]);
            }
            return ListAmendedCell.ToArray();

        }
        /// <summary>
        /// Calculate the changes for the next generation
        /// </summary>
        /// <returns>Array with the Cells that will change the state in the next generation</returns>
        public Cell[] NextGenerationGrid()
        {
            
            ListAmendedCell.Clear();
            foreach (Row row in inputGrid.RowsList)
            {
                foreach (Cell cell in row.CellsList)
                {
                    if (GridHelper.ChangeStatus(inputGrid, cell.Coor))
                    {
                        ListAmendedCell.Add(cell);

                    }
                }
            }
            //Run in parallel
            //ListAmendedCell = new ConcurrentBag<Cell>();
            //Parallel.ForEach<Row>(inputGrid.RowsList, x =>
            //{
            //    foreach (Cell cell in x.CellsList)
            //    {
            //        if (GridHelper.ChangeStatus(inputGrid, cell.Coor))
            //        {
            //            ListAmendedCell.Add(cell);

            //        }
            //    }
            //});
            return ListAmendedCell.ToArray();
        }
        /// <summary>
        /// Toggle the state of a Cell array and update its generation to the current generation
        /// </summary>
        /// <param name="ChangeStatusCell"></param>
        public void UpdateGrid(Cell[] ChangeStatusCell)
        {
            GenerationCount++;
            foreach (Cell cell in ChangeStatusCell)
            {
                cell.IsAlive = !cell.IsAlive;
                cell.Generation = GenerationCount;
            }
        }
    }



    /// <summary>
    /// Represents the Grid that where the Cells live
    /// </summary>
    public class Grid
    {
        public readonly int GridWidth;
        public readonly int GridHeight;
        public List<Row> RowsList { get; set; }
        public Grid(int GridWidth, int GridHeight)
        {
            this.GridWidth = GridWidth;
            this.GridHeight = GridHeight;
            RowsList = new List<Row>();
        }
        /// <summary>
        /// Indexer to get grid cell by using indexes for ease of use
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>returns cell</returns>
        public Cell this[int x, int y]
        {
            get
            {
                if (GridHeight <= x || GridWidth <= y || 0 > x || 0 > y)
                    throw new ArgumentOutOfRangeException("Argument out of bound");
                return RowsList[x].CellsList[y];
            }
            set
            {
                if (GridHeight <= x || GridWidth <= y || 0 > x || 0 > y)
                    throw new ArgumentOutOfRangeException("Argument out of bound");
                RowsList[x].CellsList[y] = value;
            }
        }
    }
    /// <summary>
    /// Represent a Grid row , stores the cells that lives in this grid row
    /// </summary>
    public class Row
    {
        public List<Cell> CellsList { get; set; }
        /// <summary>
        /// Get a cell by the index
        /// </summary>
        /// <param name="y"></param>
        /// <returns>returns cell</returns>
        public Cell this[int y]
        {
            get { if (CellsList.Count - 1 < y || y < 0) throw new ArgumentOutOfRangeException(); return CellsList[y]; }
            set { if (CellsList.Count - 1 < y || y < 0) throw new ArgumentOutOfRangeException(); CellsList[y] = value; }
        }
        public Row()
        {
            CellsList = new List<Cell>();
        }
    }
    public class GameOfLifeRepository : ICellRepository
    {
        public void SaveGame(Game game)
        {
            DAL dal = new DAL();
            dal.SaveGame(game);
        }
        public void SaveCell(Cell[] Cellarray)
        {
            DAL dal = new DAL();
            dal.SaveCell(Cellarray);
        }
    }
    public class DAL : DbContext
    {
        public DbSet<Game> GameList { get; set; }
        public DbSet<Cell> CellList { get; set; }
        DAL repository;
        public DAL()
        {
            this.repository = this;
        }
        public void SaveGame(Game game)
        {
            if (game != null)
            {
                try
                {
                    repository.GameList.Add(game);
                    repository.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            else throw new ArgumentNullException("The parameter game cannot be null");
        }

        public void SaveCell(Cell[] Cellarray)
        {
            try
            {
                foreach (Cell cell in Cellarray)
                {

                    if (cell != null)
                    {
                        repository.CellList.Add(cell);

                    }
                    else throw new ArgumentNullException("The parameter cell cannot be null");
                }
                repository.SaveChanges();

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


        }
    }
    /// <summary>
    /// Store the details of every game,so far only the start date but 
    /// could be extended to store the game name, user or other details
    /// As this web app will be use for anonimous user and only one at a time
    /// the Date will be use as key to store it.
    /// </summary>
    public class Game
    {
        
        [Key]
        public DateTime StartGame_Id { get; set; }
        public virtual ICollection<Cell> CellList { get; set; }
        public Game(DateTime StartGame)
        {
            this.StartGame_Id = StartGame;
        }


    }
    /// <summary>
    /// Store the Cell details generated in every game.
    /// </summary>
    public class Cell
    {

        [Key]
        [Required]
        [Column(Order = 0)]
        //Reference to a a specific Game
        public DateTime StartGame_Id { get; set; }
        [Required]
        [Key]
        [Column(Order = 1)]
        //Cell generation
        public int Generation { get; set; }
        [Required]
        [Key]
        [Column(Order = 2)]
        //Coordinate X
        public int X { get; set; }
        [Required]
        [Key]
        [Column(Order = 3)]
        //Coordinate Y
        public int Y { get; set; }
        public CellType type { get; set; }
        [NotMapped]
        //Coordinate 
        public Coordinates Coor { get { return new Coordinates(X, Y); } }
        [Required]
        [Key]
        [Column(Order = 4)]
        //Cell is Alive or Dead
        public bool IsAlive { get; set; }

        public virtual Game game { get; set; }

        public Cell(Coordinates coor, int Generation, bool IsAlive, DateTime StartGame)
        {
            if (coor == null) throw new NullReferenceException();
            this.Generation = Generation;
            this.IsAlive = IsAlive;
            X = coor.X;
            Y = coor.Y;
            this.StartGame_Id = StartGame;
        }

    }
    /// <summary>
    /// Describe the coordinates of a cell into the grid
    /// </summary>
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

    }

}