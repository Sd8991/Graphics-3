using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Template_P3
{
    class SceneGraph
    {
        Texture wood = new Texture("../../assets/wood.jpg");
        bool useRenderTarget = true;
        public Dictionary<int, int> Hierarchy;
        public Dictionary<int, Matrix4> Transforms;
        const float PI = 3.1415926535f;         // PI
        public SceneGraph(Dictionary<string, Mesh> meshes)
        {
            Hierarchy = new Dictionary<int, int>();
            Transforms = new Dictionary<int, Matrix4>();
            int derp = -1;
            while (Hierarchy.Count != meshes.Count)
            {
                foreach (Mesh m in meshes.Values)
                {
                    if (m.parentIndeks == derp)
                    {
                        Hierarchy.Add(m.indeks, m.parentIndeks);
                    }
                }
                derp++;
            }
            foreach (Mesh m in meshes.Values)
            {
                int i = m.indeks;
                Matrix4 transform = m.transform;
                Transforms.Add(i, transform);
            }
        }
        public void Render(RenderTarget target, ScreenQuad quad, Stopwatch timer, Camera c, Shader shader, Shader postproc, Dictionary<string, Mesh> meshes, Matrix4 transform, Matrix4 toWorld)
        {          
            // render scene to render target
            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                foreach (Mesh m in meshes.Values)
                {                   
                    m.Render(shader, transform, toWorld, m.teksture);
                }

                // render quad
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                foreach (Mesh m in meshes.Values)
                {                   
                    m.Render(shader, transform, toWorld, wood);
                }
            }

        }
    }
}

