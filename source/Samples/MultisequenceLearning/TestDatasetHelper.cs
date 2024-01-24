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
        
        public static List<Sequence> CreateTestDataset(int numberOfSequence, int size, int testSize, int startVal, int endVal, List<Sequence> sequences)
        {
            if (!IsCreateTestDatasetValid(numberOfSequence, size, testSize, startVal, endVal))
                return null;

            List<Sequence> testSequences = new List<Sequence>();

            for(int i = 0; i < numberOfSequence; i++)
            {
                Sequence sequence = SelectRandomSequence(sequences);
                Sequence testSequence = CreateTestSequence(testSize, size, sequence, $"T{i+1}");
                testSequences.Add(testSequence);
            }

            return testSequences;
        }

        private static Sequence SelectRandomSequence(List<Sequence> sequences)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            Sequence[] sequence = sequences.ToArray();
            int selectSequenceNo = random.Next(0,sequence.Length);
            Sequence selectSequence = sequence[selectSequenceNo];
            
            return selectSequence;
        }

        private static Sequence CreateTestSequence(int testSize, int size, Sequence sequence, string sequenceName)
        {
            Sequence newSubSequence = new Sequence();
            newSubSequence.name = sequenceName;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int selectIndexNo = random.Next(0, size-testSize-1); // is that the array index doesnot go out of bound later
            int[] value = new int[testSize];

            for (int i = testSize, j = 0; i > 0; i--, j++)
            {
                value[j] = sequence.data[selectIndexNo+j];
            }
            newSubSequence.data = value;

            return newSubSequence;
        }

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
