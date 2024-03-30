using ApproveMultisequenceLearning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultisequenceLearning
{
    public class TestDatasetHelper
    {
        /// <summary>
        /// Generates a customizable test dataset for sequence-based evaluation
        /// </summary>
        /// <param name="numberOfSequence">number of sequence which needs to be created</param>
        /// <param name="size">size of sequence</param>
        /// <param name="testSize">size of test sequence</param>
        /// <param name="startVal">start value of sequence</param>
        /// <param name="endVal">end value of sequence</param>
        /// <param name="sequences">Name of the sequence</param>
        /// <returns>return the test sequences generated</returns>
        public static List<Sequence> CreateTestDataset(int numberOfSequence, int size, int testSize, int startVal, int endVal, List<Sequence> sequences)
        {
            if (!IsCreateTestDatasetValid(numberOfSequence, size, testSize, startVal, endVal))
                return null;

            List<Sequence> testSequences = new List<Sequence>();

            for(int i = 0; i < numberOfSequence; i++)
            {
                // select random sequence
                Sequence sequence = SelectRandomSequence(sequences);
                // create sub-sequence
                Sequence testSequence = CreateTestSequence(testSize, size, sequence, $"T{i+1}");
                testSequences.Add(testSequence);
            }

            return testSequences;
        }
        
        /// <summary>
        /// Generates a customizable test dataset for sequence-based evaluation
        /// </summary>
        /// <param name="sequences">sequences which needs to be selected</param>
        /// <returns>return the selected sequence</returns>
        private static Sequence SelectRandomSequence(List<Sequence> sequences)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            Sequence[] sequence = sequences.ToArray();
            int selectSequenceNo = random.Next(0,sequence.Length);
            Sequence selectSequence = sequence[selectSequenceNo];
            
            return selectSequence;
        }
        
        /// <summary>
        /// Creates a new subsequence from the test sequence
        /// </summary>
        /// <param name="testSize">size of test sequence</param>
        /// <param name="size">size of sequence</param>
        /// <param name="sequence">original sequence from which the subsequence is created</param>
        /// <param name="sequenceName">Name of the new subsequence</param>
        /// <returns>return new Subsequence created</returns>
        private static Sequence CreateTestSequence(int testSize, int size, Sequence sequence, string sequenceName)
        {
            Sequence newSubSequence = new Sequence();
            newSubSequence.name = sequenceName;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int selectIndexNo = random.Next(0, size-testSize-1); // is that the array index doesnot go out of bound later
            int[] value = new int[testSize];

            // copy sub-sequence in temp array
            for (int i = testSize, j = 0; i > 0; i--, j++)
            {
                value[j] = sequence.data[selectIndexNo+j];
            }
            newSubSequence.data = value;

            return newSubSequence;
        }
       
        /// <summary>
        /// Checks if the parameters for creating dataset is valid or not
        /// </summary>
        /// <param name="numberOfSequence">Number of sequences which needs to be generated</param>
        /// <param name="size">size of sequence</param>
        /// <param name="testSize">size of test sequence</param>
        /// <param name="startVal">>start value of sequence</param>
        /// <param name="endVal">end value of sequence</param>
        /// <returns>return true if parameters are valid</returns>
        private static bool IsCreateTestDatasetValid(int numberOfSequence, int size, int testSize, int startVal, int endVal)
        {
            try
            {
                // we need at least 1 test case to test
                if (numberOfSequence < 1)
                    throw new ArgumentException("You must create atleast 1 test sequence");

                // need atleast size as 3 
                if (testSize <= 2)
                    throw new ArgumentException("Size of each sequence must be atleast 3 to create a piece of sequence");

                // test sequence must be smaller than the actual sequence
                if(size-testSize < 0)
                    throw new ArgumentException("Size of test sequence must smaller the actual size of sequence");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception - IsCreateTestDatasetValid: {ex.Message}");
                return false;
            }


            return true;
        }
    }
}
