//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.BodyBasics
{
    using System;
    using System.Windows.Data;
    using Microsoft.Kinect.Toolkit;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Linq;
    using System.Net;
    using System.Collections;
    using System.Timers;
    using System.Windows.Threading;
    using System.Threading;
    using System.Threading.Tasks;



    ///// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public int intime = 6;
        public int timer = 0;
        List<double> list2 = new List<double>();
        DispatcherTimer dt = new DispatcherTimer();
        DispatcherTimer dt2 = new DispatcherTimer();

        double numerator;

        int ans;
        int ans2;

        double anshead;
        double ansspinebase;
        double numnumeratorhead;
        double numnumeratorspinebase;
        double denominator;
        public double[] slidhead = new double[100];
        public double[] slidspine = new double[100];
        public double[] sliding = new double[100];
        public double[] slidinghead = new double[100];
        public double[] slidingspine = new double[100];
        public double datatt = 0;
        public double datattspine = 0;
        public static int countaa = 0;

        public double[] printmin(double[] arr, int n)
        {
            int k = 60;
            int bfslide = 80;
            int count = 20;
            int j;
            double min;
            for (int i = 0; i < n - k; i++)
            {
                min = arr[i];
                for (j = 1; j < k; j++)
                {
                    if (arr[i + j] < min)
                    {
                        min = arr[i + j];
                    }
                }
                sliding[i] = Math.Round(min, 3);
            }
            //    Console.WriteLine(count);
            //   Console.WriteLine(arr.Length - count);
            //  Console.WriteLine(arr.Length);
            for (int i = arr.Length - count; i < arr.Length; i++)
            {
                arr[i] = sliding[i - bfslide];
            }
            count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                count++;
            }
            //  Console.WriteLine(count);
            return arr;

        }



        private void dtTicker(object sender, EventArgs e)
        {
            //  list2.Add(a);

            intime--;
            timer++;
            time.Text = intime.ToString();
            //Console.WriteLine(timer);

            Console.WriteLine(intime);
            if (intime == 0)
            {
                for (int i = 0; i < listhead.Count; i++)
                {
                    slidhead[i] = listhead[i];
                    // Console.WriteLine(slidhead[i]);
                    slidspine[i] = listspinebase[i];
                }

                ans = slidhead.Length - listhead.Count;

                if (ans != 20)
                {
                    for (int i = slidhead.Length - ans; i <= 80; i++)
                    {
                        slidhead[i] = slidhead[slidhead.Length - ans - 1];
                        slidspine[i] = slidspine[slidhead.Length - ans - 1];
                    }
                }
                using (TextWriter writer = File.CreateText(@"C:\Users\Goon\Desktop\sppj2\Falldetection-test\bin\AnyCPU\Debug\data.csv"))
                {


                    //  Console.WriteLine(listhead.Count());

                    slidinghead = printmin(slidhead, slidhead.Length);
                    slidingspine = printmin(slidspine, slidspine.Length);

                    for (int i = 0; i < slidinghead.Length; i++)
                    {
                        // Console.WriteLine(slidingspine[i]);
                    }

                    for (int i = 0; i < slidinghead.Length; i++)
                    {
                        var it = slidinghead[i].ToString();
                        var it2 = slidingspine[i].ToString();
                        writer.WriteLine(it + "," + it2);

                    }
                }
                this.Close();
            }
        }

        private void dataTicker(object sender, EventArgs e)
        {
            try
            {

                Parallel.Invoke(() =>
                {

                    anshead = Math.Round(numnumeratorhead / denominator, 3);
                    ansspinebase = Math.Round(numnumeratorspinebase / denominator, 3);



                });
                if (intime < 6)
                {
                    //  timer++;
                    listhead.Add(anshead);
                    listspinebase.Add(ansspinebase);

                    //   Console.WriteLine(anshead);
                    //  Console.WriteLine(ansspinebase);

                }
            }
            catch (AggregateException a)
            {
                Console.WriteLine(a);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            test t2 = new test();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
            //    Console.WriteLine(timer);
            dt.Start();

            dt2.Interval = TimeSpan.FromSeconds(0.05);
            dt2.Tick += dataTicker;
            dt2.Start();


            /*  /*    foreach (object list2 in list1)
                  {
                      Console.WriteLine(list2);


                  } */
            //  list1 = list1.Distinct().ToList();

            /* using (TextWriter writer = File.CreateText(@"C:\Users\Goon\Desktop\sppj2\Falldetection-test\bin\AnyCPU\Debug\data.csv"))
             {
                 foreach (object list3 in list1)
                 {
                     writer.WriteLine(list3);
                 }


             } 
           
           
           t2.save(list1);
            this.Close();
           list1.Clear();
          this.Close();*/

        }

        List<double> listhead = new List<double>();
        List<double> listspinebase = new List<double>();
        //List<double> list1 = new List<double>();
        // ArrayList list1 = new ArrayList();

        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HandSize = 30; //ขนาดวงกลมของมือ ท้ังกำมือ และ แบมือ

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3; // ขนาดของตัวข้อต่อ

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Constant for clamping Z values of camera space points from being negative
        /// </summary>
        private const float InferredZPositionClamp = 0.1f;

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as closed
        /// </summary>
        private readonly Brush handClosedBrush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as opened
        /// </summary>
        private readonly Brush handOpenBrush = new SolidColorBrush(Color.FromArgb(128, 0, 255, 0));

        /// <summary>
        /// Brush used for drawing hands that are currently tracked as in lasso (pointer) position
        /// </summary>
        private readonly Brush handLassoBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255));

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;

        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> bones;

        /// <summary>
        /// Width of display (depth space)
        /// </summary>
        private int displayWidth;

        /// <summary>
        /// Height of display (depth space)
        /// </summary>
        private int displayHeight;

        /// <summary>
        /// List of colors for each body tracked
        /// </summary>
        private List<Pen> bodyColors;

        /// <summary>
        /// Current status text to display
        /// </summary>
        private string statusText = null;

        private Floor _floor = null;
        private Body _body = null;
        public Vector4 FloorClipPlane { get; }

        public int i = 0;
        double a;
        int y = 0;
       // public double[] floor;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            // one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();

            // get the coordinate mapper
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            // get the depth (display) extents
            FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            // get size of joint space
            this.displayWidth = frameDescription.Width;
            this.displayHeight = frameDescription.Height;

            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // a bone defined as a line between two joints
            this.bones = new List<Tuple<JointType, JointType>>();

            // Torso
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));
            //test


            
            //


            // populate body colors, one for each BodyIndex
            this.bodyColors = new List<Pen>();

            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));

            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText  // เช็คว่า kinect กำลัง online อยู่ไหม 
                                                            : Properties.Resources.NoSensorStatusText;

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup); //สร้าง รูปที่sensor ทำงาน แล้วคัดกรองให้เหลือแต่รูปโครงก้าง

            // use the window object as the view model in this simple example
            this.DataContext = this;

            // initialize the components (controls) of the window
            this.InitializeComponent(); //สร้างหน้าต่าง component 

        }
        public class Floor {
            public float x { get; internal set; }
            public float y { get; internal set; }
            public float z { get; internal set; }
            public float w { get; internal set; }

            public Floor(Vector4 floorClipPlane) {
                x = floorClipPlane.X;
                y = floorClipPlane.Y;
                z = floorClipPlane.Z;
                w = floorClipPlane.W;

            }
            public double DistanceFrom(CameraSpacePoint point)
            {
                double numerator = x * point.X + y * point.Y + z * point.Z + w;
                double denominator = Math.Sqrt(x * x + y * y + z * z);

                Console.WriteLine(numerator / denominator);

                return numerator / denominator;
            }

        }


        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the bitmap to display
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }
        }


        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText")); //สร้าง status ของการทำงาน kinect 
                    }
                }
            }
        }

        /// <summary>
        /// Execute start up tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
            }


        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
           
        double con;
            bool dataReceived = false;
            string message = "No Skeleton Data";

       
             
        float x1;
            float y1;
            float z1;
            float w1;
            double aa;
           


            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Floor floor = new Floor(FloorClipPlane);


                    //floor.DistanceFrom();


                    Vector4 floorClipPlane = bodyFrame.FloorClipPlane;
                    x1 = floorClipPlane.X;
                    y1 = floorClipPlane.Y;
                    z1 = floorClipPlane.Z;
                    w1 = floorClipPlane.W;


                    con = Math.Sqrt(x1 * x1 + y1 * y1 + z1 * z1);




                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;

                }
            }


            if (dataReceived)
            {

                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
                    {
                        

                                         // Draw a transparent background to set the render size
                         dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                        int penIndex = 0;

                        foreach (Body body in this.bodies)
                        {
                           
                            Pen drawPen = this.bodyColors[penIndex++];

                            if (body.IsTracked)
                            {
                                this.DrawClippedEdges(body, dc);

                                IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                                // convert the joint points to depth (display) space
                                Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                                foreach (JointType jointType in joints.Keys)
                                {


                                    // sometimes the depth(Z) of an inferred joint may show as negative
                                    // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)

                                    CameraSpacePoint position = joints[jointType].Position;
                                    if (position.Z < 0)
                                    {
                                        position.Z = InferredZPositionClamp;
                                    }

                                    Vector4 floorClipPlane = bodyFrame.FloorClipPlane;
                                    float X = floorClipPlane.X;
                                    float Y = floorClipPlane.Y;
                                    float Z = floorClipPlane.Z;
                                    float W = floorClipPlane.W;


                                    
                                  //  CameraSpacePoint ee = joints[JointType.WristLeft].Position;
                                  /*  CameraSpacePoint ankleft = joints[JointType.AnkleLeft].Position;
                                    CameraSpacePoint ankright = joints[JointType.AnkleRight].Position;
                                    CameraSpacePoint elbowleft = joints[JointType.ElbowLeft].Position;
                                    CameraSpacePoint elbowright = joints[JointType.ElbowRight].Position;
                                    CameraSpacePoint footleft = joints[JointType.FootLeft].Position;
                                    CameraSpacePoint footright = joints[JointType.FootRight].Position;
                                    CameraSpacePoint haneleft = joints[JointType.HandLeft].Position;
                                    CameraSpacePoint handright = joints[JointType.HandRight].Position;
                                    CameraSpacePoint handtipleft = joints[JointType.HandTipLeft].Position;
                                    CameraSpacePoint handtipright = joints[JointType.HandTipRight].Position;*/
                                    CameraSpacePoint head = joints[JointType.Head].Position;
                             /*       CameraSpacePoint hipleft = joints[JointType.HipLeft].Position;
                                    CameraSpacePoint hipright = joints[JointType.HipRight].Position;
                                    CameraSpacePoint kneeleft = joints[JointType.KneeLeft].Position;
                                    CameraSpacePoint kneeright = joints[JointType.KneeRight].Position;
                                    CameraSpacePoint neck = joints[JointType.Neck].Position;
                                    CameraSpacePoint shoulderleft = joints[JointType.ShoulderLeft].Position;
                                    CameraSpacePoint shouluderright = joints[JointType.ShoulderRight].Position; */
                                    CameraSpacePoint spinebase = joints[JointType.SpineBase].Position;
                                    /*     CameraSpacePoint spinemid = joints[JointType.SpineMid].Position;
                                         CameraSpacePoint spineshoulder = joints[JointType.SpineShoulder].Position;
                                         CameraSpacePoint thumbleft = joints[JointType.ThumbLeft].Position;
                                         CameraSpacePoint  thumbright = joints[JointType.ThumbRight].Position;
                                         CameraSpacePoint wristleft = joints[JointType.WristLeft].Position;
                                         CameraSpacePoint wristright = joints[JointType.WristRight].Position; */

                                   numnumeratorhead = X * head.X + Y * head.Y + Z * head.Z + W;
                                   numnumeratorspinebase = X * spinebase.X + Y * spinebase.Y + Z * spinebase.Z + W;
                                   denominator = Math.Sqrt(X * X + Y * Y + Z * Z);

                                    /*       try
                                           {
                                               if (intime<6)
                                               {
                                                 //  timer++;
                                                   listhead.Add(anshead);
                                                   listspinebase.Add(ansspinebase);

                                               }
                                               Parallel.Invoke(() =>
                                               {
                                                   double numnumeratorhead = X * head.X + Y * head.Y + Z * head.Z + W;
                                                   double numnumeratorspinebase = X * spinebase.X + Y * spinebase.Y + Z * spinebase.Z + W;
                                                   double denominator = Math.Sqrt(X * X + Y * Y + Z * Z);
                                                     anshead = Math.Round(numnumeratorhead/denominator , 3);
                                                      ansspinebase = Math.Round(numnumeratorspinebase/denominator , 3);



                                               }


                                                   );
                                           }
                                           catch(AggregateException a)
                                           {
                                               Console.WriteLine(a);
                                           }
                                     */



                                    //      numerator = X * ankleft.X + Y * ankleft.Y + Z * ankleft.Z + W;
                                    //     denominator = Math.Sqrt(X * X + Y * Y + Z * Z);
                                    //    a = Math.Round(numerator / denominator , 3);


                                    /*     double numerator = X * ee.X + Y * ee.Y + Z * ee.Z + W;
                                         double denominator = Math.Sqrt(X * X + Y * Y + Z * Z);
                                         double ans = numerator / denominator;
                                        a = Math.Round(ans, 3);
                                         //  Console.WriteLine(a);



                                             list1.Add(a);
                                           //  Console.WriteLine(a);


                                         //  floor1[i] = a;

                                         //floor[i] = a;
                                         /*  if (a <= 0.65 && a >= 0.55)
                                           {
                                               y++;
                                           }
                                           if (y==2000)
                                           {
                                               this.Close();
                                           }
                                           Console.WriteLine(y); ออกกำลังกาย */

                                    /*        if (a >= -1.43 && a < -1.03)
                                            {
                                                y++;
                                            }
                                            if (y == 2000)
                                            {
                                             //   this.Close();
                                            }
                                            Console.WriteLine(y); */







                                    var spine = body.Joints[JointType.SpineMid];



                                    DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                                    jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);

                                    message = string.Format("SKelton: X:{0:0.0} Y:{1:0.0} Z:{2:0.0}",   // สร้าง postion บนร่างกาย ด้วย x,y,z 
                                                                                                        //this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
                                                                                                        //this.bones.(new Tuple<JointType,JointType>(JointType.SpineBase , JointType.SpineBase)),
                             spine.Position.X,
                             spine.Position.Y,
                             spine.Position.Z);
                                    // position.X,
                                    // position.Y,
                                    // position.Z);
                                    
                                }

                                

                                //Console.WriteLine(i);
                                i++;

                                

                                this.DrawBody(joints, jointPoints, dc, drawPen);

                                this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                                this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                                
                            }
                        }
                        // prevent drawing outside of our render area
                        this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
                      
                    }
                }

            }
            TextH.Text = message;
           


        }


        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            // Draw the bones
            foreach (var bone in this.bones)
            {
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            }

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// /// <param name="drawingPen">specifies color to draw a specific bone</param>
        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            {
                drawPen = drawingPen;
            }

            drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition, DrawingContext drawingContext)
        {
            string sta;
            switch (handState)
            {
                case HandState.Closed:
                    drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
                    sta = "Alert: Close Hand"; // alert เมื่อกำมือ
                    Texts.Text = sta;

                    /*string UTL = "http://localhost/line/index.php";
                     WebClient client = new WebClient();
                     client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                    Stream data = client.OpenRead(UTL);
                      StreamReader reader = new StreamReader(data);
                     string s = reader.ReadToEnd();
                          this.Close(); */

                    break;

                case HandState.Open:
                    drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
                    sta = "Alert: Open Hand"; // alert เมื่อแบมือ
                    Texts.Text = sta;
                    break;

                case HandState.Lasso:
                    drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
                    break;
                
            }
        }

        /// <summary>
        /// Draws indicators to show which edges are clipping body data
        /// </summary>
        /// <param name="body">body to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawClippedEdges(Body body, DrawingContext drawingContext)
        {
            FrameEdges clippedEdges = body.ClippedEdges;

            if (clippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, this.displayHeight - ClipBoundsThickness, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, this.displayWidth, ClipBoundsThickness));
            }

            if (clippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, this.displayHeight));
            }

            if (clippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(this.displayWidth - ClipBoundsThickness, 0, ClipBoundsThickness, this.displayHeight));
            }
        }

        /// <summary>
        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }

    
    }
}
