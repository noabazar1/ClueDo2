using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    public abstract class OpponentsGridModel(Grid grdOponnents, Game game)
    {
        protected Grid grdOponnents = grdOponnents;
        protected Game game = game;
        protected readonly List<Label> lstOponnentsLabels = [];
        public abstract void DisplayOponnentsNames();
    }
}
