//  
// Copyright (c) 2017 Vulcan, Inc. All rights reserved.  
// Licensed under the Apache 2.0 license. See LICENSE file in the project root for full license information.
//

namespace HoloLensCameraStream
{
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
    }
}