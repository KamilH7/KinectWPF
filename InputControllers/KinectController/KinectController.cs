using Microsoft.Kinect;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace KinectWPF.Controllers.KinectController
{
    class KinectController : IInputController
    {
        private MainWindow window;
        private KinectSensor kinect = KinectSensor.KinectSensors[0];
        private readonly Pen skeletonPen = new Pen(Brushes.White, 6);
        private DrawingImage imageSource;
        private DrawingGroup drawingGroup;

        public bool IsHoveringOver(Point point)
        {
            return false;
        }

        public void Initialize(MainWindow window)
        {
            this.window = window;
            KinectStart();
        }

        void KinectStart()
        {
            drawingGroup = new DrawingGroup();
            imageSource = new DrawingImage(drawingGroup);
            window.MainImg.Source = imageSource;

            if (kinect.Status == KinectStatus.Connected)
            {
                kinect.Start();
                kinect.SkeletonFrameReady += SkeletonFrameReady;
                kinect.SkeletonStream.Enable();
            }
        }

        void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons;

            using (SkeletonFrame skeletonframe = e.OpenSkeletonFrame())
            {
                if (skeletonframe != null)
                {
                    skeletons = new Skeleton[skeletonframe.SkeletonArrayLength];
                    skeletonframe.CopySkeletonDataTo(skeletons);
                    DrawSkeletons(skeletons);
                }
            }
        }

        private void DrawSkeletons(Skeleton[] skeletons)
        {
            using (DrawingContext dc = drawingGroup.Open())
            {
                dc.DrawRectangle((GestureDetected(skeletons.First()) == true) ? Brushes.Red : Brushes.Black, null, new Rect(0.0, 0.0, 800, 800));

                foreach (Skeleton skel in skeletons)
                {
                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        DrawSkeleton(skel, dc);
                    }
                }
            }
        }

        private void DrawSkeleton(Skeleton skeleton, DrawingContext drawingContext)
        {
            DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);
            DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);
            DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);
            DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);
            DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);
        }

        private bool GestureDetected(Skeleton skeleton)
        {
            var LS = skeleton.Joints[JointType.ShoulderLeft].Position;
            var LE = skeleton.Joints[JointType.ElbowLeft].Position;
            var LH = skeleton.Joints[JointType.HandLeft].Position;

            var RS = skeleton.Joints[JointType.ShoulderRight].Position;
            var RE = skeleton.Joints[JointType.ElbowRight].Position;
            var RH = skeleton.Joints[JointType.HandRight].Position;

            if ((LS.Y > LE.Y)
                && (LH.Y > LS.Y)
                && (LS.X > LE.X)
                && (LS.X > LH.X)
                && (RS.Y > RE.Y)
                && (RS.Y < RH.Y)
                && (RS.X < RE.X)
                && (RS.X < RH.X))
            {
                return true;
            }

            return false;
        }

        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            if (joint0.TrackingState == JointTrackingState.NotTracked || joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            if (joint0.TrackingState == JointTrackingState.Inferred && joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            drawingContext.DrawLine(skeletonPen, SkeletonPointToScreen(joint0.Position), SkeletonPointToScreen(joint1.Position));
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);

            return new Point(depthPoint.X, depthPoint.Y);
        }
    }
}
