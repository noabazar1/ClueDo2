using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class Players : PlayersModel
    {
        public Players() { }
        public override void Add(Player p)
        {
            PlayersList.Add(p);
        }
        public override string GetPlayerName(int index)
        {
            return PlayersList[index].Name;
        }
        public override void SetNextPlayer()
        {
            NextPlay = (NextPlay + 1) % TotalPlayers;
        }
        public override bool IsOpponentTurn(int oponnentIndex)
        {
            return oponnentIndex == NextPlay;
        }
    }
}
