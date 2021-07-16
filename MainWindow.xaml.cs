using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32; // OpenFileDialog/SaveFileDialog
using System.IO; // File class
using System.Timers; // for the Timer class
using System.Media;
using System.Diagnostics;
using System.Reflection;

namespace Aladdin_SNES_Text_Changer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 

    /*****************************************/
    /* MADE BY OGAN OEZKUL AKA RYUGUU - CHAN */
    /*****************************************/

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public static byte[]    GameData;
        private List<Sprite>    SpriteList                  = new List<Sprite>();
        private Rectangle       TextRect;
        private int             CharIndex                   = 0;
        private int             TextCursorLine              = 0;
        private int             GameSymbolCharsArrayIndex   = 0;
        private int             SelectedGameDialogs         = 0;
        private bool            TextLoaded                  = false;
        private double          TextSelectionXloc, TextSelectionYloc;

        public static Dictionary<char, byte> AladdinCharData = new Dictionary<char, byte>()
        {
            { '©' , 0x10 }, { ' ' , 0x20 },
            { '$' , 0x21 }, { '\'', 0x23 },
            { '!' , 0x24 }, { '?' , 0x25 },
            { ';' , 0x26 }, { ':' , 0x27 },
            { ',' , 0x28 }, { '.' , 0x29 },
            { '"' , 0x2a }, { '(' , 0x2b },
            { ')' , 0x2c }, { '*' , 0x2d },
            { '-' , 0x2e }, { '=' , 0x2f },
            { '0' , 0x30 }, { '1' , 0x31 },
            { '2' , 0x32 }, { '3' , 0x33 },
            { '4' , 0x34 }, { '5' , 0x35 },
            { '6' , 0x36 }, { '7' , 0x37 },
            { '8' , 0x38 }, { '9' , 0x39 },
            { 'A' , 0x41 }, { 'B' , 0x42 }, 
            { 'C' , 0x43 }, { 'D' , 0x44 }, 
            { 'E' , 0x45 }, { 'F' , 0x46 }, 
            { 'G' , 0x47 }, { 'H' , 0x48 }, 
            { 'I' , 0x49 }, { 'J' , 0x4a }, 
            { 'K' , 0x4b }, { 'L' , 0x4c }, 
            { 'M' , 0x4d }, { 'N' , 0x4e }, 
            { 'O' , 0x4f }, { 'P' , 0x50 }, 
            { 'Q' , 0x51 }, { 'R' , 0x52 }, 
            { 'S' , 0x53 }, { 'T' , 0x54 }, 
            { 'U' , 0x55 }, { 'V' , 0x56 }, 
            { 'W' , 0x57 }, { 'X' , 0x58 }, 
            { 'Y' , 0x59 }, { 'Z' , 0x5a },

            // added those since they left all the Japanese characters for unknown reasons
            // probably for making the localisation process faster
            { 'あ', 0x80 }, { 'い', 0x81 },
            { 'う', 0x82 }, { 'え', 0x83 },
            { 'お', 0x84 }, { 'か', 0x85 },
            { 'き', 0x86 }, { 'く', 0x87 },
            { 'け', 0x88 }, { 'こ', 0x89 },
            { 'さ', 0x8a }, { 'し', 0x8b },
            { 'す', 0x8c }, { 'せ', 0x8d },
            { 'そ', 0x8e }, { 'た', 0x8f },
            { 'ち', 0x90 }, { 'つ', 0x91 },
            { 'て', 0x92 }, { 'と', 0x93 },
            { 'な', 0x94 }, { 'に', 0x95 },
            { 'ぬ', 0x96 }, { 'ね', 0x97 },
            { 'の', 0x98 }, { 'は', 0x99 },
            { 'ひ', 0x9a }, { 'ふ', 0x9b },
            { 'へ', 0x9c }, { 'ほ', 0x9d },
            { 'ま', 0x9e }, { 'み', 0x9f },
            { 'む', 0xa0 }, { 'め', 0xa1 },
            { 'も', 0xa2 }, { 'や', 0xa3 },
            { 'ゆ', 0xa4 }, { 'よ', 0xa5 },
            { 'ら', 0xa6 }, { 'り', 0xa7 },
            { 'る', 0xa8 }, { 'れ', 0xa9 },
            { 'ろ', 0xaa }, { 'わ', 0xab },
            { 'を', 0xac }, { 'ん', 0xad },
            { 'ゃ', 0xb4 }, { 'ゅ', 0xb5 },
            { 'ょ', 0xb6 }, { '。', 0xb8 },
            { '~',  0xbc }, { 'ア', 0xc0 },
            { 'イ', 0xc1 }, { 'ウ', 0xc2 },
            { 'エ', 0xc3 }, { 'オ', 0xc4 },
            { 'カ', 0xc4 }, { 'キ', 0xc6 },
            { 'ク', 0xc7 }, { 'ケ', 0xc8 },
            { 'コ', 0xc9 }, { 'サ', 0xca },
            { 'シ', 0xcb }, { 'ス', 0xcc },
            { 'セ', 0xcd }, { 'ソ', 0xce },
            { 'タ', 0xcf }, { 'チ', 0xd0 },
            { 'ツ', 0xd1 }, { 'テ', 0xd2 },
            { 'ト', 0xd3 }, { 'ナ', 0xd4 },
            { 'ニ', 0xd5 }, { 'ヌ', 0xd6 },
            { 'ネ', 0xd7 }, { 'ノ', 0xd8 },
            { 'ハ', 0xd9 }, { 'ヒ', 0xda },
            { 'フ', 0xdb }, { 'ヘ', 0xdc },
            { 'ホ', 0xdd }, { 'マ', 0xde },
            { 'ミ', 0xdf }, { 'ム', 0xe0 },
            { 'メ', 0xe1 }, { 'モ', 0xe2 },
            { 'ヤ', 0xe3 }, { 'ユ', 0xe4 },
            { 'ヨ', 0xe5 }, { 'ラ', 0xe6 },
            { 'リ', 0xe7 }, { 'ル', 0xe8 },
            { 'レ', 0xe9 }, { 'ロ', 0xea },
            { 'ワ', 0xeb }, { 'ヲ', 0xec },
            { 'ン', 0xed }, { 'ャ', 0xee },
            { 'ュ', 0xf5 }, { 'ョ', 0xf6 },
        };

        // same goes for all the key inputs
        private Dictionary<Key, char> AlladdinCharKeys = new Dictionary<Key, char>()
        {
            { Key.D0, '0' }, { Key.D1 , '1' },
            { Key.D2, '2' }, { Key.D3 , '3' },
            { Key.D4, '4' }, { Key.D5 , '5' },
            { Key.D6, '6' }, { Key.D7 , '7' },
            { Key.D8, '8' }, { Key.D9 , '9' },

            { Key.NumPad0, '0' }, { Key.NumPad1 , '1' },
            { Key.NumPad2, '2' }, { Key.NumPad3 , '3' },
            { Key.NumPad4, '4' }, { Key.NumPad5 , '5' },
            { Key.NumPad6, '6' }, { Key.NumPad7 , '7' },
            { Key.NumPad8, '8' }, { Key.NumPad9 , '9' },

            { Key.A , 'A' }, { Key.B , 'B' },
            { Key.C , 'C' }, { Key.D , 'D' },
            { Key.E , 'E' }, { Key.F , 'F' },
            { Key.G , 'G' }, { Key.H , 'H' },
            { Key.I , 'I' }, { Key.J , 'J' },
            { Key.K , 'K' }, { Key.L , 'L' },
            { Key.M , 'M' }, { Key.N , 'N' },
            { Key.O , 'O' }, { Key.P , 'P' },
            { Key.Q , 'Q' }, { Key.R , 'R' },
            { Key.S , 'S' }, { Key.T , 'T' },
            { Key.U , 'U' }, { Key.V , 'V' },
            { Key.W , 'W' }, { Key.X , 'X' },
            { Key.Y , 'Y' }, { Key.Z , 'Z' },

            { Key.Space, ' ' }
        };

        private char[] GameSymbolCharsArray =
        {
            '©', ' ', '$', '\\', '!', '?', ';', ':',
            ',', '.', '"', '(', ')', '*', '-', '='
        };

        GameString[]    _GameDialogs    = new GameString[87];
        List<int>       CharsInLineList = new List<int>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void GameTextComnboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GameSymbolCharsArrayIndex = 0;

            CharIndex = 0;


            if (TextRect == null)
            {
                TextRect = new Rectangle();
                TextRect.Fill = Brushes.White;
                TextRect.Width = 16;
                TextRect.Height = 2;
                SelectionTextCanvas.Children.Add(TextRect);
            }


            if (TextLoaded == true)
                GameTextComnboBox.Items[SelectedGameDialogs] = _GameDialogs[SelectedGameDialogs].GetValue;

            int CharAmountInLine = 0;

            double Xloc = 0;
            double Yloc = 30;

            TextSelectionXloc = Xloc;
            TextSelectionYloc = Yloc+16;

            Canvas.SetLeft(TextRect, TextSelectionXloc);
            Canvas.SetTop(TextRect, TextSelectionYloc);

            GameTextCanvas.Children.Clear();

            SpriteList.Clear();

            foreach (char T in _GameDialogs[GameTextComnboBox.SelectedIndex].GetValue)
            {
                if (T != '|')
                {
                    // adding it for avoid sprite char generation issues
                    if (T != '_')
                    {
                        SpriteList.Add(new Sprite(T, Xloc, Yloc));
                        ++CharAmountInLine;
                        Xloc += 16;
                    }
                    else
                    {
                        SpriteList.Add(new Sprite(T, Xloc, Yloc));
                        CharsInLineList.Add(CharAmountInLine - 1);
                        CharAmountInLine = 0;
                        Yloc += 30;
                        Xloc = 0;
                    }
                }
                else
                {
                    SpriteList.Add(new Sprite(T, Xloc, Yloc));
                    CharsInLineList.Add(CharAmountInLine - 1);
                    CharAmountInLine = 0;
                    Yloc += 30;
                    Xloc = 0;
                }
                SpriteList.Last().AddToCanvas(GameTextCanvas);
            }

            for (int x = 0; x < GameSymbolCharsArray.Length; ++x)
            {
                try { if (SpriteList[CharIndex].GetCharValue == GameSymbolCharsArray[x]) { GameSymbolCharsArrayIndex = x; goto FinalCharLoad; } }
                catch (Exception) { }
            }

            GameSymbolCharsArrayIndex = 0;

            FinalCharLoad:

            // for avoid various issues
            this.Focus();

            SelectedGameDialogs = GameTextComnboBox.SelectedIndex;

            TextLoaded = true;

            SaveFileButton.IsEnabled = true;
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Donate();
        }

        private void Donate()
        {
            Process.Start("https://www.buymeacoffee.com/ryuguuchan"); // support me
        }

        private void SaveFile()
        {
            int ii = 0;
            foreach (GameString GS in _GameDialogs)
            {
                ii = 0;
                for (int i = 0; i < GS.GetValue.Length; i++)
                {
                    // since these strings are computed in different way than others
                    // for some reason ...
                    if (GS.GetValue[i] != '|')
                    {
                        // for the strings that are separated with 00 88 05 20 or 00 08 06 20
                        if (GS.GetValue[i] == '_')
                        {
                            ii += 3;
                        }
                        else
                        {
                            GameData[(GS.GetOffset + i + ii)] = AladdinCharData[GS.GetValue[i]];
                        }
                    }
                }
            }

            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "SNES ROM (*.sfc)|*.sfc";
            if (SFD.ShowDialog() == true)
            {
                File.WriteAllBytes(SFD.FileName, GameData);
                MessageBox.Show("File Saved!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenFile()
        {
            OpenFileDialog OPFD = new OpenFileDialog();
            OPFD.Filter = "SNES ROM (*.sfc)|*.sfc";

            // yea ik its lazy but it works :P
            A:
            if (OPFD.ShowDialog() == true)
            {
                GameData = File.ReadAllBytes(OPFD.FileName);

                // checking the ROM's footer
                // its a easy check for the momment
                // but the check will be more strict in the future versions
                try
                {
                    if (GameData[0x13e000] != 0xaa)
                    {
                        MessageBox.Show("WRONG ROM!", "Wrong ROM", MessageBoxButton.OK, MessageBoxImage.Error);
                        goto A;
                    }
                    else
                    {
                        GameTextComnboBox.Items.Clear();

                        // reset the whole canvas
                        GameTextCanvas.Children.Clear();

                        // looking at the strings of the "wellcome to agrabah!"
                        try
                        {
                            // all the FF will acordingly be interpreted as the '\n'
                            // it's also the case with the 4 following bytes 00 88 05 20
                            _GameDialogs[00] = new GameString(0x12f60e, 0x37);  // WELCOME TO AGRABAH! CITY OF MYSTERY, OF ENCHANTMENT, OF
                            _GameDialogs[01] = new GameString(0x12f646, 0x37);  // THE FINEST MERCHANDISE THIS SIDE OF THE RIVER JORDAN...
                            _GameDialogs[02] = new GameString(0x12f67e, 0x43);  // I CAN SEE THAT YOU ARE ONLY INTERESTED IN THE EXCEPCTIONALLY RARE...
                            _GameDialogs[03] = new GameString(0x12f6c2, 0x3e);  // I THINK, THEN, YOU WOULD BE MOST REWARDED TO CONSIDER... THIS!
                            _GameDialogs[04] = new GameString(0x12f701, 0x2f);  // LIKE SO MANY THINGS, IT IS NOT WHAT IS OUTSIDE,
                            _GameDialogs[05] = new GameString(0x12f731, 0x1f);  // BUT WHAT IS INSIDE THAT COUNTS.
                            _GameDialogs[06] = new GameString(0x12f751, 0x3a);  // THIS LAMP ONCE CHANGED THE COURSE OF A YOUNG MAN'S LIFE...
                            _GameDialogs[07] = new GameString(0x12f791, 0x2a);  // AFTER ALL MY YEARS OF SEARCHING -- AT LAST,
                            _GameDialogs[08] = new GameString(0x12f7bd, 0x28);  // THE CAVE OF WONDERS! ... NOW REMEMBER ...
                            _GameDialogs[09] = new GameString(0x12f7d2, 0x13);  // ... NOW REMEMBER ...
                            _GameDialogs[10] = new GameString(0x12f7e6, 0x3a);  // THE REST OF THE TREASURE IS YOURS -- BUT THE LAMP IS MINE!
                            _GameDialogs[11] = new GameString(0x12f827, 0x19);  // WHO DISTURBS MY SLUMBER!?
                            _GameDialogs[12] = new GameString(0x12f846, 0X21);  // IT IS I, GAZEEM, AM HUMBLE THIEF.
                            _GameDialogs[13] = new GameString(0x12f86d, 0x23);  // KNOW THIS! ONLY ONE MAY ENTER HERE.
                            _GameDialogs[14] = new GameString(0x12f891, 0x23);  // ONE WHOSE WORTH LIES DEEP WITHIN...
                            _GameDialogs[15] = new GameString(0x12f8b5, 0x19);  // THE DIAMOND IN THE ROUGH!
                            _GameDialogs[16] = new GameString(0x12f8d4, 0x0b);  // OH NO!!!!!!
                            _GameDialogs[17] = new GameString(0x12f8e6, 0x33);  // SO... ONLY ONE MAY ENTER... I MUST FIND THIS ONE...
                            _GameDialogs[18] = new GameString(0x12f91a, 0x1d);  // ... THIS DIAMOND IN THE ROUGH.
                            _GameDialogs[19] = new GameString(0x73ce, 0x42);    // TOGETHER, ALADdIN AND JASMINE ESCAPE THE SULTAN'S GUARDS AND
                            _GameDialogs[20] = new GameString(0x7417, 0x2c);    // RUN TO ALADDIN'S HOME HIGH ABOVE AGRABAH.
                            _GameDialogs[21] = new GameString(0x744a, 0x48);    // THERE THEY ENJOY THE VIEW AND DISCUSS THEIR DREAMS FOR THE FUTURE.
                            _GameDialogs[22] = new GameString(0x7499, 0x4e);    // UNFORTUNATELY, THEY ARE FOUND BY THE GUARDS, WHO DRAG ALADDIN OFF TO THE
                            _GameDialogs[23] = new GameString(0x74ee, 0x40);    // PALACE DUNGEON AND JASMINE BACK TO HER FATHER, THE SULTAN.
                            _GameDialogs[24] = new GameString(0x7535, 0x3d);    // ALADDIN IS THROWN INTO THE DEEPEST CELL OF THE DUNGEON,
                            _GameDialogs[25] = new GameString(0x7579, 0x3b);    // BUT ALL HE CAN THINK ABOUT IS THE BEAUTIFUL PRINCESS.
                            _GameDialogs[26] = new GameString(0x75bb, 0x46);    // A STRANGE OLD MAN APPEARS, OFFERING TO SET ALADDIN FREE IF HE'LL
                            _GameDialogs[27] = new GameString(0x7608, 0x2f);    // COME ON AN ADVENTURE TO THE CAVE OF WONDERS.
                            _GameDialogs[28] = new GameString(0x763e, 0x49);    // GLAD TO GET OUT OF PRISON, ALADDIN DOESN'T SUSPECT THAT THE OLD MAN
                            _GameDialogs[29] = new GameString(0x768e, 0x21);    // IS ACTUALLY JAFAR IN DISGUISE!
                            _GameDialogs[30] = new GameString(0x76b6, 0x42);    // AS SOON AS ABU TOUCHES THE GEM, THE CANVE BEGINS TO COLLAPSE
                            _GameDialogs[31] = new GameString(0x76ff, 0x2d);    // THEY'RE GOING TO HAVE TO GET OUT OG THERE!
                            _GameDialogs[32] = new GameString(0x7733, 0x3a);    // ALADDIN AND ABU ESCAPE AND DELIVER THE LAMP TO JAFAR
                            _GameDialogs[33] = new GameString(0x7775, 0x36);    // BUT HE TRICKS THEM AND SEALS THEM INTO THE CAVE
                            _GameDialogs[34] = new GameString(0x77b2, 0x3a);    // FORTUNATELY, ABU CLEVERLY STOLE THE LAMP FROM JAFAR
                            _GameDialogs[35] = new GameString(0x77f3, 0x4c);    // IN ORDER TO READ THE WRITTING ON THE OLD LAMP, ALADDIN RUBS IT WITH HIS
                            _GameDialogs[36] = new GameString(0x7846, 0x10);    // SLEEVE AND POOF!
                            _GameDialogs[37] = new GameString(0x785d, 0x45);    // A GENIE APPEARS -- AND DECLARES THAT ALADDIN IS HIS NEW MASTER.
                            _GameDialogs[38] = new GameString(0x78a9, 0x31);    // BUT FIRST, HE INVITES ALADDIN INTO THE LAMP...
                            _GameDialogs[39] = new GameString(0x78e1, 0x46);    // WITH THE HELP OF THE GENIE AND THE MAGIC CARPET, ALADDIN ESCAPES
                            _GameDialogs[40] = new GameString(0x792e, 0x3e);    // FROM THE CAVE OF WONDERS AND FLIES BACK TOWARDS AGRABAH.
                            _GameDialogs[41] = new GameString(0x7973, 0x47);    // UNFORTUNATELY, ABU CANNOT SIT STILL AND AS HE SCAMPERS ACROSS THE
                            _GameDialogs[42] = new GameString(0x79c1, 0x31);    // CARPET, IS BLOWN OVERBOARD INTO THE DESERT.
                            _GameDialogs[43] = new GameString(0x79f9, 0x49);    // WHEN ALADIN SWOOPS DONW TO RESCUE THE PLAYFUL MONKEY, HE DISCOVERS
                            _GameDialogs[44] = new GameString(0x7a49, 0x44);    // THAT ABU HAS DISAPPEARED INTO THE RUINS OF A ANCIENT PYRAMID.
                            _GameDialogs[45] = new GameString(0x7a94, 0x3f);    // ALADDIN DECIDES TO ENTER THE PYRAMID AND FIND HIS FRIEND.
                            _GameDialogs[46] = new GameString(0x7ada, 0x4a);    // USING ONE OF HIS THREE WISHES, ALADDIN HAS THE GENIE TURN HIM INTO A
                            _GameDialogs[47] = new GameString(0x7b2b, 0x33);    // PRINCE. OUTFITTED WITH A MAGNIFICENT CARAVAN,
                            _GameDialogs[48] = new GameString(0x7b65, 0x3a);    // ALADDIN, NOW PRINCE ALI ABABWAH, RETURNS TO AGRABAH.
                            _GameDialogs[49] = new GameString(0x7ba6, 0x45);    // AS A PRINCE, HE CAN NOW TRY AND WIN THE HAND OF THE PRINCESS IN /* OUT OF RANGE */
                            _GameDialogs[50] = new GameString(0x7bf2, 0x09);    // MARRIAGE.
                            _GameDialogs[51] = new GameString(0x7c02, 0x49);    // JUST WHEN THINGS ARE GOING GREAT FOR ALADDIN, IAGO STEALS THE LAMP!
                            _GameDialogs[52] = new GameString(0x7c52, 0x4d);    // JAFAR, WHO IS NOW MASTER OF THE LAMP, USES HIS NEW MAGIC POWERS TO SEND
                            _GameDialogs[53] = new GameString(0x7ca6, 0x28);    // ALADDIN OFF TO THE ENDS OF THE EARTH.
                            _GameDialogs[54] = new GameString(0x7cd5, 0x4c);    // ONCE MORE, WITH THE HELP OF FAITHFUL ABU AND THE MAGIC CARPET, ALADDIN
                            _GameDialogs[55] = new GameString(0x7d28, 0x49);    // RETURNS TO AGRABAH, READY TO DO BATTLE WITH THE EVIL SORCERER JAFAR
                            _GameDialogs[56] = new GameString(0x7d78, 0x11);    // AND SAVE JASMINE!
                            _GameDialogs[57] = new GameString(0x12f93d, 0x3e);  // YOU THOUGHT YOU COULD OUTWIT THE MOST POWERFUL BEING ON EARTH?
                            _GameDialogs[58] = new GameString(0x12f981, 0x2b);  // YOU'RE NOT AS POWERFUL AS YOU THINK, JAFAR.
                            _GameDialogs[59] = new GameString(0x12f9ad, 0x2f);  // THE GENIE HAS MORE POWER THAN YOU'LL EVER HAVE!
                            _GameDialogs[60] = new GameString(0x12f9e2, 0x28);  // HIS POWER WILL NOT EXCEED MINE FOR LONG
                            _GameDialogs[61] = new GameString(0x12fa0b, 0x2a);  // GENIE. HERE IS MY THIRD WISH -- MAKE ME AN
                            _GameDialogs[62] = new GameString(0x12fa36, 0x14);  // ALL POWERFUL GENIE!!
                            _GameDialogs[63] = new GameString(0x12fa50, 0x32);  // I HAVE NO CHOICE, YOUR WISH IS MY COMMAND. MASTER.
                            _GameDialogs[64] = new GameString(0x12fa88, 0x28);  // YES! YES! I CAN FEEL THE ABSOLUTE POWER!
                            _GameDialogs[65] = new GameString(0x12fab1, 0x20);  // THE UNIVERSE IS MINE TO COMMAND!
                            _GameDialogs[66] = new GameString(0x12fad7, 0x20);  // WAIT!! WHAT IS HAPPENING TO ME!?
                            _GameDialogs[67] = new GameString(0x12fafd, 0x39);  // JASMINE, I'M SORRY I LIED TO YOU... ABOUT BEING A PRINCE.
                            _GameDialogs[68] = new GameString(0x12fb3c, 0x33);  // I KNOW YOU HAD TO DO IT BECAUSE OF THAT STUPID LAW!
                            _GameDialogs[69] = new GameString(0x12fb70, 0x16);  // BUT I LOVE YOU ANYWAY.
                            _GameDialogs[70] = new GameString(0x12fb8c, 0x32);  // NO PROBLEM USE YOUR THIRD WISH. MASTER, AND ZAP --
                            _GameDialogs[71] = new GameString(0x12fbbf, 0x16);  // YOU'RE A PRINCE AGAIN!
                            _GameDialogs[72] = new GameString(0x12fbdb, 0x45);  // JASMINE, I LOVE YOU TOO. BUT I CAN'T PRETEND TO BE SOMETHING I'M NOT.
                            _GameDialogs[73] = new GameString(0x12fc26, 0x0d);  // I UNDERSTAND.
                            _GameDialogs[74] = new GameString(0x12fc39, 0x35);  // GENIE, MY THIRD AND FINAL WISH IS FOR... YOUR FREEDOM!
                            _GameDialogs[75] = new GameString(0x12fc74, 0x1c);  // WOW! I'M FREE... I'M FREE!!!
                            _GameDialogs[76] = new GameString(0x12fc91, 0x40);  // OH, DOES THAT FEEK GOOD! TIME TO HIT THE ROAD AND SEE THE WORLD.
                            _GameDialogs[77] = new GameString(0x12fcd7, 0x25);  // GOSH, GENIE. I'M SURE GONNA MISS YOU.
                            _GameDialogs[78] = new GameString(0x12fd02, 0x27);  // ME TOO, AL. NO MATTER WHAT ANYONE SAYS .
                            _GameDialogs[79] = new GameString(0x12fd2a, 0x20);  // YOU'LL ALWAYS BE A PRINCE TO ME.
                            _GameDialogs[80] = new GameString(0x12fd50, 0x23);  // HMM, LOOKS LIEK WE NED A NEW LAW.
                            _GameDialogs[81] = new GameString(0x12fd74, 0x43);  // FROM THIS DAY FORWARD, THE PRINCESS MAY MARRY WHOMEVER SHE CHOOSES!
                            _GameDialogs[82] = new GameString(0x12fdbf, 0x12);  // THANK YOU, FATHER!
                            _GameDialogs[83] = new GameString(0x12fdd7, 0x18);  // I CHOOSE HIM -- ALADDIN! 
                            _GameDialogs[84] = new GameString(0x12fdf7, 0x10);  // JUST CALL ME AL.
                            _GameDialogs[85] = new GameString(0x12fe0d, 0x39);  // THIS IS WONDERFUL! COME OVER HERE, GUYS... BUG GROUP HUG!
                            _GameDialogs[86] = new GameString(0x12fe47, 0x36);  // AND NOW. I'M OUTTA HERE! LOOK OUT WORLD. HERE I COME!!


                            // reading all the things and adding them to the combobox!
                            foreach (GameString GS in _GameDialogs)
                            {
                                GameTextComnboBox.Items.Add(GS.GetValue);
                            }
                            SaveFileButton.IsEnabled = true;
                        }
                        catch (Exception E)
                        {
                            //throw E;
                            MessageBox.Show(E.ToString());
                        }

                    }

                    if (TextRect != null)
                    {
                        Canvas.SetLeft(TextRect, 0);
                        Canvas.SetTop(TextRect, 16);
                    }

                    TextSelectionXloc = 0;
                    TextSelectionYloc = 16;

                    
                    OpenFileButton.IsEnabled = false;
                }
                catch (IndexOutOfRangeException E)
                {
                    //MessageBox.Show("WRONG ROM!", "Wrong ROM", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show(E.ToString());
                    goto A;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // go on twitter
            Process.Start("https://twitter.com/ChanRyuguu");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // go on github
            Process.Start("https://github.com/Ryuguu-Chan");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // buymeacoffee thing
            Donate();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show
            (
                "Name\t"    + Assembly.GetEntryAssembly().GetName().Name    + "\n"   +
                "Version\t" + Assembly.GetEntryAssembly().GetName().Version + "\n\n" +
                "Copyright © Ogan Özkul 2021",

                "About"
            );
        }

        private void GameTextComnboBox_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("Hello World");
        }

        // crazy code incomming (it's all for user text edition btw)
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (TextLoaded == true)
            {
                if (e.Key != Key.Tab)
                {
                    if (e.Key == Key.Right)
                    {
                        if (CharIndex + 1 < SpriteList.Count)
                        {
                            ++CharIndex;
                            if 
                            (
                                SpriteList[CharIndex].GetCharValue == '|' ||
                                SpriteList[CharIndex].GetCharValue == '_'
                            )
                            {
                                ++CharIndex;
                                ++TextCursorLine;
                                TextSelectionXloc = 0;
                                TextSelectionYloc += (16 * 2);
                            }
                            else
                            {
                                TextSelectionXloc += 16;
                            }
                        }

                        for (int x = 0; x < GameSymbolCharsArray.Length; ++x)
                        {
                            try { if (SpriteList[CharIndex].GetCharValue == GameSymbolCharsArray[x]) { GameSymbolCharsArrayIndex = x; goto FinalCharLoad; } }
                            catch (Exception) { }
                        }

                        GameSymbolCharsArrayIndex = 0;

                        FinalCharLoad:
                        Canvas.SetLeft(TextRect, TextSelectionXloc);
                        Canvas.SetTop(TextRect, TextSelectionYloc);
                    }
                    else if (e.Key == Key.Left)
                    {
                        if (CharIndex - 1 >= 0)
                        {
                            --CharIndex;
                            if 
                            (
                                SpriteList[CharIndex].GetCharValue == '|' ||
                                SpriteList[CharIndex].GetCharValue == '_'
                            )
                            {
                                --CharIndex;
                                --TextCursorLine;
                                TextSelectionYloc -= (16 * 2);
                                TextSelectionXloc = (CharsInLineList[TextCursorLine] * 16);
                            }
                            else
                            {
                                TextSelectionXloc -= 16;
                            }

                            for (int x = 0; x < GameSymbolCharsArray.Length; ++x)
                            {
                                try { if (SpriteList[CharIndex].GetCharValue == GameSymbolCharsArray[x]) { GameSymbolCharsArrayIndex = x; goto FinalCharLoad; } }
                                catch (Exception) { }
                            }

                            GameSymbolCharsArrayIndex = 0;

                            FinalCharLoad:
                            Canvas.SetLeft(TextRect, TextSelectionXloc);
                            Canvas.SetTop(TextRect, TextSelectionYloc);
                        }
                    }
                    else if (e.Key == Key.Up)
                    {
                        // a voir ce qui ne va pas
                        if (GameSymbolCharsArrayIndex - 1 < 0)
                            GameSymbolCharsArrayIndex = GameSymbolCharsArray.Length - 1;

                        --GameSymbolCharsArrayIndex;
                        SpriteList[CharIndex].ChangeSprite(GameSymbolCharsArray[GameSymbolCharsArrayIndex]);
                        _GameDialogs[SelectedGameDialogs].SetCharValue(CharIndex, GameSymbolCharsArray[GameSymbolCharsArrayIndex]);
                    }
                    else if (e.Key == Key.Down)
                    {

                        if (GameSymbolCharsArrayIndex + 1 > GameSymbolCharsArray.Length - 1)
                            GameSymbolCharsArrayIndex = 0;

                        GameSymbolCharsArrayIndex++;
                        SpriteList[CharIndex].ChangeSprite(GameSymbolCharsArray[GameSymbolCharsArrayIndex]);
                        _GameDialogs[SelectedGameDialogs].SetCharValue(CharIndex, GameSymbolCharsArray[GameSymbolCharsArrayIndex]);
                    }
                    else
                    {
                        try
                        {
                            SpriteList[CharIndex].ChangeSprite(AlladdinCharKeys[e.Key]);
                            _GameDialogs[SelectedGameDialogs].SetCharValue(CharIndex, AlladdinCharKeys[e.Key]);

                            if (CharIndex + 1 < SpriteList.Count)
                                ++CharIndex;
                            if 
                            (
                                SpriteList[CharIndex].GetCharValue == '|' ||
                                SpriteList[CharIndex].GetCharValue == '_'
                            )
                            {
                                ++CharIndex;
                                ++TextCursorLine;
                                TextSelectionXloc = 0;
                                TextSelectionYloc += (16 * 2);
                            }
                            else
                            {
                                if (CharIndex + 1 < SpriteList.Count)
                                    TextSelectionXloc += 16;
                            }

                            for (int x = 0; x < GameSymbolCharsArray.Length; ++x)
                            {
                                try { if (SpriteList[CharIndex].GetCharValue == GameSymbolCharsArray[x]) { GameSymbolCharsArrayIndex = x; goto FinalCharLoad; } }
                                catch (Exception) { }
                            }

                            GameSymbolCharsArrayIndex = 0;

                            FinalCharLoad:
                            Canvas.SetLeft(TextRect, TextSelectionXloc);
                            Canvas.SetTop(TextRect, TextSelectionYloc);

                        }
                        catch (Exception)
                        {
                            SystemSounds.Asterisk.Play();
                        }

                    }
                }
            }
        }
    }
}
