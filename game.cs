using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{

    class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh /*mesh*/ floor, teacup, teapot;            // a mesh to draw using OpenGL
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Camera c;
        public Dictionary<string, Mesh> meshes; // dictionary to store the meshes for use in the Scenegraph class
        public List<Light> lights;
        public Vector3[] lightPoss;
        public Vector3[] lightCols; 

        Stopwatch timer;                        // timer for measuring frame duration
        SceneGraph scenegraph;                  // scenegraph class to use for rendering
        Shader shader;                          // shader to use for rendering
        Shader postproc;                        // shader to use for post processing
        Texture wood;                           // textures to use for rendering
        Texture porcelain;
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        float d = 0, b = 0;                    // camera rotation angles


        // initialize
        public void Init()
        {
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            porcelain = new Texture("../../assets/porcelain_texture.jpg");
            c = new Camera(new Vector3(0, -4, -15), new Vector3(0, 0, 1));
            // load teapot
            //mesh = new Mesh( "../../assets/teapot.obj", 1, 0, new Vector3(0, 1, -5), porcelain);
            floor = new Mesh("../../assets/floor.obj", 0, -1, new Vector3(0, 1, -5), wood);
            teapot = new Mesh("../../assets/teapot.obj", 2, 0, new Vector3(5, 0, 0), porcelain);

            meshes = new Dictionary<string, Mesh>();
            //meshes.Add("mesh", mesh);
            meshes.Add("floor", floor);
            meshes.Add("teapot", teapot);

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");

            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();
            // setting up the SceneGraph
            scenegraph = new SceneGraph(meshes);
            // set the lights
            int ambientColor = GL.GetUniformLocation(shader.programID, "ambientColor");
            lights = new List<Light>();
            lights.Add(new Light(new Vector3(20, 0, 0), new Vector3(1f, 1f, 1f), "lightPos", "lightCol", 0, shader, false));
            lights.Add(new Light(new Vector3(-3, 0, 0), new Vector3(0, 0, 1), "lightPos", "lightCol", 1, shader, true));
            lights.Add(new Light(new Vector3(-5, 0, 10), new Vector3(0.4f, 0.4f, 0.4f), "lightPos", "lightCol", 2, shader, false));
            lights.Add(new Light(new Vector3(5, 0, 0), new Vector3(1, 1, 1), "lightPos", "lightCol", 3, shader, false));

            lightPoss = new Vector3[4];
            lightCols = new Vector3[4];

            for (int i = 0; i < lights.Count; i++)
            {
                lightPoss[i] = lights[i].lightPosition;
                lightCols[i] = lights[i].lightColor;
            }
            GL.UseProgram(shader.programID);
            GL.Uniform3(ambientColor, new Vector3(0.01f, 0.01f, 0.01f));
            for (int i = 0; i < lights.Count; i++)
            {
                GL.Uniform3(lights[i].lightIDp, lightPoss[i]);
                GL.Uniform3(lights[i].lightIDc, lightCols[i]);
            }
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00); 
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;
            if (c.vRotate < 0) c.vRotate = 2 * PI;

            c.ExecuteCommand();

            Matrix4 transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            transform *= Matrix4.CreateTranslation(c.cameraPos);
            transform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), c.vRotate);
            transform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), c.hRotate);
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
            Matrix4 toWorld = transform;

            scenegraph.Render(target, quad, timer, c, shader, postproc, meshes, transform, toWorld);
        }
    }
} // namespace Template_P3