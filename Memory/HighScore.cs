using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    public class HighScore
    {
        string _name;
        int _time;

        [Newtonsoft.Json.JsonConstructor]
        public HighScore(string name, int time)
        {
            Name = name;
            Time = time;
        }
        public HighScore()
        {

        }
        public HighScore(int time) : this("", time) { }
        /// <summary>
        /// converts seconds to readable min:second time
        /// </summary>
        /// <returns></returns>
        public string ToReadable()
        {
            string results = "";
            int mins = 0;
            int seconds = 0;
            mins = Time / 60;
            seconds = Time % 60;

            results = string.Format("{0}:{1}:00",mins.ToString("D2"),seconds.ToString("D2"));
            return results;
        }

        public string Name { get => _name; set => _name = value; }
        public int Time { get => _time; set => _time = value; }
    }
}
