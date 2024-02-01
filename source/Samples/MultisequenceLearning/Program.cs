using MultisequenceLearning;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static ApproveMultisequenceLearning.MultiSequenceLearning;
using static System.Net.Mime.MediaTypeNames;

namespace ApproveMultisequenceLearning
{
    class Program
    {
        /// <summary>
        /// This sample shows a typical experiment code for SP and TM.
        /// You must start this code in debugger to follow the trace.
        /// and TM.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //creating dataset for experiment
            var data = CreateSaveData();

            //running the main experiment
            //RunMultiSequenceLearningExperiment();
        }

        /// <summary>
        /// This example demonstrates how to learn two sequences and how to use the prediction mechanism.
        /// First, two sequences are learned.
        /// Second, three short sequences with three elements each are created und used for prediction. The predictor used by experiment privides to the HTM every element of every predicting sequence.
        /// The predictor tries to predict the next element.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment(string dataset)
        {
            //input sequences
            Dictionary<string, List<double>> sequences = new Dictionary<string, List<double>>();

            sequences.Add("S1", new List<double>(new double[] { 0.0, 1.0, 2.0, 4.0, 6.0, 5.0, 7.0, 9,0}));
            sequences.Add("S2", new List<double>(new double[] { 8.0, 10.0, 12.0, 19.0, 20.0, 0.0, 1.0, 2.0 }));
            sequences.Add("S3", new List<double>(new double[] { 5.0, 7.0, 8.0, 10.0, 12.0, 13.0, 14.0, 15.0 }));
            sequences.Add("S4", new List<double>(new double[] { 15.0, 17.0, 18.0, 1.0, 2.0, 4.0, 6.0, 5.0 }));
            sequences.Add("S5", new List<double>(new double[] { 8.0, 10.0, 12.0, 19.0, 20.0, 0.0, 2.0, 4.0 }));

            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();

            //call the experiment and create the learned model in predictor
            var predictor = experiment.Run(sequences);

            //
            // These list are used to see how the prediction works.
            // Predictor is traversing the list element by element. 
            // By providing more elements to the prediction, the predictor delivers more precise result.
            // Consider this as test sequence and we predict the next element 
            var list1 = new double[] { 1.0, 2.0, 4.0, 6.0 };
            var list2 = new double[] { 20.0, 0.0, 1.0 };
            var list3 = new double[] { 19.0, 20.0, 0.0 };
            var list4 = new double[] { 10.0, 12.0, 13.0 };

            predictor.Reset();
            PredictNextElement(predictor, list1);

            predictor.Reset();
            PredictNextElement(predictor, list2);

            predictor.Reset();
            PredictNextElement(predictor, list3);

            predictor.Reset();
            PredictNextElement(predictor, list4);
        }
     
        private static List<string> CreateSaveData()
        {
            var datasetPath = new List<string>();
            int numberOfSequence = 10;
            int numberOfTestSequence = 10;
            int size = 20;
            int testSize = 4;
            int startVal = 4;
            int endVal = 30;
            
            ConfigOfSequence configOfSequence = new ConfigOfSequence(numberOfSequence, size, 0, startVal, endVal);

            var dataset = CreateDataset(configOfSequence);
            datasetPath.Add(dataset);

            ConfigOfSequence configOfTestSequence = new ConfigOfSequence(numberOfTestSequence, size, testSize, startVal, endVal);

            var testDataset = CreateTestDataset(configOfTestSequence, MulitsequenceHelper.ReadDataset(dataset));
            datasetPath.Add(testDataset);
            return datasetPath;
        }
        /// <summary>
        /// Generates a dataset based on the provided configuration
        /// </summary>
        /// <param name="config">Configuration for dataset generation</param>
        /// <returns>return the path to the stored dataset</returns>
        private static string CreateDataset(ConfigOfSequence config)
        {
            var dataset = DatasetHelper.CreateDataset(config.count, config.size, config.startVal, config.endVal);

            var datasetPath = MulitsequenceHelper.SaveDataset(dataset);

            return datasetPath;
        }
        /// <summary>
        /// Creates a test dataset based on the provided configuration and existing sequences
        /// </summary>
        /// <param name="config">Configuration for test dataset generation</param>
        /// <param name="sequences">List of existing sequences to be included in the test dataset</param>
        /// <returns>return the path to the stored test dataset</returns>
        private static string CreateTestDataset(ConfigOfSequence config, List<Sequence> sequences)
        {

            var testdataset = TestDatasetHelper.CreateTestDataset(config.count, config.size - 3, config.testSize, config.startVal, config.endVal, sequences);

            var testDatasetPath = MulitsequenceHelper.SaveDataset(testdataset);

            return testDatasetPath;
        }
        /// <summary>
        /// Predicts the next element in the sequence using the provided predictor and input list
        /// </summary>
        /// <param name="predictor">predictor used for making predictions</param>
        /// <param name="list">input list for prediction</param>
        private static void PredictNextElement(Predictor predictor, double[] list)
        {
            Debug.WriteLine("------------------------------");

            foreach (var item in list)
            {
                var res = predictor.Predict(item);

                if (res.Count > 0)
                {
                    foreach (var pred in res)
                    {
                        Debug.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                    }

                    var tokens = res.First().PredictedInput.Split('_');
                    var tokens2 = res.First().PredictedInput.Split('-');
                    Debug.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");
                }
                else
                    Debug.WriteLine("Nothing predicted :(");
            }

            /*
             * Calculate the ACCURACY here!!!!!!
             * 
             * Accuracy is calculated as number of matching predictions made 
             * divided by total number of prediction made for an element in subsequence
             * 
             * accuracy = number of matching predictions/total number of prediction * 100
             */

            Debug.WriteLine("------------------------------");
        }
    }
}
