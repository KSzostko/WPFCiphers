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
using WPFCiphers.Ciphers;
namespace WPFCiphers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// g
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        // cipherButtonPressed fields...
        string currentAlgorithm = "none";
        RailFence rf;
        ColumnarTransposition ct;
        private void cipherButtonPressed(object sender, System.Windows.RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            mTextBox.Text = " ";
            keyTextBox.Text = " ";
            outcomeTypeLabel.Content = " ";

            outcomeLabel.Content = " ";
            if (buttonName == currentAlgorithm)
            {
                sideBarGrid.Visibility = Visibility.Hidden;
                currentAlgorithm = "none";
            }
            else
            {
                sideBarGrid.Visibility = Visibility.Visible;
                currentAlgorithm = buttonName;
                algoNameLabel.Content = buttonName;

             
                switch (currentAlgorithm)
                {
                    case "RAIL_FENCE":
                        mLabel.Content = "M";
                        keyLabel.Content = "n";
                        break;
                    case "COLUMNAR_TRANSP":
                        mLabel.Content = "M";
                        keyLabel.Content = "key";
                        break;
                    default:
                        break;
                }
            }
        }

        private void encryptDecryptPressed(object sender, RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;

            string userInput = mTextBox.Text.ToString();
            string userKey = keyTextBox.Text.ToString();

            string encrypted = "";
            string decrypted = "";

            switch (currentAlgorithm)
            {
                case "RAIL_FENCE":
                    if (validateRailfenceFields())
                    {
                        int i = int.Parse(userKey);
                        rf = new RailFence(i);
                        encrypted = rf.Encrypt(userInput);
                        decrypted = rf.Decrypt(userInput);
                    }
                    break;
                case "COLUMNAR_TRANSP":
                    ct = new ColumnarTransposition(parseColumnarTranspKey(userKey));
                    encrypted = ct.Encrypt(userInput);
                    decrypted = ct.Decrypt(userInput);
                    break;
                default:
                    break;
            }

            if (buttonName == "encrypt")
            {
                if(encrypted != "") outcomeTypeLabel.Content = "Encrypted:";
                outcomeLabel.Content = encrypted;
            }
            else
            {
                if (decrypted != "") outcomeTypeLabel.Content = "Decrypted:";
                outcomeLabel.Content = decrypted;
            }
                

        }
    
        private bool validateRailfenceFields()
        {
            int i;
            if (int.TryParse(keyTextBox.Text.ToString(), out i))
            {
                if( i != 1) return true;
            }

            return false;
        }
        private int[] parseColumnarTranspKey(String s)
        {
            List<int> l = new List<int>();

            int digit = 0;
            int multiplier = 1;
            for(int i = 0; i < s.Length; i++)
            {
                char x = s[i];
                if (x < 48 || x > 57)
                {
                    if (digit != 0) l.Add(digit);
                    digit = 0;
                    multiplier = 1;
                } else
                {
                    digit += (x - 48) * multiplier;
                    multiplier *= 10;
                }
            }
            int[] tab;
            tab = new int[l.Count];
            for(int i = 0; i < l.Count; i++)
            {
                tab[i] = l.ElementAt(i);
            }
            return tab;
        }
      
    }

   
}
