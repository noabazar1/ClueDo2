using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class Dice
    {
        public int Die1 { get; set; }
        public int Die2 { get; set; }
        public Dice()
        {
            Die1 = 1;
            Die2 = 1;
        }
        public void RollDice()
        {
            Random rand = new Random();
            Die1 = rand.Next(1, 7);
            Die2 = rand.Next(1, 7);
        }

        public void StartAnimation()
        {
            
        }
    }
}
