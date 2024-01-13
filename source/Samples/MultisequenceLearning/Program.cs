using NeoCortexApi;
using NeoCortexApi.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static ApproveMultisequenceLearning.MultiSequenceLearning;

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
            //running the main experiment
            RunMultiSequenceLearningExperiment();
        }

        /// <summary>
        /// This example demonstrates how to learn two sequences and how to use the prediction mechanism.
        /// First, two sequences are learned.
        /// Second, three short sequences with three elements each are created und used for prediction. The predictor used by experiment privides to the HTM every element of every predicting sequence.
        /// The predictor tries to predict the next element.
        /// </summary>
        private static void RunMultiSequenceLearningExperiment()
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

            Debug.WriteLine("------------------------------");
        }
    }
}
