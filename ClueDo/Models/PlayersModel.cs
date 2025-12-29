using Plugin.CloudFirestore.Attributes;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    public abstract class PlayersModel
    {
        [Ignored]
        public int MyIndex { get; set; } = 0;
        [Ignored]
        public int Count => PlayersList.Count;
        public List<Player> PlayersList { get; set; } = [];
        public int NextPlay { get; set; }
        public int TotalPlayers { get; set; }

        public abstract void Add(Player p);
        public abstract string GetPlayerName(int index);
        public abstract bool IsOponnentTurn(int oponnentIndex);
        public abstract void SetNextPlayer();
        public abstract bool IsMyTurn();
        public abstract void Play(int rowIndex, int columnIndex);
    }
}
