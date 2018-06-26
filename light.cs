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
        public Vector3 realSpecColor;
        public Vector3 lightColor;
        public Vector3 specularLightColor;
        public string name;
        public string colorname;
        public string specularname;
        public int lightIDp;
        public int lightIDc;
        public int lightIDs;
        public int index;
        public bool on = true;
        Shader shader;

        public Light(Vector3 lightPosition, Vector3 lightColor, Vector3 specularLightColor, string name, string colorname, string specularname, int index, Shader s, bool on)
        {
            this.index = index;                                 // light index
            this.lightPosition = lightPosition;                 // light position
            this.lightColor = lightColor;                       // light color
            this.specularLightColor = specularLightColor;       // specular light color
            realLightColor = lightColor;                        // back-up light color, for if the light needs to be toggled on
            realSpecColor = specularLightColor;                 // back-up specular color, for if the light needs to be toggled on
            if (!on)    // light is off
            {
                this.lightColor = Vector3.Zero;
                this.specularLightColor = Vector3.Zero;
            }
            this.name = name;                                   // the name of the light in the fragment shader
            this.colorname = colorname;                         // color name in the fragment shader
            shader = s;
            lightIDp = GL.GetUniformLocation(shader.programID, name + '[' + index + ']');           // sets the position ID for the fragment shader
            lightIDc = GL.GetUniformLocation(shader.programID, colorname + '[' + index + ']');      // sets the color ID for the fragment shader
            lightIDs = GL.GetUniformLocation(shader.programID, specularname + '[' + index + ']');   // sets the specular color ID for the fragment shader
        }

        public void UpdateLight()
        {
            on = !on;   // toggle on/off
            if (on)
            {
                lightColor = realLightColor;                                                            // set the light color on its real color
                specularLightColor = realSpecColor;                                                     // same as above
                lightIDp = GL.GetUniformLocation(shader.programID, name + '[' + index + ']');           // update ID for fragment shader
                lightIDc = GL.GetUniformLocation(shader.programID, colorname + '[' + index + ']');      // same as above
                lightIDs = GL.GetUniformLocation(shader.programID, specularname + '[' + index + ']');   // same as above
            }
            else // light is out
            {
                lightColor = Vector3.Zero;
                specularLightColor = Vector3.Zero;
            }
        }
    }
}
