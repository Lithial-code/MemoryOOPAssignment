using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public class Card
    {
        string _name;
        string _imageLocation;
        int _id;
        PictureBox _pb;

        public Card(string name, string imageLocation, int id)
        {
            this.Name = name;
            this.ImageLocation = imageLocation;
            this.Id = id;
        }
        public string Name { get => _name; set => _name = value; }
        public string ImageLocation { get => _imageLocation; set => _imageLocation = value; }
        public int Id { get => _id; set => _id = value; }
        public PictureBox Pb { get => _pb; set => _pb = value; }
    }
}
