using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Template_P3
{
    public class Camera
    {
        public Vector3 cameraPos; //camera position
        public Vector3 cameraDir; //camera direction
        public Vector3 up;
        public Vector3 right;
        public float hRotate = 0;
        public float vRotate = 0;
        public float zRotate = 0;

        public Camera(Vector3 cameraPos, Vector3 cameraDir)
        {
            this.cameraPos = cameraPos;
            this.cameraDir = cameraDir;
            up = new Vector3(0, 1, 0);
            right = Vector3.Cross(up, cameraDir).Normalized();
            up = Vector3.Cross(cameraDir, right);
        }

        public void ExecuteCommand()
        {
            Move();
            Rotate();
            Tilt();
            ZRotate();
            CalcVectors();
        }

        public void Move()
        {
            // prepare matrix for vertex shader
            if (Keyboard.GetState().IsKeyDown(Key.D)) cameraPos += 0.9f * right;
            if (Keyboard.GetState().IsKeyDown(Key.A)) cameraPos -= 0.9f * right;
            if (Keyboard.GetState().IsKeyDown(Key.W)) cameraPos -= 0.9f * cameraDir;
            if (Keyboard.GetState().IsKeyDown(Key.S)) cameraPos += 0.9f * cameraDir;
            if (Keyboard.GetState().IsKeyDown(Key.Space)) cameraPos.Y -= 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft)) cameraPos.Y += 0.9f;
        }

        public void Rotate()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Left)) { cameraDir += right * 0.1f; cameraDir.Normalize(); }
            if (Keyboard.GetState().IsKeyDown(Key.Right)) { cameraDir -= right * 0.1f; cameraDir.Normalize(); }
        }

        public void Tilt()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Up)) { cameraDir += up * 0.1f; cameraDir.Normalize(); };
            if (Keyboard.GetState().IsKeyDown(Key.Down)) { cameraDir -= up * 0.1f; cameraDir.Normalize(); }
        }

        public void ZRotate()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Q))
            {
                zRotate += 0.1f;
                right = new Vector3((float)(right.X * Math.Cos(zRotate) + right.Y * Math.Sin(zRotate)), (float)(-right.X * Math.Sin(zRotate) + right.Y * Math.Cos(zRotate)), right.Z);
                up = Vector3.Cross(cameraDir, right);
            }
            if (Keyboard.GetState().IsKeyDown(Key.E))
            {
                zRotate -= 0.1f;
                right = new Vector3((float)(right.X * Math.Cos(zRotate) + right.Y * Math.Sin(zRotate)), (float)(-right.X * Math.Sin(zRotate) + right.Y * Math.Cos(zRotate)), right.Z);
                up = Vector3.Cross(cameraDir, right);
            }
        }

        public void CalcVectors()
        {
                        
            up = new Vector3(0, 1, 0);
            right = Vector3.Cross(up, cameraDir);
            up = Vector3.Cross(cameraDir, right);
        }
    }
}
