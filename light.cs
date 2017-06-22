using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    public class Light
    {
        public Vector3 lightPosition;
        public Vector3 lightColor;
        public string name;
        public string colorname;
        Shader shader;
        public int lightIDp;
        public int lightIDc;
        public bool on;

        public Light(Vector3 lightPosition2, Vector3 lightColor2, string name2, string colorname2, Shader s, bool on)
        {
            lightPosition = lightPosition2;
            lightColor = lightColor2;
            if (!on) lightColor = Vector3.Zero;
            name = name2;
            colorname = colorname2;
            shader = s;
            lightIDp = GL.GetUniformLocation(shader.programID, name);
            lightIDc = GL.GetUniformLocation(shader.programID, colorname);
        }
    }
}
