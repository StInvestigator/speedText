using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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

namespace speedText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int passesIn2Seconds = 0;
        DateTime secondsCount = DateTime.Now;
        Brush foregroundColor;
        public MainWindow()
        {
            InitializeComponent();
            CreateAllButtons(120, 120, 35);
        }

        private void CreateAllButtons(int Width, int Height, int FontSize)
        {
            CreateButtons("QWERTYUIOP[]", SP1, Width, Height, FontSize);
            CreateButtons("ASDFGHJKL;'", SP2, Width, Height, FontSize);
            CreateButtons("ZXCVBNM,.?", SP3, Width, Height, FontSize);
            var b = new Button() { Content = "SPACE", Width = Width*6, Height = Height, Focusable=false, FontSize = FontSize };
            b.Style = (Style)FindResource("MaterialDesignPaperButton");
            SP4.Children.Add(b);
            foregroundColor = b.Foreground.Clone();


        }

        private void CreateButtons(string chars, StackPanel SP, int Width, int Height, int FontSize)
        {
            foreach (var item in chars)
            {
                var b = new Button() { Content = item, Width = Width, Height = Height, Focusable = false, FontSize= FontSize };
                b.Style = (Style)FindResource("MaterialDesignPaperButton");
                SP.Children.Add(b);
            }
        }

        private void calculateProcentage()
        {
            if(Convert.ToInt32(TBPasses.Text) != 0 || Convert.ToInt32(TBFails.Text)!=0)
            TBPersentage.Text = (Convert.ToInt32(Math.Round(Convert.ToDouble(TBPasses.Text)/(Convert.ToDouble(TBFails.Text) + Convert.ToDouble(TBPasses.Text)),2)*100)).ToString() + " %";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            paintTheButtonBackground(e, new SolidColorBrush(Colors.MediumPurple), new SolidColorBrush(Colors.White));
            if (isRightKey(e))
            {
                StringBuilder stringBuilder = new StringBuilder(tbText.Text);
                stringBuilder.Remove(0, 1);
                tbText.Text = stringBuilder.ToString();
                tbText.UpdateLayout();
                randomLetterCreation();
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            paintTheButtonBackground(e, new SolidColorBrush(Colors.White), foregroundColor);
        }

        private void SliderDifficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(tbText.Text.Length > 0)
            {
                TStart.IsChecked = false;
                tbText.Text = tbText.Text.Remove(0, tbText.Text.Length);
            }
            randomLetterCreation();
        }
        private bool isStart()
        {
            return Convert.ToBoolean(TStart.IsChecked);
        }
        private bool isRightKey(KeyEventArgs e)
        {
            if (tbText.Text.Length != 0 && isStart())
            {
                char sign;
                string key = e.Key.ToString();
                if (key == "Space")
                {
                    sign = ' ';
                }
                else
                {
                    sign = char.ToLower(key[0]);
                }
                if ((DateTime.Now - secondsCount).TotalSeconds<=2)
                {
                    passesIn2Seconds++;
                }
                else
                {
                    secondsCount = DateTime.Now;
                    TBSpeed.Text = (passesIn2Seconds*30).ToString()+ " dig/min";
                    passesIn2Seconds = 0;
                }
                if (sign == tbText.Text.First())
                {
                    TBPasses.Text = (Convert.ToInt32(TBPasses.Text) + 1).ToString(); calculateProcentage();
                    return true;
                }
                else
                {
                    TBFails.Text = (Convert.ToInt32(TBFails.Text)+1).ToString(); calculateProcentage();
                }
            }
            return false;
        }
        private void randomLetterCreation()
        {
            if (tbText.Text.Length == 0)
            {
                Random rand = new Random();
                bool isSpace = false;
                int w = rand.Next(5, 8);
                int chars = rand.Next(97, 122 - (int)SliderDifficulty.Value);
                while (tbText.Text.Length != 32)
                {
                    if (isSpace && tbText.Text.Length!= 31)
                    {
                        tbText.Text += ' ';
                        w = rand.Next(5, 8);
                        isSpace = false;
                    }
                    else
                    {
                        w--;
                        tbText.Text += Convert.ToChar(rand.Next(chars, chars + (int)SliderDifficulty.Value));
                        if(w==0)
                        {
                            isSpace = true;
                        }
                    }
                }
            }
        }


        private void paintTheButtonBackground(KeyEventArgs e, Brush color, Brush color2)
        {
            try
            {
                char sign;
                string key = e.Key.ToString();
                if (key == "Space")
                {
                    (SP4.Children[0] as Button).Background = color;
                    (SP4.Children[0] as Button).Foreground = color2;
                    return;
                }
                else if (key == "OemOpenBrackets")
                {
                    sign = '[';
                }
                else if (key == "Oem6")
                {
                    sign = ']';
                }
                else if (key == "OemQuotes")
                {
                    sign = '\'';
                }
                else if (key == "Oem1")
                {
                    sign = ';';
                }
                else if (key == "OemComma")
                {
                    sign = ',';
                }
                else if (key == "OemPeriod")
                {
                    sign = '.';
                }
                else if (key == "OemQuestion")
                {
                    sign = '?';
                }
                else
                {
                    sign = char.ToLower(key[0]);
                }
                List<StackPanel> SPes = new List<StackPanel>
            {
                    SP1,SP2,SP3,SP4
            };
                foreach (var SP in SPes)
                {
                    foreach (var item in SP.Children)
                    {
                        if (item is Button)
                        {
                            if ((item as Button).Content.ToString().ToLower() == sign.ToString())
                            {
                                (item as Button).Background = color;
                                (item as Button).Foreground = color2;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error with button coloring", "Eror", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TStart_Checked(object sender, RoutedEventArgs e)
        {
            TBFails.Text = "0";
            TBPasses.Text = "0";
            TBPersentage.Text = "0 %";
            TBSpeed.Text = "0 dig/min";
        }
    }
}
