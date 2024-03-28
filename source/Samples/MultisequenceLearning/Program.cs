using MultisequenceLearning;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using static ApproveMultisequenceLearning.MultiSequenceLearning;
using static System.Net.Mime.MediaTypeNames;

namespace ApproveMultisequenceLearning
{
    class Program
    {
        public static int max;

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

            //read dataset in sequence
            var dataset = ReadDataset(data.First());
            var testDataset = ReadDataset(data.Last());

            //running the main experiment
            RunMultiSequenceLearningExperiment(dataset, testDataset);
        }

        /// <summary>
        /// This example demonstrates how to learn two sequences and how to use the prediction mechanism.
        /// First, two sequences are learned.
        /// Second, three short sequences with three elements each are created und used for prediction. The predictor used by experiment privides to the HTM every element of every predicting sequence.
        /// The predictor tries to predict the next element.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment(List<Sequence> dataset, List<Sequence> testDataset)
        {
            // Prototype for building the prediction engine.
            MultiSequenceLearning experiment = new MultiSequenceLearning();

            //call the experiment and create the learned model in predictor
            var predictor = experiment.Run(dataset, max);

            var logFile = experiment.OutputPath;
            var logs = new List<string>();

            foreach (Sequence item in testDataset)
            {
                Console.WriteLine("------------------------------");
                logs.Add("------------------------------");
                Console.WriteLine($"Using test sequence: {item.name}");
                logs.Add($"Using test sequence: {item.name}");
                predictor.Reset();
                var accuracy = PredictNextElement(predictor, item.data);
                Console.WriteLine($"Accuracy for {item.name} sequence: {accuracy}%");
                logs.Add($"Accuracy for {item.name} sequence: {accuracy}%");
            }

            MulitsequenceHelper.WriteLogs(logFile, logs);
        }
     
        /// <summary>
        /// Creates and saves dataset and test dataset as per config
        /// </summary>
        /// <returns>list of dataset full path</returns>
        private static List<string> CreateSaveData()
        {
            var datasetPath = new List<string>();
            int numberOfSequence = 50;
            int numberOfTestSequence = 10;
            int size = 25;
            int testSize = 5;
            int startVal = 4;
            int endVal = 35;

            max = endVal;

            // creating config for dataset
            ConfigOfSequence configOfSequence = new ConfigOfSequence(numberOfSequence, size, 0, startVal, endVal);

            var dataset = CreateDataset(configOfSequence);
            datasetPath.Add(dataset);

            // creating config for test dataset
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

            var datasetPath = MulitsequenceHelper.SaveDataset(dataset, "dataset");

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

            var testDatasetPath = MulitsequenceHelper.SaveDataset(testdataset, "dataset_test");

            return testDatasetPath;
        }

        /// <summary>
        /// Predicts the next element in the sequence using the provided predictor and input list and calculates accuracy
        /// </summary>
        /// <param name="predictor">predictor used for making predictions</param>
        /// <param name="list">input list for prediction</param>
        private static double PredictNextElement(Predictor predictor, int[] list)
        {
            int matchCount = 0;
            int predictions = 0;
            double accuracy = 0.0;
            int prev = -1;
            bool first = true;

            Debug.WriteLine("------------------------------");

            // loop through all the items in the list
            foreach (var item in list)
            {
                
                /*
                 * for the first item do not predict 
                 */
                if(first)
                {
                    first = false;
                }
                else 
                {
                    // predict the next element of previously saved element
                    Console.WriteLine($"Input: {prev}");
                    var res = predictor.Predict(prev);

                    if (res.Count > 0)
                    {
                        foreach (var pred in res)
                        {
                            Debug.WriteLine($"{pred.PredictedInput} - {pred.Similarity}");
                        }

                        var tokens = res.First().PredictedInput.Split('_');
                        var tokens2 = res.First().PredictedInput.Split('-');
                        Debug.WriteLine($"Predicted Sequence: {tokens[0]}, predicted next element {tokens2.Last()}");

                        // compare the predicted element with actual next element in sequence
                        if (item == Int32.Parse(tokens2.Last()))
                        {
                            // increment the count since matched
                            matchCount++;
                        }

                        // increment the count since predict function was called and prediction took place
                        predictions++;
                    }
                    else
                        Debug.WriteLine("Nothing predicted :(");
                }

                // save the element as previous to compare with next element predicted
                prev = item;
            }

            /*
             * ACCURACY calculated here!!!!!!
             * 
             * Accuracy is calculated as number of matching predictions made 
             * divided by total number of prediction made for an element in subsequence
             * 
             * accuracy = number of matching predictions/total number of prediction * 100
             */
            accuracy = (double)matchCount / predictions * 100;
            Debug.WriteLine("------------------------------");

            return accuracy;
        }

        /// <summary>
        /// Wrappper to read the dateset with full path to dataset
        /// </summary>
        /// <param name="datasetPath">full path to dataset</param>
        /// <returns>list of sequences</returns>
        private static List<Sequence> ReadDataset(string datasetPath)
        {
            List<Sequence> dataset = MulitsequenceHelper.ReadDataset(datasetPath);

            return dataset;
        }
    }
}
