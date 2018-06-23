using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Camera
    {
        public Vector3 cameraPos; //camera position
        public Vector3 cameraDir; //camera direction
        public float hRotate = 0;
        public float vRotate = 0;

        public Camera(Vector3 cameraPos, Vector3 cameraDir)
        {
            this.cameraPos = cameraPos;
            this.cameraDir = cameraDir;
        }

        public void ExecuteCommand()
        {
            Move();
            Rotate();
            Tilt();
        }

        public void Move()
        {
            // prepare matrix for vertex shader
            if (Keyboard.GetState().IsKeyDown(Key.D)) cameraPos.X -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Key.A)) cameraPos.X += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Key.W)) cameraPos.Z += 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.S)) cameraPos.Z -= 0.9f;
            if (Keyboard.GetState().IsKeyDown(Key.Space)) cameraPos.Y -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Key.ShiftLeft)) cameraPos.Y += 0.1f;
        }

        public void Rotate()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Left)) hRotate -= 0.015f;
            if (Keyboard.GetState().IsKeyDown(Key.Right)) hRotate += 0.015f;
        }

        public void Tilt()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Up)) vRotate -= 0.015f;
            if (Keyboard.GetState().IsKeyDown(Key.Down)) vRotate += 0.015f;
        }
    }
}
