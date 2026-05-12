using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that manages the opponents grid in the game page, allowing users to see the names of their 
    /// opponents and their colors. The OpponentsGrid class inherits from the OpponentsGridModel class, 
    /// which defines the properties and methods for managing the opponents grid. The OpponentsGrid class 
    /// implements the method for displaying the opponents' names in the grid, which creates a label for 
    /// each opponent in the game and sets the text of the label to the opponent's name. The background
    /// color of the label is set to the opponent's assigned color. The OpponentsGrid class is used in the
    /// GamePage to display the opponents' names and colors in a grid format.
    /// </summary>
    public class OpponentsGrid : OpponentsGridModel
    {
        /// <summary>
        /// constructor for the OpponentsGrid class, which initializes the grid and the game properties,
        /// and creates a label for each opponent in the game. The constructor takes a Grid object and a 
        /// Game object as parameters, and it adds a column to the grid for each opponent in the game. The 
        /// constructor also initializes the list of opponent labels and adds them to the grid. The labels 
        /// are styled with a specific font, text color, and margin to ensure they are visually distinct 
        /// and easy to read. The constructor is called in the GamePage when the page is initialized, and it
        /// sets up the opponents grid to display the opponents' names and colors when the game starts.
        /// </summary>
        /// <param name="grdOponnents"></param>
        /// <param name="game"></param>
        public OpponentsGrid(Grid grdOponnents, Game game) : base(grdOponnents, game)
        {
            int oponnentsCount = game.TotalPlayers - 1;
            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                lstOponnentsLabels.Add(new Label
                {
                    Text = string.Empty,
                    TextColor = Colors.White,
                    FontSize = 14,
                    FontFamily = Keys.ClueFont,
                    Margin = new Thickness(5),
                    Padding = new Thickness(1)
                });
                grdOponnents.Add(lstOponnentsLabels[i], i, 0);
            }
        }
        /// <summary>
        /// method to display the opponents' names in the grid, which is called when the game starts and 
        /// whenever there is a change in the opponents' information. The method iterates through the list
        /// of players in the game, excluding the current player, and updates the corresponding label in the
        /// grid with the opponent's name and color. If it is the opponent's turn, the background color of 
        /// the label is set to the opponent's assigned color; otherwise, it is set to the default color for
        /// that opponent.
        /// </summary>
        public override void DisplayOponnentsNames()
        {
            int lblIndex = 0;
            int myIndex = game.Players.MyIndex;
            for (int i = 0; i < game.Players.PlayersList.Count; i++)
            {
                if (i != myIndex)
                {
                    if (lblIndex < lstOponnentsLabels.Count)
                    {
                        Label lbl = lstOponnentsLabels[lblIndex];
                        lbl.Text = game.GetPlayerName(i);
                        lbl.BackgroundColor = game.Players.PlayersList[i].Color;
                        lblIndex++;
                    }
                }
            }
        }
    }
}