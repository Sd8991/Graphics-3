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
        SceneGraph nodeParent;                                                      // this is the parent of the current mesh
        public Mesh nodeMesh;                                                       // the mesh of the mesh
        Texture nodeTexture;                                                        // the texture of the mesh
        public List<SceneGraph> nodeChildren = new List<SceneGraph>();              // a list of children of the mesh
           
        public SceneGraph(SceneGraph p, Mesh mesh, Texture texture)
        {
            nodeParent = p;
            nodeMesh = mesh;
            nodeTexture = texture;

            if (p != null)
                p.nodeChildren.Add(this);   // if the mesh has a parent, add the mesh to the parent mesh's children list
        }

        public void Render( Shader shader, Matrix4 transform, Matrix4 toWorld)
        {
            Matrix4 preTransform = Matrix4.Identity * nodeMesh.mTransform;  // calculate part of the transform
            transform = preTransform * transform;                           // calculate transform
            toWorld = preTransform * toWorld;                               // calculate transform in world space

            if (nodeMesh != null)
            {
                nodeMesh.Render(shader, transform, toWorld, nodeTexture);   // render the mesh
            }

            foreach (SceneGraph child in nodeChildren)
            {
                child.Render(shader, transform, toWorld);                   // render the meshes of all your children
            }
        }
    }
}

