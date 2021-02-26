﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        public ObservableCollection<string> outcomeScrollViewerList { get; set; }
        public ObservableCollection<string> userInputScrollList { get; set; }
        //List<string> userInputList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            outcomeScrollViewerList = new ObservableCollection<string>();
            userInputScrollList = new ObservableCollection<string>();
            this.DataContext = this;

          

        }

        string currentAlgorithm = "none";
        RailFence rf;
        ColumnarTransposition ct;



        private void cipherButtonPressed(object sender, System.Windows.RoutedEventArgs e)
        {
            string buttonName = ((Button)sender).Name;
            clearView();

            outcomeLabel.Content = " ";
            if (buttonName == currentAlgorithm)
            {
                filessideBarGrid.Visibility = Visibility.Hidden;
                sideBarGrid.Visibility = Visibility.Hidden;
                currentAlgorithm = "none";
            }
            else
            {
                filessideBarGrid.Visibility = Visibility.Visible;
                sideBarGrid.Visibility = Visibility.Visible;
                currentAlgorithm = buttonName;
                algoNameLabel.Content = buttonName;
                filesalgoNameLabel.Content = buttonName;

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
                    if (validateRailfenceFields(keyTextBox.Text.ToString()))
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
                if (encrypted != "") outcomeTypeLabel.Content = "Encrypted:";
                outcomeLabel.Content = encrypted;
            }
            else
            {
                if (decrypted != "") outcomeTypeLabel.Content = "Decrypted:";
                outcomeLabel.Content = decrypted;
            }
           


        }

        private void filesencryptDecryptPressed(object sender, RoutedEventArgs e)
        {
            // setup outcome list
            outcomeScrollViewerList.Clear();

            // init key, algorithm and temp fields
            string buttonName = ((Button)sender).Name;
            string userKey = fileskeyTextBox.Text.ToString();
            string userInput = "";
            string encrypted = "";
            string decrypted = "";
            string status = "";
            Cipher algorithm;
            // needed cause compiler cries about algorithm not being set
            algorithm = new RailFence(3);
            // check for encryption status
            if (buttonName == "filesencrypt")
            {
                status = "encrypting";
            }
            else
            {
                status = "decrypting";
            }

            parseColumnarTranspKey(userKey);

            // inicialize algorithm with key
            switch (currentAlgorithm)
            {
                case "RAIL_FENCE":
                    if (validateRailfenceFields(fileskeyTextBox.Text.ToString()))
                    {
                        int i = int.Parse(userKey);
                        algorithm = new RailFence(i);
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "COLUMNAR_TRANSP":
                    if (validateColumnarTranspkey(userKey))
                    {
                        parseColumnarTranspKey(userKey);
                        //algorithm = new ColumnarTransposition(parseColumnarTranspKey(userKey));
                    } else
                    {
                        return;
                    }
                    break;
                default:
                    return;
            }

            // depending on the status move through input list and update outcome list
            if (status == "encrypting")
                foreach (string line in userInputScrollList)
                {
                    userInput = line;
                    encrypted = algorithm.Encrypt(line);
                    outcomeScrollViewerList.Add(encrypted);
                }
            else
            {
                foreach (string line in userInputScrollList)
                {
                    userInput = line;
                    decrypted = algorithm.Decrypt(line);
                    outcomeScrollViewerList.Add(decrypted);
                }
            }

        }

        private bool validateRailfenceFields(string s)
        {
            int i;
            if (int.TryParse(s, out i))
            {
                if (i != 1) return true;
            }

            return false;
        }
        private bool validateColumnarTranspkey(string s)
        {
            // TODO: get to know-how of columanr transp algorithm and replace this dummy method with proper one
            return true;
        }
        private int[] parseColumnarTranspKey(string s)
        {
            List<int> l = new List<int>();

            string cacheNumber = "";
            // needed for parsing algorithm to work.
            s += " ";
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsDigit(s[i]))
                {
                    cacheNumber += s[i];
                } else
                {
                    if(cacheNumber != "") l.Add(Int32.Parse(cacheNumber));
                    cacheNumber = "";

                }
            }
            int[] tab = new int[l.Count];
            for (int i = 0; i < l.Count; i++)
            {
                tab[i] = l.ElementAt(i);
            }
            return tab;
        }


        List<string> listOfLines = new List<string>();

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            

            clearLists();

            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "TXT Files (*.txt)|*.txt" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            var filepath = ofd.FileName;
            filesmLabel.Content = ofd.FileName;
           


            using (FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {

                    outcomeLabel.Content = i;
                    i++;
                    listOfLines.Add(line);
                    //userInputScrollList.Add("X");
                }
            }
            //foreach (string line in listOfLines)
            //{
            //    dispatcher.Invoke(DispatcherPriority.DataBind, (Action<T>)((line) => {
            //        Add(line);
            //    }), item);
            //}
            //string[] lines = System.IO.File.ReadAllLines(@filepath);
            //foreach (string line in lines)
            //{
            //    // Use a tab to indent each line of the file.
            //    userInputList.Add(line);
            //    userInputScrollList.Add(line);
            //}
        }
        private void clearView()
        {
            mTextBox.Text = " ";
            keyTextBox.Text = " ";
            outcomeTypeLabel.Content = " ";
            userInputScrollList.Clear();
            outcomeScrollViewerList.Clear();
            
            filesmLabel.Content = "";
        }
        private void clearLists()
        {
            userInputScrollList.Clear();
            outcomeScrollViewerList.Clear();
            listOfLines.Clear();
           
        }
    }



}
