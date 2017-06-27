﻿using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Camera
    {
        public Vector3 cameraPos; //camera position
        public Vector3 cameraDir; //camera direction

        public Camera(Vector3 cameraPos2, Vector3 cameraDir2)
        {
            cameraPos = cameraPos2;
            cameraDir = cameraDir2;

        }
    }
}
