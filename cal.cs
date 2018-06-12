using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    class cal
    {
        public float x, y, z, w;

      public   void calu(float x , float y, float z, float w)
        {
            x = this.x;
            y = this.y;
            z = this.z;
            w = this.w;
        }

        public double num(CameraSpacePoint ankleft)
        {

             double  numerator = x * ankleft.X + y * ankleft.Y + z * ankleft.Z + w;
          double    denominator = Math.Sqrt(x * x + y * y + z * z);
             double a = Math.Round(numerator / denominator , 3);


            return a;
        }


    }


}
