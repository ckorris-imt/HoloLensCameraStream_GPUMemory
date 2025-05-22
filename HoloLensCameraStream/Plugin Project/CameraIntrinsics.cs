//  
// Copyright (c) 2017 Vulcan, Inc. All rights reserved.  
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
//

using System.Text;

namespace HoloLensCameraStream
{
    /// <summary>
    /// Contains information on camera intrinsic parameters.
    /// Note: This class wraps logic in Windows.Media.Devices.Core.CameraIntrinsics for use in Unity.
    /// </summary>
    public class CameraIntrinsics
    {
        public uint ImageWidth;            // image width of the camera, in pixels.
        public uint ImageHeight;           // image height of the camera, in pixels.
        public uint FocusDistance;
        public float FocalLengthX;         // focal length x.
        public float FocalLengthY;         // focal length y.
        public float PrincipalPointX;      // principal point x.
        public float PrincipalPointY;      // principal point y.
        public float RadialDistK1;         // radial distortion coefficient k1.
        public float RadialDistK2;         // radial distortion coefficient k2.
        public float RadialDistK3;         // radial distortion coefficient k3.
        public float TangentialDistP1;     // tangential distortion coefficient p1.
        public float TangentialDistP2;     // tangential distortion coefficient p2.
        public float[] UndistortedProjectionTransform;

        /// <summary>
        /// CameraIntrinsics constructor
        /// </summary>
        /// <param name="imageWidth">image width in pixels</param>
        /// <param name="imageHeight">image height in pixels</param>
        /// <param name="focalLengthX">focal length x for the camera</param>
        /// <param name="focalLengthY">focal length y for the camera</param>
        /// <param name="principalPointX">principal point x for the camera </param>
        /// <param name="principalPointY">principal point y for the camera </param>
        /// <param name="radialDistK1">radial distortion k1 for the camera</param>
        /// <param name="radialDistK2">radial distortion k2 for the camera</param>
        /// <param name="radialDistK3">radial distortion k3 for the camera</param>
        /// <param name="tangentialDistP1">tangential distortion p1 for the camera</param>
        /// <param name="tangentialDistP2">tangential distortion p2 for the camera</param>
        public CameraIntrinsics(
            uint imageWidth,
            uint imageHeight,
            uint focusDistance,
            float focalLengthX,
            float focalLengthY,
            float principalPointX,
            float principalPointY,
            float radialDistK1,
            float radialDistK2,
            float radialDistK3,
            float tangentialDistP1,
            float tangentialDistP2,
            System.Numerics.Matrix4x4 undistortedProjectionTransform)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            FocusDistance = focusDistance;
            FocalLengthX = focalLengthX;
            FocalLengthY = focalLengthY;
            PrincipalPointX = principalPointX;
            PrincipalPointY = principalPointY;
            RadialDistK1 = radialDistK1;
            RadialDistK2 = radialDistK2;
            RadialDistK3 = radialDistK3;
            TangentialDistP1 = tangentialDistP1;
            TangentialDistP2 = tangentialDistP2;
            UndistortedProjectionTransform = Matrix4x4ToFloatArray(undistortedProjectionTransform);
        }

        /// <summary>
        /// CameraIntrinsics constructor
        /// </summary>
        /// <param name="intrinsics">Windows.Media.Devices.Core.CameraIntrinsics</param>
        public CameraIntrinsics(Windows.Media.Devices.Core.CameraIntrinsics intrinsics, uint focusDistance)
        {
            ImageWidth = intrinsics.ImageWidth;
            ImageHeight = intrinsics.ImageHeight;
            FocusDistance = focusDistance;
            FocalLengthX = intrinsics.FocalLength.X;
            FocalLengthY = intrinsics.FocalLength.Y;
            PrincipalPointX = intrinsics.PrincipalPoint.X;
            PrincipalPointY = intrinsics.PrincipalPoint.Y;
            RadialDistK1 = intrinsics.RadialDistortion.X;
            RadialDistK2 = intrinsics.RadialDistortion.Y;
            RadialDistK3 = intrinsics.RadialDistortion.Z;
            TangentialDistP1 = intrinsics.TangentialDistortion.X;
            TangentialDistP2 = intrinsics.TangentialDistortion.Y;
            UndistortedProjectionTransform = Matrix4x4ToFloatArray(intrinsics.UndistortedProjectionTransform);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < UndistortedProjectionTransform.Length; i++)
            {
                stringBuilder.Append(UndistortedProjectionTransform[i].ToString("G4"));
                if (i < UndistortedProjectionTransform.Length - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            return $"Image Width:{ImageWidth.ToString("G4")}, " +
                $"Image Height:{ImageHeight.ToString("G4")}," + 
                $"Focal Length X:{FocalLengthX.ToString("G4")}, " +
                $"Focal Length Y:{FocalLengthY.ToString("G4")}, " +
                $"Principal Point X:{PrincipalPointX.ToString("G4")}, " +
                $"Principal Point Y:{PrincipalPointY.ToString("G4")}, " +
                $"Radial Distortion K1:{RadialDistK1.ToString("G4")}, " +
                $"Radial Distortion K2:{RadialDistK2.ToString("G4")}, " +
                $"Radial Distortion K3:{RadialDistK3.ToString("G4")}, " +
                $"Tangential Distortion P1:{TangentialDistP1.ToString("G4")} " +
                $"Tangential Distortion P2:{TangentialDistP2.ToString("G4")} " + 
                $"Undistored Proj Matrix: {stringBuilder.ToString()}";
        }

        private float[] Matrix4x4ToFloatArray(System.Numerics.Matrix4x4 matrix)
        {
            float[] result = new float[16]
            {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            };

            return result;
        }
    }
}