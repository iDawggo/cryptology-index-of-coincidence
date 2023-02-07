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
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Security;

namespace Index_of_Coincidence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void scrllInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            String cipherText = scrllInput.Text.ToLower();
            String unfText = Regex.Replace(cipherText, "[^a-z]", ""); //unformatted ciphertext

            bool checkTxt = Regex.IsMatch(unfText, @"^[a-zA-Z]+$"); //checking if input isnt exclusively numbers or symbols
            if (checkTxt == false)
            {
                iocOutput.Text = "Please enter a valid string with letters! No numbers or symbols!";
                return;
            }
            else
            {
                iocOutput.Text = "";
                iocOutput.Text = indexOfCoincidence(unfText).ToString(); //using IOC method
            }
        }

        private void tpdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            String cipherText = scrllInput.Text.ToLower();
            String unfText = Regex.Replace(cipherText, "[^a-z]", ""); //unformatted ciphertext

            int testPd;

            bool checkPd = int.TryParse(tpdInput.Text, out testPd);
            if (checkPd == false)
            {
                scrllOutput.Text = "Please enter a valid integer!";
                return;
            }
            else
            {
                scrllOutput.Text = "";
            }

            scrllOutput.Text = "";
            for (int i = 0; i < testPd; i++) //outer loop, for the length of the test period
            {
                string splitText = "";
                for (int j = i; j < unfText.Length - i; j += testPd) //inner loop, splitting chunks of text for every # of test pd
                {
                    splitText += unfText[j];
                }
                scrllOutput.Text += (i + 1) + " = " + indexOfCoincidence(splitText).ToString() + "\n"; //calculating IOC
            }
        }

        double indexOfCoincidence(string cipherTxt) //index of coincidence method
        {
            int numChars = cipherTxt.Length; //# of characters
            int indexChar = 0; //index of a char
            int sumFreq = 0; //sum of frequencies of each letter
            int[] alphaFreqs = new int[26]; //storing # of occurences of every letter (Fa)
            foreach (char letter in cipherTxt) //calculating # of occurences of every letter (Fa)
            {
                indexChar = (int)(letter);
                indexChar -= 97;
                alphaFreqs[indexChar]++;
            }

            for (int i = 0; i < 26; i++) //calculates the numerator of the IOC formula
            {
                sumFreq += alphaFreqs[i] * (alphaFreqs[i] - 1);
            }

            double iocOut = (double)sumFreq / (numChars * (numChars - 1)); //caulculates the rest of the IOC formula
            return iocOut;
        }
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            String tempTxt = "";
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    var sr = new StreamReader(fileDialog.FileName);
                    tempTxt = sr.ReadToEnd();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error. \n\n Error message: {ex.Message}\n\n" + $"Details:\n\n{ex.StackTrace}");
                }
            }

            scrllInput.Text = tempTxt;
        }
    }
}
