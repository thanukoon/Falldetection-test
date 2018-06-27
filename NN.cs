using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;



namespace Microsoft.Samples.Kinect.BodyBasics
{
    class NN
    {
        private static readonly string sourceFile = Path.Combine(Environment.CurrentDirectory, "datatest.csv"); //breast-cancer-wisconsin
       // private static readonly string scource = 
        // Number of input neurons, hidden neurons and output neurons


            

        private static readonly int[] inputColumns = { 0, 1, 2, 3, 4, 5, 6, 7, 8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54 }; // ไว้เพิ่มcolumn
        //private static readonly int numInput = inputColumns.Length;
        private static readonly int numInput = inputColumns.Length;
        private const int numHidden = 20; 
        private const int numOutput = 2;
        
        // Parameters for NN training
        private const int maxEpochs = 500;
        private const double learnRate = 0.05;
        private const double momentum = 0.01;
        private const double weightDecay = 0.0001;
        public static int cc;
        
    


        public void yes ()
        {

            var datahead = new List<double[]>();
            var dataspine = new List<double[]>();
            Console.WriteLine("Neural Network Demo using .NET by Sebastian Brandes");
            Console.WriteLine("Data Set: Breast Cancer Wisconsin (Diagnostic), November 1995");
            // Source: http://archive.ics.uci.edu/ml/machine-learning-databases/breast-cancer-wisconsin/
            Console.WriteLine();
            //C:\Users\Goon\source\repos\BreastCancerNeuralNetwork\BreastCancerNeuralNetwork\Data\breast-cancer-wisconsin.csv
            #region Data Generation
            Console.WriteLine("Loading source file and generating data sets...");
            var rows = File.ReadAllLines(sourceFile);
            var data = new List<double[]>();
            var data2 = new List<double[]>();

            //  testt.AddRange(inputColumns);
            int count = 0;
            int countdown = 0;
            

            foreach (var row in rows)
            {
                var values = row.Split(',');
                cc = values.Length;
                var observation = new double[values.Length];
              
              
               for (int i = 0; i < values.Length; i++)
                {
                  double.TryParse(values[i], out observation[i]);
                    count++;
                }
                count = 0;
                countdown++;
                data.Add(observation);
               
            }

            //   ab = data.ToArray();
            datahead.AddRange(data);
            int remove = data.Count / 2;
            datahead.RemoveRange(0,remove);
            dataspine.AddRange(datahead);
            int head = 0;
            int spine = 0;

            for (int i = 0;  i<data.Count; i++)
            {
                if(i % 2 == 0)
                {
                    for (int j = 0; j<cc;j++)
                    {
                        datahead[head][j] = data[i][j];
                        
                    }
                    Console.WriteLine("asd");
                    head++;
                }
                else
                {
                    for (int j = 0; j<cc; j++)
                    {
                        dataspine[spine][j] = data[i][j];
                        Console.WriteLine(dataspine[spine][j]);

                    }
                    spine++;
                }
               
            }





            List<double[]> trainData;
            List<double[]> testData;
            Helpers.GenerateDataSets(datahead, out trainData, out testData, 0.8);

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion
            

            #region Normalization
            Console.WriteLine("Normalizing data...");
            List<double[]> normalizedTrainData = Helpers.NormalizeData(trainData,inputColumns);
            List<double[]> normalizedTestData = Helpers.NormalizeData(testData, inputColumns);

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion

            #region Initializing the Neural Network
            Console.WriteLine("Creating a new {0}-input, {1}-hidden, {2}-output neural network...", numInput, numHidden, numOutput);
            var nn = new NeuralNetwork(numInput, numHidden, numOutput);

            Console.WriteLine("Initializing weights and bias to small random values...");
            nn.InitializeWeights();

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion

            #region Training
            Console.WriteLine("Beginning training using incremental back-propagation...");
            nn.Train(normalizedTrainData.ToArray(), maxEpochs, learnRate, momentum, weightDecay);

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion

            #region Results
            double[] weights = nn.GetWeights();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Final neural network weights and bias values:");
            Console.ResetColor();
            Helpers.ShowVector(weights, 10, 3, true);
            Console.WriteLine();

            double trainAcc = nn.Accuracy(normalizedTrainData.ToArray());
            Console.WriteLine("Accuracy on training data = " + trainAcc.ToString("F4"));
            double testAcc = nn.Accuracy(normalizedTestData.ToArray());
            Console.WriteLine("Accuracy on test data = " + testAcc.ToString("F4"));
            Console.WriteLine();




            ///spine neuralnetwork
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Spinebase neuralnetwork");

            List<double[]> trainDataspine;
            List<double[]> testDataspine;
            Helperspine.GenerateDataSets(dataspine, out trainDataspine, out testDataspine, 0.8);

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion


            #region Normalization
            Console.WriteLine("Normalizing data...");
            List<double[]> normalizedTrainDataspine = Helperspine.NormalizeData(trainDataspine, inputColumns);
            List<double[]> normalizedTestDataspine = Helperspine.NormalizeData(testDataspine, inputColumns);

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion

            #region Initializing the Neural Network
            //Console.WriteLine("Creating a new {0}-input, {1}-hidden, {2}-output neural network...", numInput, numHidden, numOutput);
            var nn2 = new NeuralNetworkspine(numInput, numHidden, numOutput);

            Console.WriteLine("Initializing weights and bias to small random values...");
            nn2.InitializeWeights();

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion

            #region Training
            Console.WriteLine("Beginning training using incremental back-propagation...");
            nn2.Train(normalizedTrainDataspine.ToArray(), maxEpochs, learnRate, momentum, weightDecay);

            Console.WriteLine("Done!");
            Console.WriteLine();
            #endregion

            #region Results
            double[] weightspine = nn2.GetWeights();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Final neural network weights and bias values:");
            Console.ResetColor();
            Helperspine.ShowVector(weightspine, 10, 3, true);
            Console.WriteLine();

            double trainAccspine = nn2.Accuracy(normalizedTrainData.ToArray());
            Console.WriteLine("Accuracy on training data = " + trainAccspine.ToString("F4"));
            double testAccspine = nn2.Accuracy(normalizedTestData.ToArray());
            Console.WriteLine("Accuracy on test data = " + testAccspine.ToString("F4"));
            Console.WriteLine();


            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine("Raw results:");
            //Console.ResetColor();
            //Console.WriteLine(nn.ToString());
            #endregion



        }
    }






}

