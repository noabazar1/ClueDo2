using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class Room
    {
        public string Name { get; }
        public Point Point;
        public IndexButton OpenRoom;
        public Room(string name, Point Point, IndexButton openRoom)
        {
            Name = name;
            Point = Point;
            OpenRoom = openRoom;
        }
        public Room()
        {
            Name = string.Empty;
            Point = new Point(0, 0);
            OpenRoom = new IndexButton(0, 0);
        }
    }
}
