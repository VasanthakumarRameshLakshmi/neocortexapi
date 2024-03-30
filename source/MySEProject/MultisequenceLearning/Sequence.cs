using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApproveMultisequenceLearning
{
    // Model of sequence
    public class Sequence
    {
        // name of sequence
        public String name { get; set; }
        // sequence itself
        public int[] data { get; set; }
    }

    // Model of configuration used to create sequence
    public class ConfigOfSequence
    {
        // count of the sequence
        public int count { get; set; }
        // length/size of each sequence
        public int size { get; set; }
        // length/size of each test sequence
        public int testSize { get; set; }
        // start value of sequence
        public int startVal { get; set; }
        // end value of sequence
        public int endVal { get; set; }

        // constructor
        public ConfigOfSequence(int Count, int Size, 
            int TestSize, int StartVal, int EndVal)
        {
            this.count = Count;
            this.size = Size + 3;
            this.testSize = TestSize;
            this.startVal = StartVal;
            this.endVal = EndVal;
        }
    }
}
