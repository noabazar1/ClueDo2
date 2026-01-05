using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class OpponentsGrid : OpponentsGridModel
    {
        public override void DisplayOponnentsNames()
        {
            int lblIndex = 0;
            for (int i = 0; i < game.MyIndex; i++)
            {
                lstOponnentsLabels[lblIndex].Text = game.GetPlayerName(i);
                lstOponnentsLabels[lblIndex++].BackgroundColor = i == game.NextPlay ? Colors.Transparent : Color.FromArgb("#003c00");
            }
            for (int i = game.MyIndex + 1; i < game.PlayersCount; i++)
            {
                lstOponnentsLabels[lblIndex].Text = game.GetPlayerName(i);
                lstOponnentsLabels[lblIndex++].BackgroundColor = game.IsOponnentTurn(i) ? Colors.Transparent : Color.FromArgb("#003c00");
            }
        }
        public OpponentsGrid(Grid grdOponnents, Game game) : base(grdOponnents, game)
        {
            int oponnentsCount = game.TotalPlayers - 1;
            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                lstOponnentsLabels.Add(new Label
                {
                    Text = Strings.Waiting,
                    FontSize = 14,
                    FontFamily = "ClueFont",
                    Margin = new Thickness(1),
                    Padding = new Thickness(1)
                });
                grdOponnents.Add(lstOponnentsLabels[i], i, 0);
            }
        }
    }
}
