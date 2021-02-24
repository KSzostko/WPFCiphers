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
        private void cipherButtonPressed(object sender, System.Windows.RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            mTextBox.Text = " ";
            keyTextBox.Text = " ";
            encryptedLabel.Content = " ";
            decryptedLabel.Content = " ";
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

        private void doTheMagic(object sender, RoutedEventArgs e)
        {
            switch (currentAlgorithm)
            {
                case "RAIL_FENCE":
                    if (validateRailfenceFields())
                    {
                        int i = int.Parse(keyTextBox.Text.ToString());
                        rf = new RailFence(i);
                        string encrypted = rf.Encrypt(mTextBox.Text.ToString());
                        string decrypted = rf.Decrypt(encrypted);
                        encryptedLabel.Content = encrypted;
                        decryptedLabel.Content = decrypted;
                    }
                    break;
                case "COLUMNAR_TRANSP":
                    
                    break;
                default:
                    break;
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
    }

   
}
