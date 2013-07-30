using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elp.StegoIn.lib
{
    public class Pixel
    {
        public byte a { get; set; }
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }

        public Pixel() { }
        public Pixel(byte a, byte r, byte g, byte b)
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }

    }
}
