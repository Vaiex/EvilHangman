using System;
using System.Data.Common;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Linq;
using Unit1Notes.EvilHangman;

namespace Unit1Notes.EvilHangman
{

    public class EvilHangman
    {
        //private enum HangmanState
        //{
        //    Initial = 6,
        //    Head = 5,
        //    Neck = 4,
        //    LeftArm = 3,
        //    RightArm = 2,
        //    LeftLeg = 1,
        //    Dead = 0
        //}

        private int numGuess = 15;
        private string word;
        private List<string> possibleWords;
        private HashSet<char> guessedLetters;
        static private int wordLength;


        static string path = "/Users/kokil/Documents/sc/appDev/Unit1Notes/Unit1Notes/EvilHangman/";

        static void Main(string[] args)
        {
            EvilHangman game = new EvilHangman();
            Console.WriteLine("Enter the number of letters you want for your word");
            wordLength = Convert.ToInt32(Console.ReadLine());
            game.run();
        }

        public void run()
        {
            game();

        }

        private void LoadDictionary(int wordLength)
        {
            var dic = path + "dictionary.txt";
            possibleWords = File.ReadAllLines(dic)
                              .Where(word => word.Length == wordLength)
                              .ToList();
        }

        public void game()
        {
            LoadDictionary(wordLength);
            guessedLetters = new HashSet<char>();
            bool gameOver = false;

            Random random = new Random();
            word = new string('_', wordLength);

            while (!gameOver)
            {
                Console.Clear();
                DrawHangMan(numGuess);
                DisplayWord();

                Console.WriteLine("Guessed Letters: " + (guessedLetters != null ? string.Join(", ", guessedLetters) : ""));
                Console.Write("Enter a letter as a guess: ");
                char guess = Char.ToUpper(Console.ReadKey().KeyChar);

                if (!char.IsLetter(guess) || (guessedLetters != null && guessedLetters.Contains(guess)))
                {
                    Console.WriteLine("\nLetter already guessed... try again.");
                    continue;
                }

                guessedLetters.Add(guess);

                Dictionary<string, List<string>> wordFamilies = getWordFamilies(guess);
                bestWordArea(wordFamilies);


                if (!word.Contains(guess))
                {
                    numGuess--;
                    if (numGuess == 0)
                    {
                        //Console.Clear();
                        DrawHangMan(numGuess);

                        if (possibleWords.Any())
                        {
                            Console.WriteLine("You lose! The word was: " + possibleWords[0]);
                        }
                        else
                        {
                            Console.WriteLine("You lose! There are no remaining words.");
                            Console.WriteLine("The word was: " + word);
                        }

                        gameOver = true;
                    }
                }
                else if (word == possibleWords[0])
                {
                    //Console.Clear();
                    DrawHangMan(numGuess);
                    Console.WriteLine("Congratulations! You win! The word is: " + word);
                    gameOver = true;
                }
            }
        }


        private void DisplayWord()
        {
            Console.WriteLine("\n" + wordLength + " is the length of the word.");
            Console.WriteLine("Word: " + word);
            Console.WriteLine("Remaining Guesses: " + numGuess);
        }


        private Dictionary<string, List<string>> getWordFamilies(char guess)
        {
            Dictionary<string, List<string>> wordFamilies = new Dictionary<string, List<string>>();

            foreach (string possibleWord in possibleWords)
            {
                StringBuilder wb = new StringBuilder(word);

                for (int i = 0; i < word.Length; i++)
                {
                    if (possibleWord[i] == guess)
                    {
                        wb[i] = guess;
                    }
                    else if (word[i] != '_' && word[i] != possibleWord[i])
                    {
                        wb[i] = '_';
                    }
                }

                string newWord = wb.ToString();

                if (!wordFamilies.ContainsKey(newWord))
                {
                    wordFamilies[newWord] = new List<string>();
                }

                wordFamilies[newWord].Add(possibleWord);
            }

            return wordFamilies;
        }


        private void bestWordArea(Dictionary<string, List<string>> wordFamilies)
        {
            var AWithUND = wordFamilies.Where(pair => pair.Key.Contains('_')).ToList();

            if (AWithUND.Any())
            {
                var largestAreas = AWithUND.OrderByDescending(pair => pair.Value.Count).First();
                word = largestAreas.Key;
                possibleWords = largestAreas.Value;
            }
            else
            {
                if (possibleWords.Any())
                {
                    var largestAreas = wordFamilies.OrderByDescending(pair => pair.Value.Count).First();
                    word = largestAreas.Key;
                    possibleWords = largestAreas.Value;
                }
                else
                {
                    Console.WriteLine("No matching words found.");
                }
            }
        }


        //static void DrawHangMan()
        //{
        //    StringBuilder hangman = new StringBuilder()
        //    .Append(@"------").AppendLine()
        //    .Append(@"|    o").AppendLine()
        //    .Append(@"|   /|\").AppendLine()
        //    .Append(@"|   / \").AppendLine()
        //    .Append(@"|      ").AppendLine()
        //    .Append(@"-------").AppendLine();
        //}

        static void DrawHangMan(int numGuess)
        {
            StringBuilder hangman = new StringBuilder();

            switch (numGuess)
            {
                case (6):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("-------");
                    break;

                case (5):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|    o");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("-------");
                    break;

                case (4):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|    o");
                    hangman.AppendLine("|    |");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("-------");
                    break;

                case (3):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|    o");
                    hangman.AppendLine("|   /|");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("-------");
                    break;

                case (2):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|    o");
                    hangman.AppendLine("|   /|\\");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("|     ");
                    hangman.AppendLine("-------");
                    break;

                case (1):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|    o");
                    hangman.AppendLine("|   /|\\");
                    hangman.AppendLine("|   /  ");
                    hangman.AppendLine("|      ");
                    hangman.AppendLine("-------");
                    break;

                case (0):
                    hangman.AppendLine("------");
                    hangman.AppendLine("|    o");
                    hangman.AppendLine("|   /|\\");
                    hangman.AppendLine("|   / \\");
                    hangman.AppendLine("|      ");
                    hangman.AppendLine("-------");
                    break;
            }

            Console.WriteLine(hangman);

        }
    }


}


