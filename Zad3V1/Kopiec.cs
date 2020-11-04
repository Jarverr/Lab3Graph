using System;
using System.Collections.Generic;
using System.Text;

namespace Zad3V1
{
    class Kopiec
    {
        int[][] kopiec;
        public Kopiec(int vertex, int amoutOfVertcies)
        {
            CreateKopiec(vertex, amoutOfVertcies);
        }
        private void CreateKopiec(int vertex, int amountOfvertecies)
        {
            kopiec = new int[amountOfvertecies][];
            for (int i = 0; i < amountOfvertecies; i++)
            {
                kopiec[i] = new int[2];
                kopiec[i][0] = i;
                kopiec[i][1] = Int32.MaxValue;

            }
            kopiec[vertex][1] = 0;
            PrzywrocWlasnocKopca();
        }
        public bool IsEmpty()
        {
            if (kopiec.Length >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public (int,int) DeleteMin()
        {
            int toReturn = kopiec[0][0];
            int toReturnValue = kopiec[0][1];
            int[][] kopiecToRewrite = kopiec;
            kopiec = new int[kopiec.Length - 1][];
            if (kopiec.Length != 0)
            {
                kopiec[0] = new int[2];
                kopiec[0][0] = kopiecToRewrite[kopiec.Length][0];
                kopiec[0][1] = kopiecToRewrite[kopiec.Length][1];
                for (int i = 1; i < kopiecToRewrite.Length - 1; i++)
                {
                    kopiec[i] = new int[2];
                    kopiec[i][0] = kopiecToRewrite[i][0];
                    kopiec[i][1] = kopiecToRewrite[i][1];
                }
                PrzywrocWlasnocKopca();
            }
            return (toReturn,toReturnValue);
        }

        private void PrzywrocWlasnocKopca()
        {
            int temp;
            int temp2;
            bool doIChangeSth = false;
            for (int i = 1; i < kopiec.Length; i++)
            {
                if (!(kopiec[i][1] >= kopiec[Convert.ToInt32(Math.Floor((i - 1) / 2d))][1]))
                {
                    temp = kopiec[Convert.ToInt32(Math.Floor((i - 1) / 2d))][1];
                    temp2 = kopiec[Convert.ToInt32(Math.Floor((i - 1) / 2d))][0];
                    kopiec[Convert.ToInt32(Math.Floor((i - 1) / 2d))][1] = kopiec[i][1];
                    kopiec[Convert.ToInt32(Math.Floor((i - 1) / 2d))][0] = kopiec[i][0];
                    kopiec[i][1] = temp;
                    kopiec[i][0] = temp2;
                    doIChangeSth = true;
                }
            }
            if (doIChangeSth)
            {
                PrzywrocWlasnocKopca();
            }
            return;
        }
        public bool ChangeDistanceIfPossible(int startValue, int end, int value)
        {
           for (int i = 0; i < kopiec.Length; i++)
            {
                if (end == kopiec[i][0])
                {
                    if (kopiec[i][1] > value + startValue)
                    {
                        kopiec[i][1] = value + startValue;
                        PrzywrocWlasnocKopca();
                        return true;
                    }
                }
            }
            return false;
        }
        private void ChangeValueOfVertex(int vert, int value)
        {
            for (int i = 0; i < kopiec.Length; i++)
            {
                if (kopiec[i][0] == vert)
                {
                    kopiec[i][1] = value;
                    break;
                }
            }
            PrzywrocWlasnocKopca();
            return;
        }
    }
}
