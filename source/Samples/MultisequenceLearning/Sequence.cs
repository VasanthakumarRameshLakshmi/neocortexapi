using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApproveMultisequenceLearning
{
    public class Sequence
    {
        public String name { get; set; }
        public int[] data { get; set; }
    }

    public class ConfigOfSequence
    {
        public int count { get; set; }
        public int size { get; set; }
        public int testSize { get; set; }
        public int startVal { get; set; }
        public int endVal { get; set; }

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
