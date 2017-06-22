using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_P3
{
    class SceneGraph
    {
        public SceneGraph(List<Mesh> scenegraph)
        {
            int derp = -1;
            for (int i = 0; i < scenegraph.Count(); i++)
            {
                if (scenegraph[i].parentIndeks == derp)
                {
                    derp = scenegraph[i].indeks;
                }
            }


        }
        public void Render()
        {

        }
    }
}
