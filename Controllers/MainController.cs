using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameOfLife.Models;
using System.Web.Script.Serialization;
namespace GameOfLife.Controllers
{
    public class MainController : Controller
    {
        static IGameofLife Generationsgame;
        static ICellRepository repository;
        public static Game CurrentGame;
        static bool initialize=false;
        public MainController()
        {
            
            if (!MainController.initialize)
            {
                MainController.Generationsgame = new GenerationsModel(64, 64);
                repository = new GameOfLifeRepository();
                MainController.initialize = true;
            }

        }
        //
        // GET: /Main/
        public MainController(IGameofLife game,ICellRepository repository)
        {
            MainController.Generationsgame = game;
            MainController.repository = repository;
        }
        /// <summary>
        /// Return the main web view
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            ViewBag.GridRow = Properties.Settings.Default.Rows;
            ViewBag.GridColumn = Properties.Settings.Default.Rows;
            return View();
        }
        /// <summary>
        /// Initialize a Game setting the initial cells alive.
        /// </summary>
        /// <param name="coor">json coor[] to string</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Start(string coor)
        {
            //Gonvert coordenates from string to Coordinates[]
            var serializer = new JavaScriptSerializer();
            
            CurrentGame = new Game(DateTime.Now);
            List<Coordinates> coorList = new List<Coordinates>();
            try
            {
                var jsoncoor = (Object[])serializer.Deserialize<object>(coor);
                foreach (dynamic item in jsoncoor)
                {
                    coorList.Add(new Coordinates(Convert.ToInt16(item["X"]), Convert.ToInt16(item["Y"])));
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
            //Initialize the cell grid and get the cells to update
            Cell[] ChangedStatusCells = Generationsgame.Start(coorList.ToArray(), CurrentGame);
            //Update the cells state
            Generationsgame.UpdateGrid(ChangedStatusCells);
            //Create a new entry for the current game
            MainController.repository.SaveGame(CurrentGame);
            return Json(true);
        }
        /// <summary>
        /// Calculate next generation and store the changes in the database.
        /// </summary>
        /// <returns></returns>
        public JsonResult NextGeneration()
        {
            Cell[] ChangedStatusCells;
            //Get array with the cells which will change the status in the next generation
            ChangedStatusCells = Generationsgame.NextGenerationGrid();
            //Update the Cell State
            Generationsgame.UpdateGrid(ChangedStatusCells);
            
            Coordinates[] ChangedCoor = new Coordinates[ChangedStatusCells.Length];
            int index = 0;
            //Set the cells to be store in the dabase and create the cell coordinates array
            //
            foreach (Cell cell in ChangedStatusCells)
            {
                cell.StartGame_Id = CurrentGame.StartGame_Id;
                ChangedCoor[index] = cell.Coor;
                index++;
            }
            //Save the amended Cells
            MainController.repository.SaveCell(ChangedStatusCells);
            return Json(ChangedCoor, JsonRequestBehavior.AllowGet);


        }

    }
}
