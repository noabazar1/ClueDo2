using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class OpponentsGrid : OpponentsGridModel
    {
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
                    Margin = new Thickness(1),
                    Padding = new Thickness(1)
                });
                grdOponnents.Add(lstOponnentsLabels[i], i, 0);
            }
        }
        public override void DisplayOponnentsNames()
        {
            int lblIndex = 0;
            int myIndex = game.Players.MyIndex;
            for (int i = 0; i < game.PlayersCount; i++)
            {
                if (i == myIndex)
                    continue;
                Label lbl = lstOponnentsLabels[lblIndex];
                lbl.Text = game.GetPlayerName(i);
                if (game.IsOponnentTurn(i))
                    lbl.BackgroundColor = game.GetPlayerColor(i);
                else
                    lbl.BackgroundColor = game.Players.PlayersList[i].Color;
                lblIndex++;
            }
        }
    }
}