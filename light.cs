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

        public Light(Vector3 lightPosition2, Vector3 lightColor2, string name2, string colorname2, Shader s)
        {
            lightPosition = lightPosition2;
            lightColor = lightColor2;
            name = name2;
            colorname = colorname2;
            shader = s;
            lightIDp = GL.GetUniformLocation(shader.programID, name);
            lightIDc = GL.GetUniformLocation(shader.programID, colorname);
            //GL.UseProgram(shader.programID);
            //GL.Uniform3(lightIDp, lightPosition);
            //GL.Uniform3(lightIDc, lightColor);

        }
    }
}
