using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aladdin_SNES_Text_Changer
{
    class GameString
    {
        private int Offset, Length;
        private string Value;

        private static readonly Dictionary<byte, char> AladdinCharDataComputing = new Dictionary<byte, char>()
        {
            { 0x10, '©' }, { 0x20, ' ' },
            { 0x21, '$' }, { 0x23, '\''},
            { 0x24, '!' }, { 0x25, '?' },
            { 0x26, ';' }, { 0x27, ':' },
            { 0x28, ',' }, { 0x29, '.' },
            { 0x2a, '"' }, { 0x2b, '(' },
            { 0x2c, ')' }, { 0x2d, '*' },
            { 0x2e, '-' }, { 0x2f, '=' },
            { 0x30, '0' }, { 0x31, '1' },
            { 0x32, '2' }, { 0x33, '3' },
            { 0x34, '4' }, { 0x35, '5' },
            { 0x36, '6' }, { 0x37, '7' },
            { 0x38, '8' }, { 0x39, '9' },
            { 0x41, 'A' }, { 0x42, 'B' },
            { 0x43, 'C' }, { 0x44, 'D' },
            { 0x45, 'E' }, { 0x46, 'F' },
            { 0x47, 'G' }, { 0x48, 'H' },
            { 0x49, 'I' }, { 0x4a, 'J' },
            { 0x4b, 'K' }, { 0x4c, 'L' },
            { 0x4d, 'M' }, { 0x4e, 'N' },
            { 0x4f, 'O' }, { 0x50, 'P' },
            { 0x51, 'Q' }, { 0x52, 'R' },
            { 0x53, 'S' }, { 0x54, 'T' },
            { 0x55, 'U' }, { 0x56, 'V' },
            { 0x57, 'W' }, { 0x58, 'X' },
            { 0x59, 'Y' }, { 0x5a, 'Z' },

            // added those since they left all the Japanese characters for unknown reasons
            // probably for making the localisation process faster
            { 0x80, 'あ' }, { 0x81, 'い' },
            { 0x82, 'う' }, { 0x83, 'え' },
            { 0x84, 'お' }, { 0x85, 'か' },
            { 0x86, 'き' }, { 0x87, 'く' },
            { 0x88, 'け' }, { 0x89, 'こ' },
            { 0x8a, 'さ' }, { 0x8b, 'し' },
            { 0x8c, 'す' }, { 0x8d, 'せ' },
            { 0x8e, 'そ' }, { 0x8f, 'た' },
            { 0x90, 'ち' }, { 0x91, 'つ' },
            { 0x92, 'て' }, { 0x93, 'と' },
            { 0x94, 'な' }, { 0x95, 'に' },
            { 0x96, 'ぬ' }, { 0x97, 'ね' },
            { 0x98, 'の' }, { 0x99, 'は' },
            { 0x9a, 'ひ' }, { 0x9b, 'ふ' },
            { 0x9c, 'へ' }, { 0x9d, 'ほ' },
            { 0x9e, 'ま' }, { 0x9f, 'み' },
            { 0xa0, 'む' }, { 0xa1, 'め' },
            { 0xa2, 'も' }, { 0xa3, 'や' },
            { 0xa4, 'ゆ' }, { 0xa5, 'よ' },
            { 0xa6, 'ら' }, { 0xa7, 'り' },
            { 0xa8, 'る' }, { 0xa9, 'れ' },
            { 0xaa, 'ろ' }, { 0xab, 'わ' },
            { 0xac, 'を' }, { 0xad, 'ん' },
            { 0xb4, 'ゃ' }, { 0xb5, 'ゅ' },
            { 0xb6, 'ょ' }, { 0xb8, '。' },
            { 0xbc, '~'  }, { 0xc0, 'ア' },
            { 0xc1, 'イ' }, { 0xc2, 'ウ' },
            { 0xc3, 'エ' }, { 0xc4, 'オ' },
            { 0xc5, 'カ' }, { 0xc6, 'キ' },
            { 0xc7, 'ク' }, { 0xc8, 'ケ' },
            { 0xc9, 'コ' }, { 0xca, 'サ' },
            { 0xcb, 'シ' }, { 0xcc, 'ス' },
            { 0xcd, 'セ' }, { 0xce, 'ソ' },
            { 0xcf, 'タ' }, { 0xd0, 'チ' },
            { 0xd1, 'ツ' }, { 0xd2, 'テ' },
            { 0xd3, 'ト' }, { 0xd4, 'ナ' },
            { 0xd5, 'ニ' }, { 0xd6, 'ヌ' },
            { 0xd7, 'ネ' }, { 0xd8, 'ノ' },
            { 0xd9, 'ハ' }, { 0xda, 'ヒ' },
            { 0xdb, 'フ' }, { 0xdc, 'ヘ' },
            { 0xdd, 'ホ' }, { 0xde, 'マ' },
            { 0xdf, 'ミ' }, { 0xe0, 'ム' },
            { 0xe1, 'メ' }, { 0xe2, 'モ' },
            { 0xe3, 'ヤ' }, { 0xe4, 'ユ' },
            { 0xe5, 'ヨ' }, { 0xe6, 'ラ' },
            { 0xe7, 'リ' }, { 0xe8, 'ル' },
            { 0xe9, 'レ' }, { 0xea, 'ロ' },
            { 0xeb, 'ワ' }, { 0xec, 'ヲ' },
            { 0xed, 'ン' }, { 0xee, 'ャ' },
            { 0xf5, 'ュ' }, { 0xf6, 'ョ' },
        };

        // some getter/setters
        public string GetValue  { get { return Value;  } }
        public int GetOffset    { get { return Offset; } }
        public int GetLength    { get { return Length; } }
        public string SetValue  { set { Value = value; } }

        public void SetCharValue(int Index, char Lettre)
        {
            // convert the string to char array
            char[] T = Value.ToCharArray();

            // changing the wanted char
            T[Index] = Lettre;

            // convert it back to string
            SetValue = new string(T);
        }

        // constructor
        public GameString(int _Offset, int L)
        {
            Offset = _Offset;
            Length = L;

            for (int i = Offset; i < (Offset + L); ++i) 
            {
                try { Value += AladdinCharDataComputing[MainWindow.GameData[i]]; }
                catch (Exception) { Value += "|"; }
            }
        }

        // destructor
        ~GameString()
        {
            AladdinCharDataComputing.Clear();
        }
    }
}
