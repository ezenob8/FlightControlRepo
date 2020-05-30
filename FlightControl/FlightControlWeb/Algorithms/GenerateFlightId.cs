using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Algorithms
{
    public class GenerateFlightId
    {
        public static string GenerateFlightIdentifier(string currentId)
        {
            if(currentId.CompareTo("") == 0)
            {
                return "AAAA-0000";
            }
            else
            {
                string alphabetPartStr = currentId.Split('-')[0];
                string numberPartStr = currentId.Split('-')[1];
                int numberPart = Convert.ToInt32(numberPartStr);

                if (numberPart <= 9998)
                {
                    numberPart++;
                    numberPartStr = numberPart.ToString();
                    numberPartStr = numberPartStr.PadLeft(4, '0');
                }
                else
                {
                    numberPartStr = "0000";
                    alphabetPartStr = nextAlphabetPart(alphabetPartStr);
                }
                return alphabetPartStr + "-" + numberPartStr;
            }
        }

        public static string nextAlphabetPart(string alphabetPart)
        {
            List<string> alphabet = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            int index = 0;
            foreach (var letter in alphabet)
            {
                alphabetPart = alphabetPart.Replace(letter, index.ToString());
                index++;
            }
            int nextAlphabetPart = Convert.ToInt32(alphabetPart) + 1;
            alphabetPart = nextAlphabetPart.ToString();
            alphabetPart = alphabetPart.PadLeft(4, '0');
            index = 0;
            foreach (var letter in alphabet)
            {
                alphabetPart = alphabetPart.Replace(index.ToString(), letter);
                index++;
            }
            return alphabetPart;
        }
    }
}
