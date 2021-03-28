using System;
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
using WPFCiphers.Generators;

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
        Vigenere vig;
        ColumnarTranspositionC ctc;
        Cezar cz;

        SynchronousStreamCipher synchronousStreamCipher;

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
                logoLabel.Foreground = Brushes.Black;
            }
            else
            {
                filessideBarGrid.Visibility = Visibility.Visible;
                sideBarGrid.Visibility = Visibility.Visible;
                currentAlgorithm = buttonName;
                algoNameLabel.Content = buttonName;
                filesalgoNameLabel.Content = buttonName;
                hideGenButtons();
                switch (currentAlgorithm)
                {
                    case "RAIL_FENCE":
                        mLabel.Content = "M";
                        keyLabel.Content = "n";
                        fileskeyLabel.Content = "n";
                        logoLabel.Foreground = Brushes.Red;
                        break;
                    case "COLUMNAR_TRANSP":
                        mLabel.Content = "M";
                        keyLabel.Content = "key";
                        fileskeyLabel.Content = "key";
                        logoLabel.Foreground = Brushes.Orange;
                        break;
                    case "MATRIX_TRANSP":
                        mLabel.Content = "M";
                        keyLabel.Content = "key";
                        fileskeyLabel.Content = "key";
                        logoLabel.Foreground = Brushes.Yellow;
                        break;
                    case "COLUMNAR_C":
                        mLabel.Content = "M";
                        keyLabel.Content = "key";
                        fileskeyLabel.Content = "KEY";
                        logoLabel.Foreground = Brushes.Green;
                        break;
                    case "ViGENERE":
                        mLabel.Content = "M";
                        keyLabel.Content = "K";
                        fileskeyLabel.Content = "K";
                        logoLabel.Foreground = Brushes.LightBlue;
                        break;
                    case "CEZAR":
                        mLabel.Content = "M";
                        keyLabel.Content = "K";
                        fileskeyLabel.Content = "K";
                        logoLabel.Foreground = Brushes.Purple;
                        break;
                    case "SYNC":
                        mLabel.Content = "M";
                        keyLabel.Content = "POLYNOMIAL";
                        fileskeyLabel.Content = "POLYNOMIAL";
                        showGenButtons();
                        logoLabel.Foreground = Brushes.White;
                        filessideBarGrid.Visibility = Visibility.Hidden;
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

            bool normalDialog = true;

            switch (currentAlgorithm)
            {
                case "RAIL_FENCE":
                    if (mTextBox.Text == "")
                    {
                        MessageBox.Show("Rail fence text input is empty. Please type in something.");
                        return;
                    }
                    if (validateRailfenceFields(keyTextBox.Text.ToString()))
                    {
                        int i = int.Parse(userKey);
                        rf = new RailFence(i);
                        encrypted = rf.Encrypt(userInput);
                        decrypted = rf.Decrypt(userInput);

                    }
                    break;
                case "COLUMNAR_TRANSP":
                    if (mTextBox.Text == "")
                    {
                        MessageBox.Show("Columanr transp text input is empty. Please type in something.");
                        return;
                    }
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
                    if (mTextBox.Text == "")
                    {
                        MessageBox.Show("Matrix transp text input is empty. Please type in something.");
                        return;
                    }
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
                case "COLUMNAR_C":
                    if (mTextBox.Text == "")
                    {
                        MessageBox.Show("Matrix transp version C text input is empty. Please type in something.");
                        return;
                    }
                    if (validateMatrixTranspVerCKey(userKey) && validateMatrixTranspVerCWord(userInput))
                    {
                        ctc = new ColumnarTranspositionC(userKey);
                        encrypted = ctc.Encrypt(userInput);
                        decrypted = ctc.Decrypt(userInput);
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "ViGENERE":
                    if (mTextBox.Text == "")
                    {
                        MessageBox.Show("Winegret text input is empty. Please type in something.");
                        return;
                    }
                    if (validateVinegretKey(userKey) && validateVinegretWord(userInput))
                    {
                        vig = new Vigenere(userKey);
                        encrypted = vig.Encrypt(userInput);
                        decrypted = vig.Decrypt(userInput);
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "CEZAR":
                    if (mTextBox.Text == "")
                    {
                        MessageBox.Show("Cezar text input is empty. Please type in something.");
                        return;
                    }
                    if (validateCezarKey(userKey) && validateCezarWord(userInput))
                    {
                        int i = int.Parse(userKey);
                        cz = new Cezar(i);
                        encrypted = cz.Encrypt(userInput);
                        decrypted = cz.Decrypt(userInput);
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "SYNC":
                    if (syncFileName.Content.ToString() == " ")
                    {
                        MessageBox.Show("SYNC file input is empty. Please type in something.");
                        return;
                    }
                    if (keyTextBox.Text == "")
                    {
                        MessageBox.Show("SYNC key text input is empty. Please type in something.");
                        return;
                    }
                    if (!syncKeyGenerated)
                    {
                        MessageBox.Show("SYNC key needs to be generated first, please press run and then stop button before proceeding.");
                        return;
                    }
                    normalDialog = false;
                    synchronousStreamCipher = new SynchronousStreamCipher(lsfrGen.GetSequence());

                   
                    if (buttonName == "encrypt")
                    {
                         synchronousStreamCipher.Encrypt(syncFileName.Content.ToString());

                    }
                    else
                    {
                         synchronousStreamCipher.Decrypt(syncFileName.Content.ToString());
                    }
                    break;
                default:
                    break;
            }


            int length = encrypted.Length;

            outcomeLabel.FontSize = 30;
            if (length > 10) outcomeLabel.FontSize = 20;
            if (length > 20) outcomeLabel.FontSize = 15;

            if (normalDialog)
            {
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
            } else
            {
     
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
                    if (fileskeyTextBox.Text == "")
                    {
                        MessageBox.Show("Rail fence key is emtpy. Please type in something.");
                        return;
                    }
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
                    if (fileskeyTextBox.Text == "")
                    {
                        MessageBox.Show("Columnar transp key is emtpy. Please type in something.");
                        return;
                    }
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
                    if (fileskeyTextBox.Text == "")
                    {
                        MessageBox.Show("Matrix transp key is emtpy. Please type in something.");
                        return;
                    }
                    if (validateMatrixTransp(userKey))
                    {
                        algorithm = new MatrixTransp(parseMatrixTranspKey(userKey));
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "COLUMNAR_C":
                    if (fileskeyTextBox.Text == "")
                    {
                        MessageBox.Show("Matrix transp version C text input is empty. Please type in something.");
                        return;
                    }
                    if (validateMatrixTranspVerCKey(userKey))
                    {
                        algorithm = new ColumnarTranspositionC(userKey);

                    }
                    else
                    {

                        return;
                    }
                    break;
                case "ViGENERE":
                    if (fileskeyTextBox.Text == "")
                    {
                        MessageBox.Show("Winegret text input is empty. Please type in something.");
                        return;
                    }
                    if (validateVinegretKey(userKey))
                    {
                        algorithm = new Vigenere(userKey);
                    }
                    else
                    {

                        return;
                    }
                    break;
                case "CEZAR":
                    if (fileskeyTextBox.Text == "")
                    {
                        MessageBox.Show("Cezar text input is empty. Please type in something.");
                        return;
                    }
                    if (validateCezarKey(userKey))
                    {
                        int i = int.Parse(userKey);
                        algorithm = new Cezar(i);
                    }
                    else
                    {

                        return;
                    }
                    break;
                default:
                    return;
            }

            // depending on the status move through input list and update outcome listsdfafsdafsadfsad

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
                else outcomeScrollViewerList.Add("DecryptedListOfWords - ");
                outcomeScrollViewerList.Add(currentAlgorithm);
                outcomeScrollViewerList.Add(" - ");
                outcomeScrollViewerList.Add(currentDate);
                outcomeScrollViewerList.Add(".txt");
            }


        }
        private bool validateCezarWord(string s)
        {
            bool containsAtLeastOneLetter = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    containsAtLeastOneLetter = true;
                }
                else
                {
                    MessageBox.Show("CEZAR word needs to contain only letters.");
                    return false;
                }
            }
            if (containsAtLeastOneLetter)
            {
                return true;
            }
            else
            {
                MessageBox.Show("CEZAR word needs to contain at least one letter. Please type in something.");
                return false;
            }
        }
        private bool validateCezarKey(string s)
        {
            int i;



            if (int.TryParse(s, out i))
            {
                if (i < 1)
                {
                    MessageBox.Show("CEZAR key is invalid. Please provide integer greater than 1 or equal.");
                    return false;
                }

                return true;
            }
            MessageBox.Show("CEZAR key is invalid. Please provide integer greater than 1 or equal.");
            return false;
        }
        private bool validateMatrixTranspVerCKey(string s)
        {
            bool containsAtLeastOneLetter = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    containsAtLeastOneLetter = true;
                }
                else
                {
                    MessageBox.Show("Matrix transp ver_C key needs to contain only letters and blank spaces.");
                    return false;
                }
            }
            if (containsAtLeastOneLetter)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Matrix transp ver_C key needs to contain at least one letter. Please type in something.");
                return false;
            }

        }
        private bool validateMatrixTranspVerCWord(string s)
        {
            bool containsAtLeastOneLetter = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]) || Char.IsWhiteSpace(s[i]))
                {
                    containsAtLeastOneLetter = true;
                }
                else
                {
                    MessageBox.Show("Matrix transp ver_C key needs to contain only letters and blank spaces.");
                    return false;
                }
            }
            if (containsAtLeastOneLetter)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Matrix transp ver_C key needs to contain at least one letter. Please type in something.");
                return false;
            }

        }
        private bool validateVinegretWord(string s)
        {
            bool containsAtLeastOneLetter = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    containsAtLeastOneLetter = true;
                }
                else
                {
                    MessageBox.Show("winegret key should contain only letters");
                    return false;
                }
            }
            if (containsAtLeastOneLetter)
            {
                return true;
            }
            else
            {
                MessageBox.Show("winegret key needs to contain at least one letter. Please type in word that contains only letters.");
                return false;
            }

        }
        private bool validateVinegretKey(string s)
        {
            bool containsAtLeastOneLetter = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    containsAtLeastOneLetter = true;
                }
                else
                {
                    MessageBox.Show("winegret key should contain only letters");
                    return false;
                }
            }
            if (containsAtLeastOneLetter)
            {
                return true;
            }
            else
            {
                MessageBox.Show("winegret key needs to contain at least one letter. Please type in word that contains only letters.");
                return false;
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
            MessageBox.Show("Matrix transp key needs to contain at least one letter. Please type in something.");
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
                if (i < 1)
                {
                    MessageBox.Show("Rail fence key is invalid. Please provide integer greater than 1 or equal.");
                    return false;
                }

                return true;
            }
            MessageBox.Show("Rail fence key is invalid. Please provide integer greater than 1 or equal.");
            return false;
        }
        private bool validateColumnarTranspkey(int[] table)
        {
            int[] temptable = table;
            Array.Sort(temptable);
            int i = 1;

            if (temptable.Length < 1)
            {
                MessageBox.Show("Columnar transp key input is emtpy. Please type in something.");
                return false;
            }
            foreach (int value in temptable)
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
            fileskeyTextBox.Text = "";
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

        private void mTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textBoxText = ((TextBox)sender).Text;
            int length = textBoxText.Length;

            mTextBox.FontSize = 20;

            if (length > 10) mTextBox.FontSize = 15;
            if (length > 20) mTextBox.FontSize = 10;
        }


        private bool validateSyncKey(int[] table)
        {
            int[] temptable = table;
            Array.Sort(temptable);
            bool firstLoop = true;
            int prevValue = 0;

            if (temptable.Length < 1)
            {
                MessageBox.Show("SYNC key input is emtpy. Please type in something.");
                return false;
            }
            foreach (int value in temptable)
            {
                if (value == prevValue && firstLoop == false)
                {
                    MessageBox.Show("SYNC key is invalid.There shall not be any repetitions.");
                    return false;
                }
                else
                {
                    prevValue = value;
                    firstLoop = false;
                }
                   
            }
            return true;
        }
        private int[] parseSyncKey(string s)
        {
            List<int> l = new List<int>();

            //
            char startingSequence = '^';
            bool beginSequence = false;
            char endingSequence = '+';

            string cachedNumber = "";
            //
            // needed for parsing algorithm to work.
            s += " ";
            for (int i = 0; i < s.Length; i++)
            {
                if (beginSequence)
                {
                    if (Char.IsDigit(s[i]))
                    {
                        cachedNumber += s[i];
                    }
                    else
                    {
                        if (cachedNumber != "" && Int32.Parse(cachedNumber) != 0) l.Add(Int32.Parse(cachedNumber));
                        cachedNumber = "";

                    }
                }
                if (!Char.IsDigit(s[i]))
                {
                    if (s[i] == startingSequence)
                    {
                        beginSequence = true;
                    }
                    if (s[i] == endingSequence)
                    {
                        beginSequence = false;
                    }
                }


            }
            int[] tab = new int[l.Count];
            for (int i = 0; i < l.Count; i++)
            {
                tab[i] = l.ElementAt(i);
            }
            return tab;
        }


        internal bool stillWorking = true;
        public bool syncKeyGenerated = false;
        LFSR lsfrGen = new LFSR();
        internal async Task lfsrGenerating(int[] table)
        {

                while (stillWorking)
                {

                lsfrGen.GenerateSequence(table);
                await Task.Delay(1000);
                }
     
        }
        private void startGen_Click(object sender, RoutedEventArgs e)
        {
           
        
            int[] table = parseSyncKey(keyTextBox.Text);
            if (validateSyncKey(table))
            {
                stopGen.IsEnabled = true;
                startGen.IsEnabled = false;
                syncKeyGenerated = false;
                encrypt.IsEnabled = false;
                decrypt.IsEnabled = false;
                genStatusLabel.Content = "Your key is being generated.";
                lsfrGen.StartGenerator(table);
            } else
            {
                //MessageBox.Show("SYNC key is invalid.");
            }
           
         
       
        }

        private void stopGen_Click(object sender, RoutedEventArgs e)
        {
            lsfrGen.StopGenerator();
            genStatusLabel.Content = "Your key is ready.";
            syncKeyGenerated = true;
            stillWorking = false;
            stopGen.IsEnabled = false;
            startGen.IsEnabled = true;
            encrypt.IsEnabled = true;
            decrypt.IsEnabled = true;
        }
        private void showGenButtons()
        {
            stopGen.IsEnabled = false;
            startGen.IsEnabled = true;
            startGen.Visibility = Visibility.Visible;
            stopGen.Visibility = Visibility.Visible;
            syncchooseFileButton.Visibility = Visibility.Visible;
            syncFileName.Visibility = Visibility.Visible;
            mLabel.Visibility = Visibility.Hidden;
            mTextBox.Visibility = Visibility.Hidden;
        }
        private void hideGenButtons()
        {
            stopGen.IsEnabled = false;
            startGen.IsEnabled = false;
            startGen.Visibility = Visibility.Hidden;
            stopGen.Visibility = Visibility.Hidden;
            syncchooseFileButton.Visibility = Visibility.Hidden;
            syncFileName.Visibility = Visibility.Hidden;
            mLabel.Visibility = Visibility.Visible;
            mTextBox.Visibility = Visibility.Visible;
        }

        private void syncchooseFileButton_Click(object sender, RoutedEventArgs e)
        {

            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "SYNC_CIPHER FILES (*.png;*.jpeg;*.txt;*.bin)|*.png;*.jpeg;*.txt;*.bin|All files (*.*)|*.*" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            var filepath = ofd.FileName;
            syncFileName.Content = ofd.FileName;
        }
    }



}
