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
        int textFileLinesAmountBreakPoint = 100;

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
        MatrixTransp mt;


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
                    case "MATRIX_TRANSP":
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
                    int[] inputTab = parseColumnarTranspKey(userKey);
                    if (validateColumnarTranspkey(inputTab))
                    {
                        ct = new ColumnarTransposition(parseColumnarTranspKey(userKey));
                        encrypted = ct.Encrypt(userInput);
                        decrypted = ct.Decrypt(userInput);
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "MATRIX_TRANSP":
                    if (validateMatrixTransp(userKey))
                    {
                        mt = new MatrixTransp(parseMatrixTranspKey(userKey));
                        encrypted = mt.Encrypt(userInput);
                        decrypted = mt.Decrypt(userInput);
                    }
                    else
                    {

                        return;
                    }
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
            string algorithmName = "";
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
                        algorithmName = "rail fence";
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "COLUMNAR_TRANSP":
                    int[] inputTab = parseColumnarTranspKey(userKey);
                    if (validateColumnarTranspkey(inputTab))
                    {
                        algorithm = new ColumnarTransposition(parseColumnarTranspKey(userKey));
                        algorithmName = "columnar transposition";
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "MATRIX_TRANSP":
                    if (validateMatrixTransp(userKey))
                    {
                        algorithm = new MatrixTransp(parseMatrixTranspKey(userKey));
                    }
                    else
                    {

                        return;
                    }
                    break;
                default:
                    return;
            }

            // depending on the status move through input list and update outcome list

            string currentDate = DateTime.Now.ToString().Replace(':', ' ').Replace('/', ' ');
            

            if (status == "encrypting")
            {
                if (listOfLines.Count > textFileLinesAmountBreakPoint)
                {
                    TextWriter tw = new StreamWriter("EncryptedListOfWords - " + currentAlgorithm + " - " + currentDate + ".txt");
                    string outcome = "";
                    foreach (string s in listOfLines)
                    {
                        outcome = algorithm.Encrypt(s);
                        tw.WriteLine(outcome);
                    }
                    tw.Close();
                }
                else
                {
                    foreach (string line in userInputScrollList)
                    {
                        userInput = line;
                        encrypted = algorithm.Encrypt(line);
                        outcomeScrollViewerList.Add(encrypted);
                    }
                }
            }
            else
            {
                if (listOfLines.Count > textFileLinesAmountBreakPoint)
                {
                    TextWriter tw = new StreamWriter("DecryptedListOfWords - " + currentAlgorithm + " - " + currentDate + ".txt");
                    string outcome = "";
                    foreach (string s in listOfLines)
                    {
                        outcome = algorithm.Decrypt(s);
                        tw.WriteLine(outcome);
                    }
                    tw.Close();
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
            if (listOfLines.Count > textFileLinesAmountBreakPoint)
            {
                MessageBox.Show("Check app's folder for outcome of this operation. ");
                outcomeScrollViewerList.Add("Check your app's folder for");
                if (status == "encrypting") outcomeScrollViewerList.Add("EncryptedListOfWords - ");
                else outcomeScrollViewerList.Add("DecryptedListOfWords - " );
                outcomeScrollViewerList.Add(currentAlgorithm); 
                outcomeScrollViewerList.Add(" - ");
                outcomeScrollViewerList.Add(currentDate);
                outcomeScrollViewerList.Add(".txt");
            }


        }
        private bool validateMatrixTransp(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    return true;
                }
            }
            return false;
        }
        private string parseMatrixTranspKey(string s)
        {
            return s.ToLower();
        }
        private bool validateRailfenceFields(string s)
        {
            int i;
           
            if (int.TryParse(s, out i))
            {
                if (i < 2)
                {
                    MessageBox.Show("Rail fence key is invalid. Please provide integer greater than 1.");
                    return false;
                }
                  
                return true;
            }
            MessageBox.Show("Rail fence key is invalid. Please provide integer greater than 1.");
            return false;
        }
        private bool validateColumnarTranspkey(int []table)
        {
            // TODO: get to know-how of columanr transp algorithm and replace this dummy method with proper one
            // Numbers must be in a succession ( 3-2-1-4 -> ok,    3-2-4 -> bad )
            // every number from 1 to max number inside key must be present
            // there shall not be any repetitions
            int[] temptable = table;
            Array.Sort(temptable);
            int i = 1;
            foreach( int value in temptable)
            {
                if (value != i)
                {
                    MessageBox.Show("Columnar transp key is invalid. Every number from 1 to max must be present, there shall not be any repetitions.");
                    return false;
                }  
                else i++;
            }
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
                }
                else
                {
                    if (cacheNumber != "") l.Add(Int32.Parse(cacheNumber));
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
                    i++;
                    listOfLines.Add(line);
                    //
                }
            }
            if (listOfLines.Count > textFileLinesAmountBreakPoint)
            {
                userInputScrollList.Add("File stored in memory");
                userInputScrollList.Add("you can still do your ");
                userInputScrollList.Add("encrypt/decrypt operations.");
                userInputScrollList.Add("Lines stored: " + listOfLines.Count);
                MessageBox.Show("Your file is stored in the memory, you can do the encrypt/decrypt operation on it and it will be saved to seperate text file inside app's main folder.");
            }
            else
            {
                foreach (string s in listOfLines)
                    userInputScrollList.Add(s);
            }
        }
        private void clearView()
        {
            mTextBox.Text = "";
            keyTextBox.Text = "";
            outcomeTypeLabel.Content = "";
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
