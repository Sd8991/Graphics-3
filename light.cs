using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Light
    {
        public Vector3 lightPosition;
        public Vector3 realLightColor;
        public Vector3 lightColor;
        public string name;
        public string colorname;
        public int lightIDp;
        public int lightIDc;
        public int index;
        public bool on = true;
        Shader shader;

        public Light(Vector3 lightPosition, Vector3 lightColor, string name, string colorname, int index, Shader s, bool on)
        {
            this.index = index;
            this.lightPosition = lightPosition;
            this.lightColor = lightColor;
            realLightColor = lightColor;
            if (!on)
                this.lightColor = Vector3.Zero;
            this.name = name;
            this.colorname = colorname;
            shader = s;
            lightIDp = GL.GetUniformLocation(shader.programID, name + '[' + index + ']');
            lightIDc = GL.GetUniformLocation(shader.programID, colorname + '[' + index + ']');
        }

        public void UpdateLight()
        {
            on = !on;
            if (on)
            {
                lightColor = realLightColor;
                lightIDp = GL.GetUniformLocation(shader.programID, name + '[' + index + ']');
                lightIDc = GL.GetUniformLocation(shader.programID, colorname + '[' + index + ']');
            }
            else lightColor = Vector3.Zero;
        }
    }
}
