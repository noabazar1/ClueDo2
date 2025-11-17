using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class GameStatus
    {
        private readonly string[] msgs = [Strings.WaitMessage, Strings.PlayMessage];
        public enum Status { Wait, Play }
        public Status CurrentStatus { get; set; } = Status.Wait;
        public string StatusMessage => msgs[(int)CurrentStatus];

        internal void UpdateStatus()
        {

        }
    }
}
