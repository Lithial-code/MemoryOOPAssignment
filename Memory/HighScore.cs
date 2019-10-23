using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    class HighScore
    {
        string _name;
        int _position;
        string _time;

        public HighScore(string name, int position, string time)
        {
            Name = name;
            Position = position;
            Time = time;
        }

        public string Name { get => _name; set => _name = value; }
        public int Position { get => _position; set => _position = value; }
        public string Time { get => _time; set => _time = value; }
    }
}
