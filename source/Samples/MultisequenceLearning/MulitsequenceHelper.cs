using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoCortexApi.Entities;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using Newtonsoft.Json;

namespace ApproveMultisequenceLearning
{
    public class MulitsequenceHelper
    {
        /// <summary>
        /// Get the encoder with settings
        /// </summary>
        /// <param name="inputBits">input bits</param>
        /// <param name="max">max value of input</param>
        /// <returns>Object of EncoderBase</returns>
        public static ScalarEncoder GetEncoder(int inputBits, int max)
        {
            Dictionary<string, object> settings = new Dictionary<string, object>()
            {
                { "W", 15},
                { "N", inputBits},
                { "Radius", -1.0},
                { "MinVal", 0.0},
                { "Periodic", false},
                { "Name", "scalar"},
                { "ClipInput", false},
                { "MaxVal", (double)max}
            };

            ScalarEncoder encoder = new ScalarEncoder(settings);

            return encoder;
        }

        /// <summary>
        /// HTM Config for creating Connections
        /// </summary>
        /// <param name="inputBits">input bits</param>
        /// <param name="numColumns">number of columns</param>
        /// <returns>Object of HTMConfig</returns>
        public static HtmConfig GetHtmConfig(int inputBits, int numColumns)
        {
            HtmConfig cfg = new HtmConfig(new int[] { inputBits }, new int[] { numColumns })
            {
                Random = new ThreadSafeRandom(42),

                CellsPerColumn = 25,
                GlobalInhibition = true,
                LocalAreaDensity = -1,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * inputBits),
                //InhibitionRadius = 15,

                MaxBoost = 10.0,
                DutyCyclePeriod = 25,
                MinPctOverlapDutyCycles = 0.75,
                MaxSynapsesPerSegment = (int)(0.02 * numColumns),

                ActivationThreshold = 15,
                ConnectedPermanence = 0.5,

                // Learning is slower than forgetting in this case.
                PermanenceDecrement = 0.25,
                PermanenceIncrement = 0.15,

                // Used by punishing of segments.
                PredictedSegmentDecrement = 0.1
            };

            return cfg;
        }

        /// <summary>
        /// Reads dataset from the file
        /// </summary>
        /// <param name="path">full path of the file</param>
        /// <returns>Object of list of Sequence</returns>
        public static List<Sequence> ReadDataset(string path)
        {
            Console.WriteLine("Reading Sequence...");
            String lines = File.ReadAllText(path);
            List<Sequence> sequence = System.Text.Json.JsonSerializer.Deserialize<List<Sequence>>(lines);

            return sequence;
        }

        /// <summary>
        /// Saves the dataset in 'dataset' folder in BasePath of application
        /// </summary>
        /// <param name="sequences">Object of list of Sequence</param>
        /// <returns>Full path of the dataset</returns>
        public static string SaveDataset(List<Sequence> sequences)
        {
            if (sequences == null)
                return null;

            string BasePath = AppDomain.CurrentDomain.BaseDirectory;
            string datasetFolder = Path.Combine(BasePath, "dataset");
            if (!Directory.Exists(datasetFolder))
                Directory.CreateDirectory(datasetFolder);
            string datasetPath = Path.Combine(datasetFolder, $"dataset_{DateTime.Now.Ticks}.json");

            Console.WriteLine("Saving dataset...");

            if (!File.Exists(datasetPath))
            {
                using (StreamWriter sw = File.CreateText(datasetPath))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(sequences));
                }
            }

            return datasetPath;
        }
    }
}
