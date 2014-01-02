Game Of Life
==========
 
This is web application version of the game "Game of Life" invented by John Horton Conway.

The version is intended to play in a browser , accesing only one user at a time.
Every game will be store in a database saving in every generation which cell become alive or dead.
 
Technology
--------------------
 
+ MVC .Net with c#
+ Entity Framework code first
+ javascript,Ajax,jquery
+ SQL Express

Installation
--------------------
+ 1)Clone the repository in your local machine

+ 2)Open the solution with Visual Studio and compile

+ 3)Set the connection string in the Web.Config file to point to your local database server.

+ 4)Deploy into ISS or Run it using Visual Studio


Game Instructions
--------------------

The game page contains 3 buttons :

+ Start : Start the simulation
+ Stop : Stop the simulation
+ Reset: Reset the simulation in order to set a initial state.

Before pressing start you must set the initial state of the map by clicking the map position where you want a alive cell.
When you click a position in the map the color should change to purple indicating that this cell will be alive.
Once you finish creating the initial state press on the Start button.

Game Message
--------------------
While running the simulation can appear different messsages:
+ Select in the map the initial Cells Alive and press reset to start a new game: Initial message.
+ Loading:It appears when waiting for the server to calculate the next generation.
+ Stable state reached,press reset to start a new game:This will appear when there was no change betwen 2 generations,you must press reset to start a new game.
+ Unexpected error. Please refresh the page to load again:Appears when the connection with the server was lost or when there was a server error/problem.

