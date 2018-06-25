﻿using System.Diagnostics;
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
            if (Keyboard.GetState().IsKeyDown(Key.D)) cameraPos.X -= 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.A)) cameraPos.X += 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.W)) cameraPos.Z += 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.S)) cameraPos.Z -= 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.Space)) cameraPos.Y -= 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft)) cameraPos.Y += 0.9f;
        }

        public void Rotate()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Left)) hRotate -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Key.Right)) hRotate += 0.1f;
        }

        public void Tilt()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Up)) vRotate -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Key.Down)) vRotate += 0.1f;
        }

        public void ZRotate()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Q)) zRotate -= 0.05f;
            if (Keyboard.GetState().IsKeyDown(Key.E)) zRotate += 0.05f;
        }

        public void CalcVectors()
        {   

            up = new Vector3(0, 1, 0);
            right = Vector3.Cross(up, cameraDir);
            up = Vector3.Cross(cameraDir, right);
        }
    }
}
