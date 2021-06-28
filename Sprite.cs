using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;

namespace Aladdin_SNES_Text_Changer
{
    class Sprite
    {
        private System.Windows.Controls.Image IMG = new System.Windows.Controls.Image();
        private Bitmap BMP;
        private Bitmap refPIC = new Bitmap(global::Aladdin_SNES_Text_Changer.Properties.Resources.CHARMAP);
        private char CharValue;

        // getters/setters
        public char GetCharValue                            { get { return CharValue;   } }
        public System.Windows.Controls.Image GetIMGcontrol  { get { return IMG;         } }

        // Here's how it works
        // all the characters are found on the single PNG file "CHARMAP.png"
        // each chars in the game are 8x8 pixels in the image
        // so when the program has to print a char
        // it will copy a 8x8 pixel chunk from a coordinate (determined by the key in tht dictionary)
        // then creates a whole new 8x8 image that all the pixels would be taken from that 8x8 chunk computed before
        private Dictionary<char, int[]> BitmapAreaDico = new Dictionary<char, int[]>()
        {
            { '©' , new int[2] { 128, 320 } }, { ' ' , new int[2] { 128, 312 } },
            { '$' , new int[2] { 264, 320 } }, { '\'', new int[2] { 281, 320 } },
            { '!' , new int[2] { 289, 320 } }, { '?' , new int[2] { 297, 320 } },
            { ';' , new int[2] { 306, 320 } }, { ':' , new int[2] { 314, 320 } },
            { ',' , new int[2] { 322, 320 } }, { '.' , new int[2] { 330, 320 } },
            { '"' , new int[2] { 337, 320 } }, { '(' , new int[2] { 345, 320 } },
            { ')' , new int[2] { 352, 320 } }, { '*' , new int[2] { 360, 320 } },
            { '-' , new int[2] { 368, 320 } }, { '=' , new int[2] { 376, 320 } },
            { '0' , new int[2] { 384, 320 } }, { '1' , new int[2] { 392, 320 } },
            { '2' , new int[2] { 400, 320 } }, { '3' , new int[2] { 408, 320 } },
            { '4' , new int[2] { 416, 320 } }, { '5' , new int[2] { 424, 320 } },
            { '6' , new int[2] { 432, 320 } }, { '7' , new int[2] { 440, 320 } },
            { '8' , new int[2] { 448, 320 } }, { '9' , new int[2] { 456, 320 } },
            { 'A' , new int[2] { 464, 320 } }, { 'B' , new int[2] { 472, 320 } },
            { 'C' , new int[2] { 480, 320 } }, { 'D' , new int[2] { 488, 320 } },
            { 'E' , new int[2] { 496, 320 } }, { 'F' , new int[2] { 504, 320 } },
            { 'G' , new int[2] { 56 , 328 } }, { 'H' , new int[2] { 64 , 328 } },
            { 'I' , new int[2] { 72 , 328 } }, { 'J' , new int[2] { 80 , 328 } },
            { 'K' , new int[2] { 88 , 328 } }, { 'L' , new int[2] { 96 , 328 } },
            { 'M' , new int[2] { 104, 328 } }, { 'N' , new int[2] { 112, 328 } },
            { 'O' , new int[2] { 120, 328 } }, { 'P' , new int[2] { 128, 328 } },
            { 'Q' , new int[2] { 136, 328 } }, { 'R' , new int[2] { 144, 328 } },
            { 'S' , new int[2] { 152, 328 } }, { 'T' , new int[2] { 160, 328 } },
            { 'U' , new int[2] { 168, 328 } }, { 'V' , new int[2] { 176, 328 } },
            { 'W' , new int[2] { 184, 328 } }, { 'X' , new int[2] { 192, 328 } },
            { 'Y' , new int[2] { 200, 328 } }, { 'Z' , new int[2] { 208, 328 } },

            // added those since they left all the Japanese characters for unknown reasons
            // probably for making the localisation process faster
            { 'あ', new int[2] { 0  , 336 } }, { 'い', new int[2] { 8  , 336 } },
            { 'う', new int[2] { 16 , 336 } }, { 'え', new int[2] { 24 , 336 } },
            { 'お', new int[2] { 32 , 336 } }, { 'か', new int[2] { 40 , 336 } },
            { 'き', new int[2] { 48 , 336 } }, { 'く', new int[2] { 56 , 336 } },
            { 'け', new int[2] { 64 , 336 } }, { 'こ', new int[2] { 72 , 336 } },
            { 'さ', new int[2] { 80 , 336 } }, { 'し', new int[2] { 88 , 336 } },
            { 'す', new int[2] { 96 , 336 } }, { 'せ', new int[2] { 104, 336 } },
            { 'そ', new int[2] { 112, 336 } }, { 'た', new int[2] { 120, 336 } },
            { 'ち', new int[2] { 128, 336 } }, { 'つ', new int[2] { 136, 336 } },
            { 'て', new int[2] { 144, 336 } }, { 'と', new int[2] { 152, 336 } },
            { 'な', new int[2] { 160, 336 } }, { 'に', new int[2] { 168, 336 } },
            { 'ぬ', new int[2] { 176, 336 } }, { 'ね', new int[2] { 184, 336 } },
            { 'の', new int[2] { 192, 336 } }, { 'は', new int[2] { 200, 336 } },
            { 'ひ', new int[2] { 208, 336 } }, { 'ふ', new int[2] { 216, 336 } },
            { 'へ', new int[2] { 224, 336 } }, { 'ほ', new int[2] { 232, 336 } },
            { 'ま', new int[2] { 240, 336 } }, { 'み', new int[2] { 248, 336 } },
            { 'む', new int[2] { 256, 336 } }, { 'め', new int[2] { 264, 336 } },
            { 'も', new int[2] { 272, 336 } }, { 'や', new int[2] { 280, 336 } },
            { 'ゆ', new int[2] { 288, 336 } }, { 'よ', new int[2] { 296, 336 } },
            { 'ら', new int[2] { 304, 336 } }, { 'り', new int[2] { 312, 336 } },
            { 'る', new int[2] { 320, 336 } }, { 'れ', new int[2] { 328, 336 } },
            { 'ろ', new int[2] { 336, 336 } }, { 'わ', new int[2] { 344, 336 } },
            { 'を', new int[2] { 352, 336 } }, { 'ん', new int[2] { 340, 336 } },
            { 'ゃ', new int[2] { 348, 336 } }, { 'ゅ', new int[2] { 356, 336 } },
            { 'ょ', new int[2] { 364, 336 } }, { '。', new int[2] { 448, 336 } },
            { '、', new int[2] { 546, 336 } }, { '~',  new int[2] { 479, 336 } }, 
            { 'ア', new int[2] { 0  , 344 } }, { 'イ', new int[2] { 8  , 344 } }, 
            { 'ウ', new int[2] { 16 , 344 } }, { 'エ', new int[2] { 24 , 344 } }, 
            { 'オ', new int[2] { 32 , 344 } }, { 'カ', new int[2] { 40 , 344 } }, 
            { 'キ', new int[2] { 48 , 344 } }, { 'ク', new int[2] { 56 , 344 } }, 
            { 'ケ', new int[2] { 64 , 344 } }, { 'コ', new int[2] { 72 , 344 } }, 
            { 'サ', new int[2] { 80 , 344 } }, { 'シ', new int[2] { 88 , 344 } }, 
            { 'ス', new int[2] { 96 , 344 } }, { 'セ', new int[2] { 104, 344 } }, 
            { 'ソ', new int[2] { 112, 344 } }, { 'タ', new int[2] { 120, 344 } }, 
            { 'チ', new int[2] { 128, 344 } }, { 'ツ', new int[2] { 136, 344 } }, 
            { 'テ', new int[2] { 144, 344 } }, { 'ト', new int[2] { 152, 344 } }, 
            { 'ナ', new int[2] { 160, 344 } }, { 'ニ', new int[2] { 168, 344 } }, 
            { 'ヌ', new int[2] { 176, 344 } }, { 'ネ', new int[2] { 184, 344 } }, 
            { 'ノ', new int[2] { 192, 344 } }, { 'ハ', new int[2] { 200, 344 } }, 
            { 'ヒ', new int[2] { 208, 344 } }, { 'フ', new int[2] { 216, 344 } }, 
            { 'ヘ', new int[2] { 224, 344 } }, { 'ホ', new int[2] { 232, 344 } }, 
            { 'マ', new int[2] { 240, 344 } }, { 'ミ', new int[2] { 248, 344 } }, 
            { 'ム', new int[2] { 256, 344 } }, { 'メ', new int[2] { 264, 344 } }, 
            { 'モ', new int[2] { 272, 344 } }, { 'ヤ', new int[2] { 280, 344 } }, 
            { 'ユ', new int[2] { 288, 344 } }, { 'ヨ', new int[2] { 296, 344 } }, 
            { 'ラ', new int[2] { 304, 344 } }, { 'リ', new int[2] { 312, 344 } }, 
            { 'ル', new int[2] { 320, 344 } }, { 'レ', new int[2] { 328, 344 } },
            { 'ロ', new int[2] { 336, 344 } }, { 'ワ', new int[2] { 344, 344 } }, 
            { 'ヲ', new int[2] { 352, 344 } }, { 'ン', new int[2] { 360, 344 } }, 
            { 'ャ', new int[2] { 368, 344 } }, { 'ュ', new int[2] { 376, 344 } }, 
            { 'ョ', new int[2] { 384, 344 } },

        };

        public Sprite(char Letter, double locationX, double locationY)
        {
            // 8x8 sprite
            BMP = new Bitmap(8, 8);

            CharValue = Letter;

            // get all the pixel from the 8x8 area (just like wht the SNES GPU does)
            for (int y = 0; y < 8; ++y)
            {
                for (int x = 0; x < 8; ++x)
                {
                    try { BMP.SetPixel(x, y, refPIC.GetPixel((BitmapAreaDico[Letter][0] + x), (BitmapAreaDico[Letter][1] + y))); }
                    catch (Exception) { /* nothing :P */ }
                }
            }
            // convert it to a suitable format that the System.Windows.Controls.Image.Source supports
            ImageSource PICsource = BMP_SPRITE.BmpToIMG(BMP);

            // defining the custom bitmap as the resource!
            IMG.Source = PICsource;

            // resizing it (x2)
            IMG.Width = 16;
            IMG.Height = 16;

            // setting the position inside the canvas
            Canvas.SetLeft(IMG, locationX);
            Canvas.SetTop(IMG, locationY);

        }

        public void ChangeSprite(char L)
        {
            // get all the pixel from the 8x8 area (just like wht the SNES VMEM does)
            for (int y = 0; y < 8; ++y)
            {
                for (int x = 0; x < 8; ++x)
                {
                    try { BMP.SetPixel(x, y, refPIC.GetPixel((BitmapAreaDico[L][0] + x), (BitmapAreaDico[L][1] + y))); }
                    catch (Exception) { /* nothing :P */ }
                }
            }
            // convert it to a suitable format that the System.Windows.Controls.Image.Source supports
            ImageSource PICsource = BMP_SPRITE.BmpToIMG(BMP);

            // define the image
            IMG.Source = PICsource;

            // change the char value
            CharValue = L;
        }

        public void AddToCanvas(Canvas C)
        {
            C.Children.Add(IMG);
        }
    }
}
