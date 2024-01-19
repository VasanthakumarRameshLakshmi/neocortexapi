using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApproveMultisequenceLearning
{
    public class DatasetHelper
    {
        /// <summary>
        /// Checks if the parameters for creating dataset is valid or not
        /// </summary>
        /// <param name="numberOfSequence">number of sequence which needs to be created</param>
        /// <param name="size">size of sequence</param>
        /// <param name="startVal">start value of sequence</param>
        /// <param name="endVal">end value of sequence</param>
        /// <returns>return true if parameters are valid</returns>
        public static bool IsCreateDatasetValid(int numberOfSequence, int size, int startVal, int endVal)
        {
            try
            {
                if (numberOfSequence < 2)
                    throw new ArgumentException("Number sequence must atleast 2");

                //need atleast size as 9 since we randomly remove 3 elements and output size will be 6
                if (size <= 9)
                    throw new ArgumentException("Size of each sequence must be atleast 8");

                //the set of number in sequence must be a whole number i.e all positive numbers
                if (startVal < 0)
                    throw new ArgumentException("");

                //endValue of sequence must be greater than startValue to have a sequence
                if (endVal < startVal)
                    throw new ArgumentException("endVal must be greater than startVal");

                //since minimum size of sequence is 8 the difference in startValue and endValue must be atleast 9
                if ((endVal - startVal) < size)
                    throw new ArgumentException("Size is greater than unique number of input");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception - IsCreateDatasetValid: {ex.Message}");
                return false;
            }


            return true;
        }

        /// <summary>
        /// Creates list of Sequence as per configuration
        /// </summary>
        /// <param name="numberOfSequence"></param>
        /// <param name="size"></param>
        /// <param name="startVal"></param>
        /// <param name="endVal"></param>
        /// <returns>return a list of sequence</returns>
        public static List<Sequence> CreateDataset(int numberOfSequence, int size, int startVal, int endVal)
        {
            if (!IsCreateDatasetValid(numberOfSequence, size, startVal, endVal))
                return null;
            Console.WriteLine("Creating Sequence...");
            List<Sequence> sequence = CreateSequences(numberOfSequence, size, startVal, endVal);

            return sequence;

        }

        /// <summary>
        /// Creats multiple sequences as per parameters
        /// </summary>
        /// <param name="count">Number of sequences to be created</param>
        /// <param name="size">Size of each sequence</param>
        /// <param name="startVal">Minimum value of item in a sequence</param>
        /// <param name="stopVal">Maximum value of item in a sequence</param>
        /// <returns>Object of list of Sequence</returns>
        public static List<Sequence> CreateSequences(int count, int size, int startVal, int stopVal)
        {
            List<Sequence> dataset = new List<Sequence>();

            for (int i = 0; i < count; i++)
            {
                Sequence sequence = new Sequence();
                sequence.name = $"S{i + 1}";
                sequence.data = getSyntheticData(size, startVal, stopVal);
                dataset.Add(sequence);
            }

            return dataset;
        }

        /// <summary>
        /// Creates a sequence of given size-3 and range
        /// </summary>
        /// <param name="size">Size of list</param>
        /// <param name="startVal">Min range of the list</param>
        /// <param name="stopVal">Max range of the list</param>
        /// <returns></returns>
        private static int[] getSyntheticData(int size, int startVal, int stopVal)
        {
            int[] data = new int[size];

            data = randomRemoveInt(randomInt(size, startVal, stopVal), 3);

            return data;
        }

        /// <summary>
        /// Creates a sorted list of array with given paramerters
        /// </summary>
        /// <param name="size">Size of array</param>
        /// <param name="startVal">Min range of the list</param>
        /// <param name="stopVal">Max range of the list</param>
        /// <returns></returns>
        private static int[] randomInt(int size, int startVal, int stopVal)
        {
            int[] array = new int[size];
            List<int> list = new List<int>();
            int number = 0;
            Random r = new Random(Guid.NewGuid().GetHashCode());
            while (list.Count < size)
            {
                number = r.Next(startVal, stopVal);
                if (!list.Contains(number))
                {
                    if (number >= startVal && number <= stopVal)
                        list.Add(number);
                }
            }

            array = list.ToArray();
            Array.Sort(array);

            return array;
        }

        /// <summary>
        /// Randomly remove less number of items from array
        /// </summary>
        /// <param name="array">array to processed</param>
        /// <param name="less">number of removals to be done</param>
        /// <returns>array with less numbers</returns>
        private static int[] randomRemoveInt(int[] array, int less)
        {
            int[] temp = new int[array.Length - less];
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int number = 0;
            List<int> list = new List<int>();

            while (list.Count < (array.Length - less))
            {
                number = array[random.Next(0, (array.Length))];
                if (!list.Contains(number))
                    list.Add(number);
            }

            temp = list.ToArray();
            Array.Sort(temp);

            return temp;
        }

        /// <summary>
        /// Creates a sorted list of unique random integers with the given parameters
        /// </summary>
        /// <param name="size">Size of the list</param>
        /// <param name="startVal">Min range of the list (inclusive)</param>
        /// <param name="stopVal">Max range of the list (exclusive)</param>
        /// <returns>Sorted array of unique random integers</returns>
        private static int[] RandomSortedIntegers(int size, int startVal, int stopVal)
        {
            if (size <= 0 || stopVal <= startVal)
                throw new ArgumentException("Invalid size or range.");

            int range = stopVal - startVal;

            if (size > range)
                throw new ArgumentException("Size cannot be greater than the range.");

            Random r = new Random();
            int[] array = new int[size];

            for (int i = 0; i < size; i++)
            {
                int randomNumber = r.Next(startVal, stopVal);

                // Ensure the generated number is unique
                while (Array.IndexOf(array, randomNumber) != -1)
                {
                    randomNumber = r.Next(startVal, stopVal);
                }

                array[i] = randomNumber;
            }

            Array.Sort(array);
            return array;
        }
    }
}
