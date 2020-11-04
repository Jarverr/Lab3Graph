using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Parser
{
    public class MethodsParser
    {
        public (Dictionary<int, string>, int[][]) ParseText(string text)
        {
            Dictionary<int, string> vertices = new Dictionary<int, string>();
            int[][] edges;
            IEnumerable<string> theRestToConvert;
            (vertices, theRestToConvert) =setVerticies(ref text);
            edges = GetInfoAboutEdges(theRestToConvert,vertices);
            return (vertices, edges);
        }

        private int[][] GetInfoAboutEdges(IEnumerable<string> theRestToConvert, Dictionary<int, string> vertices)
        {
            int[][] toReturn = new int[vertices.Count][];
            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = new int[vertices.Count];
            }
            string line;
            int x=0, y=0,value;
            int helper1 = 0, helper2 = 0;
            for (int i = 0; i < theRestToConvert.Count() - 1; i++)
            {
                line = String.Concat(theRestToConvert.ElementAt(i).Where(c => !Char.IsWhiteSpace(c)));
                var lenghtOfLine = line.Length;
                if (line.Contains("}"))
                    continue;
                for (int j = 0; j < lenghtOfLine; j++)
                {
                    if (line[j] == '-')
                    {
                        x = Convert.ToInt32(line.Substring(0, j));
                        break;
                    }
                }
                for (int j = 0; j < lenghtOfLine; j++)
                {
                    if (line[j] == '-' && line[j - 1] == '-')
                    {
                        helper1 = j + 1;
                    }
                    else if (line[j] == '[')
                    {
                        helper2 = j;
                    }
                }
                y = Convert.ToInt32(line.Substring(helper1, helper2 - helper1));
                value = Convert.ToInt32(line.Substring(helper2 + 8, line.Length - 2 - helper2 - 8));
                toReturn[y][x] = value;
                toReturn[x][y] = value;
            }


            ///Przerabianie grafu
            /****
            int[][] toReturn;
            //additional option 1 - commented
            if (theRestToConvert.ElementAt(0).Contains(">"))
            {
                toReturn = new int[theRestToConvert.Count() - 1][];
            }
            else
            {
                toReturn = new int[(theRestToConvert.Count() - 1)*2][];
            }
            int helper1=0, helper2 =0;
            string line;
            for (int i = 0; i < theRestToConvert.Count()-1; i++)
            {
                toReturn[i] = new int[3];
                line = String.Concat(theRestToConvert.ElementAt(i).Where(c => !Char.IsWhiteSpace(c)));
                var x=line.Length;
                if (line.Contains("}"))
                    continue;
                for (int j = 0; j < x; j++)
                {
                    if (line[j]=='-')
                    {
                        toReturn[i][0] = Convert.ToInt32(line.Substring(0, j));
                        break;
                    }
                }
                for (int j = 0; j < x; j++)
                {
                    if (line[j] == '-' && line[j - 1] == '-')
                    {
                        helper1 = j + 1;
                    }
                    else if (line[j] == '[')
                    {
                        helper2 = j;
                    }
                }
                toReturn[i][1] = Convert.ToInt32(line.Substring(helper1, helper2 - helper1));
                toReturn[i][2] = Convert.ToInt32(line.Substring(helper2+8, line.Length-2 - helper2 - 8));
            }


            ///ditional option 1
            if (!theRestToConvert.ElementAt(0).Contains(">"))
            {
                for (int i = 0; i < theRestToConvert.Count() - 1; i++)
                {
                    toReturn[i + theRestToConvert.Count() - 1] = new int[3];
                    toReturn[i + theRestToConvert.Count() - 1][0] = toReturn[i][1];
                    toReturn[i + theRestToConvert.Count() - 1][1] = toReturn[i][0];
                    toReturn[i + theRestToConvert.Count() - 1][2] = toReturn[i][2];
                }
            }
            */////
            return toReturn;
        }

        private (Dictionary<int, string>,IEnumerable<string>) setVerticies(ref string text)
        {
            Dictionary<int, string> toReturn = new Dictionary<int, string>();
            using var sr = new StringReader(text);
            string line;
            int counter = 1;
            while((line = sr.ReadLine()!)!=null)
            {
                if (line.Contains("name"))
                {
                    line = String.Concat(line.Where(c => !Char.IsWhiteSpace(c)));
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '[')
                        {
                            toReturn.Add(Convert.ToInt32(line.Substring(0, i)), line.Substring(i + 7, line.Length - 3 - i -7));
                            counter++;
                            break;
                            
                        }

                    }
                }
                else if (line.Contains("weight"))
                    break;
            }
            var diffDates=skipLines(counter, text).ToArray();
            return (toReturn,diffDates);

        }

        private IEnumerable<string> skipLines(int counter,  string text)
        {
            return text.Split(Environment.NewLine)
                .Skip(counter).ToArray();
        }
    }
}
