using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace projectTower
{
   
    static class Writer
    {

        public static void WriteText(string str, int y, int x = 1){
            List<string> writtenDesc = FitString(str);
            for (int i = 0; i < writtenDesc.Count(); i++)
            {
                Console.SetCursorPosition(x, y+i);
                Console.WriteLine(writtenDesc[i]);
            }
            Console.SetCursorPosition(0, 35);
        }


        public static List<string> FitString(string str){
            List<string> writtenDesc = new List<string>();
            int descLength = str.Length;
            int lineSize = 60;
            for (int i = 0; i < descLength; i+=lineSize)
            {
                lineSize = DetermineLineSize(str, i);
                if(descLength - i >= lineSize)
                    writtenDesc.Add(str.Substring(i, lineSize));
                else  
                    writtenDesc.Add(str.Substring(i));
            }

            return writtenDesc;
        }

        public static int DetermineLineSize(string str, int lineStart){
            int lineSize = 64;
            string line = str.Substring(lineStart);

            while(line.Length > lineSize && line[lineSize] != ' '){
                lineSize++;
            }

            return lineSize;
        }
    }
}





