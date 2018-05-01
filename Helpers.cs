using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class Helpers
    {

       


        public static void GenerateDataSets(List<double[]> data, out List<double[]> trainData, out List<double[]> testData, double ratio)
        {
            int trainDataLength = (int)Math.Floor(data.Count * ratio);

            var r = new Random();

            trainData = data.OrderBy(x => r.Next()).Take(trainDataLength).ToList();
            testData = data.Except(trainData).ToList();
        }

        public static List<double[]> NormalizeData(List<double[]> data, int[] columns)
        {
            foreach (var col in columns)
            {
                double sum = data.Sum(observation => observation[col]);//observation => observation[col]
                double mean = sum / data.Count;
                double sse = data.Sum(observation => (observation[col] - mean) * (observation[col] - mean));
                double sd = Math.Sqrt(sse / (data.Count - 1));

                foreach (var observation in data)
                {
                    observation[col] = (observation[col] - mean) / sd;
                }
            }

            return data;
        }

        public static void ShowVector(double[] vector, int valsPerRow, int decimals, bool newLine)
        {
           double[] a = vector;
         

            for (int i = 0; i < vector.Length; ++i)
            {

                if (i % valsPerRow == 0)

                    Console.WriteLine("");
                Console.Write(vector[i].ToString("F" + decimals).PadLeft(decimals + 4) + " ");
                a[i] = vector[i];

                
            }
            if (newLine == true)
            {
                Console.WriteLine(" ");
                


            }

        }
    }
}
