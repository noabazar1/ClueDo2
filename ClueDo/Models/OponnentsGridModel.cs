using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the data of the opponents grid, and the method to display the opponents names in
    /// the grid. This class is used in the GamePage to display the opponents names in the grid. 
    /// </summary>
    /// <param name="grdOponnents"></param>
    /// <param name="game"></param>
    public abstract class OpponentsGridModel(Grid grdOponnents, Game game)
    {
        protected Grid grdOponnents = grdOponnents;
        protected Game game = game;
        protected readonly List<Label> lstOponnentsLabels = [];
        /// <summary>
        /// abstract method to display the opponents names in the grid, which will be implemented in the
        /// OpponentsGrid class. This method will create a label for each opponent in the game, and it 
        /// will set the text of the label to the opponent's name, and it will add the label to the grid.
        /// </summary>
        public abstract void DisplayOponnentsNames();
    }
}
