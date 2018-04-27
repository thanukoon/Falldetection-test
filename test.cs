using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class test
    {
        public  void save(Array arr)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("E:\\data.csv"))
            {
                file.Write(string.Join(",", arr));
                Console.WriteLine("save complete");
            }
            
        }

    }
}
