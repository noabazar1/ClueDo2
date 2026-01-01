using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class Player(string name, int index, IndexButton button) : PlayerModel(name, index, button)
    {
        public Player() : this(string.Empty, 0, new IndexButton()) { }
        public Player(string name, int index) : this(name, index, new IndexButton()) { }
    }
}
