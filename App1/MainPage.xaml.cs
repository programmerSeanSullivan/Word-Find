using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        String wordFound = "";
       private String[] dictionary = { "PIZZAZZ", "ZYZZYVA", "PIZAZZY", "JACUZZI", "JAZZMAN", "JAZZBOS", "JAZZILY" };


        struct scoringpiece
        {
           private  int score;
           private  char symbol;

            public void resetScore()
            {
                score = 0;
            }
            //Make sure to set symbol first
            public int getScore()
            {
                if(symbol.Equals('A') || symbol.Equals('E') || symbol.Equals('I') || symbol.Equals('O') 
                    || symbol.Equals('U') || symbol.Equals('L') || symbol.Equals('N') || symbol.Equals('S') || symbol.Equals('T') || symbol.Equals('R'))
                {
                    score = 1;
                } else if(symbol.Equals('D') || symbol.Equals('G'))
                {
                    score = 2;
                }else if(symbol.Equals('B') || symbol.Equals('C') || symbol.Equals('M') || symbol.Equals('P'))
                {
                    score = 3;
                }
                else if (symbol.Equals('F') || symbol.Equals('H') || symbol.Equals('V') || symbol.Equals('W') || symbol.Equals('Y'))
                {
                    score = 4;
                } else if(symbol.Equals('K'))
                {
                    score = 5;
                } else if(symbol.Equals('J') || symbol.Equals('X'))
                {
                    score = 8;
                }
                else if (symbol.Equals('Q') || symbol.Equals('Z'))
                {
                    score = 10;
                }
                              
                   return score;
            }

            public void setSymbol(char symbol)
            {
                this.symbol = symbol;
            }
           
       }
        
        public MainPage()
        {
            InitializeComponent();
        }

        /*
         * Individual Game pieces
         * 1-Highlight upon click
         * 1B-When Highlighted char will be removed if clicked again
         * 2-Unhilight on second click
         * 2B-When not Higlighted char will be appended to word
         */
        private void Button_Clicked(object sender, EventArgs e)
        {
          
       
            if ( ((Button)sender).BackgroundColor == Color.Gold)
            {
                ((Button)sender).BackgroundColor = default(Color);
                foundWord.Text = foundWord.Text.Replace(((Button)sender).Text.Substring(0, 1), "");
            }
            else
            {
                ((Button)sender).BackgroundColor = Color.Gold;
                wordFound = ((Button)sender).Text;
                foundWord.Text += wordFound;
            }
            
        }

        private String[] loadDictionary()
        {
           
            return dictionary;
        }
        /*
         * Check Button
         * 1-See if Found
         * 2-Remove from list of words
         * 3-Add up Points
         * 4-Update Score
         * 5-Reset Board and Clear out the Text for found Word
         */
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            List<String> words = new List<String>(loadDictionary());
            bool match = words.Any(foundWord.Text.Contains);
           
            if(match)
            {
                //Scoring
                int scoreboard = 0;
                scoringpiece theScoringPiece = new scoringpiece();
                char[] foundChars = foundWord.Text.ToCharArray();
                for(int i = 0; i < foundWord.Text.Length; i++)
                {
                    //Set the symbol
                    theScoringPiece.setSymbol(foundChars[i]);
                    scoreboard +=theScoringPiece.getScore();
                }
                Score.Text = (Convert.ToInt32(Score.Text) + scoreboard).ToString();


            }
            //Removal
            words.Remove(foundWord.Text);
            dictionary = words.ToArray<String>();
            //Resets
            foundWord.Text = "";
            Button[] buttons = new Button[49];
            String name;
            for (int i = 0; i < buttons.Length; i++)
            {
                name = "Button" + i;
                buttons[i] = (Button)FindByName(name);
                buttons[i].BackgroundColor = default(Color);
            }
            if(dictionary.Length ==0)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    foundWord.Text = "YOU WON!";
                    name = "Button" + i;
                    buttons[i] = (Button)FindByName(name);
                    buttons[i].BackgroundColor = default(Color);
                    buttons[i].IsEnabled = false;
                }
                StartButton.IsEnabled = true;
                StartButton.Text = "New Game";
            }
        }

        private char[] theShuffle(char[] cwords)
        {
          
            Random rnd = new Random();
            char[] shuffled = cwords.OrderBy(x => rnd.Next()).ToArray();
            return shuffled;
        }

        /*
         * 1-Loads in the dictionary list from the list of words text file
         * 2-Generates a random number for a starting point
         * 3-From that starting point it selects words until 49 char limit is reached.
         * 4-It will then set those strings into the array dictionary and will set the game words.
         *
         */
        private void setupGame()
        {

        }

        /*
         * Quit Button
         */
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        /*
         *  Start Button
         *  1-Starts the game by loading the dictionary
         *  2-Spreads the words across the board
         *  3-Start Button becomes unclickable.
         */
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            StartButton.Text = "Started";
            //Load dictionary for creating board
            String[] words = loadDictionary();
            //Encrypted List of Letters
            Char[] listOfLetters;
            //Holding All of the Strings
            String holder ="";
            //Merges the Array into single string
            foreach(String word in words)
            {
                holder +=word;
            }
            //Changes to char array
         
            listOfLetters = theShuffle(holder.ToCharArray());

            //Instatiates all of the buttons to be used for letter holders
            Button[] buttons = new Button[49];
            String name;
            //Sets up the board
            for (int i = 0; i < buttons.Length; i++)
            {
                ///Creates the name of the button
                name = "Button" + i;
                ///Creates the button to beloaded into array
                buttons[i]= (Button)FindByName(name);
                ///Places the new "Encrypted char" into the buttons text
                buttons[i].Text = listOfLetters[i].ToString();    
            }
            //Enables the Button
            ((Button)sender).IsEnabled = false;
        }
    }
}
