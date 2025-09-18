using System;
using System.Numerics;
using Windows.Media.Capture.Frames;
using Windows.Perception;
using Windows.Perception.Spatial;


namespace HoloLensCameraStream
{
    public class FallbackOffsetHandler
    {
        private Matrix4x4? _sensorToHeadsetTransform = null;

        public void TryUpdatePose(MediaFrameReference mediaFrameReference)
        {
            PerceptionTimestamp perceptionTimestamp
                = PerceptionTimestampHelper.FromSystemRelativeTargetTime(mediaFrameReference.SystemRelativeTime.Value);

            System.Diagnostics.Debug.WriteLine("Got PerceptionTimestamp.");

            SpatialLocation hmdLocation = SpatialLocator.GetDefault()
                .TryLocateAtTimestamp(perceptionTimestamp, mediaFrameReference.CoordinateSystem);

            System.Diagnostics.Debug.WriteLine($"Called TryLocateAtTimestamp. Worked: {hmdLocation != null}");

            if (hmdLocation.Position == null || hmdLocation.Orientation == null)
            {
                System.Diagnostics.Debug.WriteLine("Failed to update pose.");
                return; //Unable to locate the camera at the given timestamp.
            }

            Matrix4x4 hmdPosRelativeToFrame = Matrix4x4.CreateFromQuaternion(hmdLocation.Orientation) *
                Matrix4x4.CreateTranslation(hmdLocation.Position);

            System.Diagnostics.Debug.WriteLine("Made hmdPosRelativeToFrame.");

            Matrix4x4 sensorToHeadsetTransform;
            if (Matrix4x4.Invert(hmdPosRelativeToFrame, out sensorToHeadsetTransform))
            {
                System.Diagnostics.Debug.WriteLine("Updated pose.");
                _sensorToHeadsetTransform = sensorToHeadsetTransform;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to invert pose.");
            }
        }

        public Matrix4x4? EstimateCameraPoseAtTimestamp(TimeSpan timestamp, SpatialCoordinateSystem worldOrigin)
        {
            if (_sensorToHeadsetTransform.HasValue == false)
            {
                return null; //Transform not available.
            }

            PerceptionTimestamp perceptionTimestamp
                = PerceptionTimestampHelper.FromSystemRelativeTargetTime(timestamp);

            SpatialLocator locator = SpatialLocator.GetDefault();

            if(locator.Locatability != SpatialLocatability.PositionalTrackingActive)
            {
                System.Diagnostics.Debug.WriteLine($"EstimateCameraPoseAtTimestamp called, but locatability is {locator.Locatability}.");
                return null; //Locatability is not active, cannot estimate pose.
            }

            SpatialLocation location = locator.TryLocateAtTimestamp(perceptionTimestamp, worldOrigin);

            if (location == null ||
                location.Position == null || location.Orientation == null)
            {
                System.Diagnostics.Debug.WriteLine("EstimateCameraPoseAtTimestamp couldn't get location at timestamp.");
                return null; //Unable to locate the camera at the given timestamp.
            }

            Matrix4x4 headsetPose = Matrix4x4.CreateFromQuaternion(location.Orientation) *
                Matrix4x4.CreateTranslation(location.Position);

            System.Diagnostics.Debug.WriteLine("EstimateCameraPoseAtTimestamp worked.");

            return _sensorToHeadsetTransform * headsetPose;
        }
    }
}
