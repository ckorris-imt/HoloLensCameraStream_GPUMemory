using System;
using System.Numerics;
using Windows.Media.Capture.Frames;
using Windows.Perception;
using Windows.Perception.Spatial;

namespace HoloLensCameraStream
{
    public class FallbackOffsetHandler
    {
        private readonly MediaFrameSourceInfo _mediaFrameSourceInfo;
        private Matrix4x4? _sensorToHeadsetTransform;

        public FallbackOffsetHandler(MediaFrameSourceInfo mediaFrameSourceInfo)
        {
            _mediaFrameSourceInfo = mediaFrameSourceInfo
                ?? throw new ArgumentNullException(nameof(mediaFrameSourceInfo));

            TryUpdatePose();
        }

        public void TryUpdatePose()
        {
            System.Diagnostics.Debug.WriteLine("Try update pose.");

            SpatialCoordinateSystem headsetOrigin = SpatialLocator.GetDefault()
                .CreateStationaryFrameOfReferenceAtCurrentLocation()
                .CoordinateSystem;

            if(_mediaFrameSourceInfo.CoordinateSystem == null)
            {
                System.Diagnostics.Debug.WriteLine("MediaFrameSourceInfo does not have a CoordinateSystem. Cannot compute sensor to headset transform.");
                return;
            }

            Matrix4x4? transform = _mediaFrameSourceInfo.CoordinateSystem.TryGetTransformTo(headsetOrigin);
            if (transform.HasValue)
            {
                _sensorToHeadsetTransform = transform.Value;
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

            SpatialLocation location = SpatialLocator.GetDefault()
                .TryLocateAtTimestamp(perceptionTimestamp, worldOrigin);

            if (location.Position == null || location.Orientation == null)
            {
                return null; //Unable to locate the camera at the given timestamp.
            }

            Matrix4x4 headsetPose = Matrix4x4.CreateFromQuaternion(location.Orientation) *
                Matrix4x4.CreateTranslation(location.Position);

            return _sensorToHeadsetTransform * headsetPose;
        }
    }
}
