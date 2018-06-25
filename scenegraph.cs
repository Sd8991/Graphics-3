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
        SceneGraph nodeParent;
        public Mesh nodeMesh;
        Texture nodeTexture;
        public List<SceneGraph> nodeChildren = new List<SceneGraph>();
           
        public SceneGraph(SceneGraph p, Mesh mesh, Texture texture)
        {
            nodeParent = p;
            nodeMesh = mesh;
            nodeTexture = texture;

            if (p != null)
                p.nodeChildren.Add(this);
        }

        public void Render( Shader shader, Matrix4 transform, Matrix4 toWorld)
        {
            Matrix4 preTransform = Matrix4.Identity * nodeMesh.mTransform;
            transform = preTransform * transform;
            toWorld = preTransform * toWorld;

            if (nodeMesh != null)
            {
                nodeMesh.Render(shader, transform, toWorld, nodeTexture);
            }

            foreach (SceneGraph child in nodeChildren)
            {
                child.Render(shader, transform, toWorld);
            }
        }
    }
}

