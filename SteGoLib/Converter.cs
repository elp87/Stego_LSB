using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace elp.StegoIn.lib
{
    public static class Converter
    {
        private const string _nullBitArray = "0000000000000000";

        public static List<Pixel> ImageToByte(System.Drawing.Bitmap img)
        {
            int count = img.Width * img.Height * 4;
            List<Pixel> bl = new List<Pixel>();
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    Color c = img.GetPixel(j, i);
                    Pixel p = new Pixel(c.A, c.R, c.G, c.B);
                    bl.Add(p);
                }
            }
            return bl;
        }

        public static string StringToBitString(char[] txtArray)
        {
            string array = "";
            for (int i = 0; i < txtArray.Count(); i++)
            {
                char[] a = _nullBitArray.ToCharArray();
                char[] b = Convert.ToString(txtArray[i], 2).ToCharArray();
                for (int j = 0; j < b.Count(); j++)
                {
                    if (b.Count() - 1 - j >= 0) a[a.Count() - 1 - j] = b[b.Count() - 1 - j];
                }
                string s = new string(a);
                array += s;
            }
            array += _nullBitArray;
            return array;
        }

        public static string BitStringToString(string[] btxtArray)
        {
            string s = "";
            for (int i = 0; i < btxtArray.Count(); i++)
            {
                uint ui = Convert.ToUInt32(btxtArray[i], 2);
                char ch = Convert.ToChar(ui);
                s = s + ch;
            }
            return s;
        }

        public static List<Pixel> EncodeMessage(List<Pixel> pixelArray, string btxtArray)
        {
            int index = 0;
            for (int i = 0; i < btxtArray.Length; i = i + 4)
            {
                Pixel curPixel = pixelArray[index];
                string messagePart = "";
                messagePart += btxtArray[i].ToString() + btxtArray[i + 1].ToString() + btxtArray[i + 2].ToString() + btxtArray[i + 3].ToString();
                byte blue = curPixel.b;
                string blueBit = Converter.ByteToBitString(blue);
                byte newBlue = Converter.InsertPart(blueBit, messagePart);
                curPixel.b = newBlue;

                index++;
            }
            return pixelArray;
        }

        private static byte InsertPart(string blueBit, string messagePart)
        {
            int count = blueBit.Length;
            char[] blueChar = blueBit.ToCharArray();
            char[] messChar = messagePart.ToCharArray();
            for (int i = 0; i < 4; i++)
            {
                blueChar[count - 1 - i] = messChar[messagePart.Length - 1 - i];
            }
            string resultBitString = new string(blueChar);
            return Convert.ToByte(resultBitString, 2);
        }

        private static string ByteToBitString(byte b)
        {
            string s =  Convert.ToString(b, 2);
            if (s.Length == 1) s = "000" + s;
            if (s.Length == 2) s = "00" + s;
            if (s.Length == 3) s = "0" + s;
            return s;
        }

        public static string DecodeMessage(List<Pixel> pixelArray)
        {
            string message = "";
            for (int i = 0; i < pixelArray.Count; i += 4)
            {
                string letterCode = "";
                for (int j = 0; j < 4; j++)
                {
                    int pos = i + j;
                    byte blue = pixelArray[pos].b;
                    string blueString = ByteToBitString(blue);
                    string charPart = getUseBits(blueString);
                    letterCode += charPart;
                }
                uint ui = Convert.ToUInt32(letterCode, 2);
                if (ui == 0) return message;
                char ch = Convert.ToChar(ui);
                message += ch;
            }
            return message;
        }

        private static string getUseBits(string blueString)
        {
            int length = blueString.Length;
            string mes = blueString[length - 4].ToString() + blueString[length - 3].ToString() + blueString[length - 2].ToString() + blueString[length - 1].ToString();
            return mes;
        }
    }
}
