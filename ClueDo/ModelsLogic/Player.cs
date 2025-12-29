using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class Player(string name, int index) : PlayerModel(name, index)
    {
        public Player() : this(string.Empty, 0) { }
    }
}
