using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCiphers.Ciphers
{
    class ColumnarTranspositionC : Cipher
    {
        private int[] rowsOrder;
        public string key;

        public ColumnarTranspositionC(string key)
        {
            this.key = key;
            rowsOrder = new int[key.Length];

            CalculateRowsOrder();
        }

        public string Decrypt(string text)
        {
            // usuwa spacje z textu
            string corrected_text = removeSpaces(text);

            //tworzy tablice o min wymiarach
            char[,] array = getArray(corrected_text.Length);
            // wstawia znaki w opdowiednie miejsca do tablcy a reszte zapełnia spacjami 
            array = fillArray(array, corrected_text);

            // z tablicy na string
            int key_value = 0;
            string output = "";
            for (int i = 0; i < array.GetLength(1); i++)
            {
                for (int j = 0; j < array.GetLength(0); j++)
                {
                    if (array[j, key_value] != ' ')
                        output += array[j, key_value];
                }
                output += ' ';
                key_value = getNextKeyValue(i);
            }

            return output;
        }

        public string Encrypt(string text)
        {
            return null;
        }

        private void CalculateRowsOrder()
        {
            char[] chars = key.ToArray();
            Array.Sort(chars);

            Dictionary<char, int> occurrences = InitOccurrences(chars);

            for (int i = 0; i < rowsOrder.Length; i++)
            {
                rowsOrder[i] = Array.IndexOf(chars, key[i]) + occurrences[key[i]];
                occurrences[key[i]]++;
            }
        }

        private Dictionary<char, int> InitOccurrences(char[] chars)
        {
            Dictionary<char, int> occurences = new Dictionary<char, int>();
            foreach (char ch in chars)
            {
                occurences[ch] = 0;
            }

            return occurences;
        }

        //tworzy tablice o min wymiarach
        private char[,] getArray(int text_lenght)
        {
            int rows = 1;
            int counted_letters = 0;

            for (int current_letter = 0; current_letter < key.Length; current_letter++)
            {
                for (int i = 0; i < key.Length; i++)
                {
                    counted_letters++;
                    if (rowsOrder[i] == current_letter)
                        break;
                }

                if (counted_letters >= text_lenght)
                    break;
                else
                    rows++;
            }

            return new char[rows, key.Length];
        }

        // wstawia znaki w opdowiednie miejsca do tablicy a reszte zapełnia spacjami 
        private char[,] fillArray(char[,] array, string text)
        {
            int current_letter = 0;
            int key_value = 0;
            bool flag = false;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (flag == false && current_letter < text.Length)
                    {
                        array[i, j] = text[current_letter];
                        current_letter++;

                        if (rowsOrder[j] == key_value)
                            flag = true;
                    }
                    else
                        array[i, j] = ' ';
                }
                flag = false;
                key_value++;
            }

            return array;
        }

        //zwraca pozycję kolejnej kolumny z której odczytać
        private int getNextKeyValue(int prev_value)
        {
            int output = 0;
            for (int i = 0; i < key.Length; i++)
                if (rowsOrder[i] == prev_value + 1)
                    output = i;
            return output;
        }

        // usuwa spacje z textu
        private string removeSpaces(string text)
        {
            string output = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ')
                    output += text[i];
            }

            return output;
        }

        // dodatkowa funkcja sprawdzająca czy da się zakodować dany tekst przy użyciu danego klucza
        // nie uzywam tego przy szyfrowaniu tylko tak dałem zeby potem ułatwić weryfikację danych wejsciowych 
        private bool checkKeyCorrectness(string text)
        {
            // tekst bez spacji
            string corrected_text = removeSpaces(text);
            int counter = 0;

            // sprawdzenie jaka jest maksymalna długosc slowa mozliwa do zakodowania w 
            // danym kluczu
            for (int current_letter = 0; current_letter < key.Length; current_letter++)
            {
                for (int i = 0; i < key.Length; i++)
                {
                    counter++;
                    if (rowsOrder[i] == current_letter)
                        break;
                }
            }

            // porównanie otrzymanej wartosci i długosci slowa
            if (counter >= corrected_text.Length)
                return true;
            else
                return false;
        }
    }
}
