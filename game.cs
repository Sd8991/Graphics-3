using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{

    class Game
    {
        // screen, camera and root node
        public Surface screen;                  // background surface for printing etc.    
        SceneGraph floorNode;
        Camera c;

        // lights
        public List<Light> lights;
        public Vector3[] lightPoss;
        public Vector3[] lightCols;

        // Meshes and Textures
        Mesh teacup, floor, teapot;             // a mesh to draw using OpenGL         
        Texture wood, porcelain;                // textures to use for rendering

        // Rendering stuff and stopwatch
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Shader postproc;                        // shader to use for post processing
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing

        // Some variables
        bool useRenderTarget = true;
        const float PI = 3.1415926535f;          // PI
        float a = 0;                             // teapot rotation angle
        int currentLight = 0;
        int inputTimer = 0;

        // initialize
        public void Init()
        {
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            porcelain = new Texture("../../assets/porcelain_texture.jpg");

            c = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1));

            // load teapot
            floor = new Mesh("../../assets/floor.obj");
            floor.mTransform = Matrix4.CreateTranslation(new Vector3(0,0,0));
            floorNode = new SceneGraph(null, floor, wood);

            teapot = new Mesh("../../assets/teapot.obj");
            teapot.mTransform = Matrix4.CreateTranslation(new Vector3(0, 0, 0));
            SceneGraph teapotNode = new SceneGraph(floorNode, teapot, porcelain);

            teacup = new Mesh("../../assets/Cup.obj");
            teacup.mTransform = Matrix4.CreateTranslation(new Vector3(10, 0, -5));
            SceneGraph teacupNode = new SceneGraph(floorNode, teacup, porcelain);

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

            // set the lights
            int ambientColor = GL.GetUniformLocation(shader.programID, "ambientColor");
            lights = new List<Light>();
            lights.Add(new Light(new Vector3(15, 5, 14), new Vector3(0.1f, 0f, 0.1f), "lightPos", "lightCol", 0, shader, true));
            lights.Add(new Light(new Vector3(-3, 3, 5), new Vector3(0, 1, 1), "lightPos", "lightCol", 1, shader, true));
            lights.Add(new Light(new Vector3(0, 20, 14), new Vector3(0.4f, 0.4f, 0.4f), "lightPos", "lightCol", 2, shader, true));
            lights.Add(new Light(new Vector3(5, 0, 0), new Vector3(0, 0, 1), "lightPos", "lightCol", 3, shader, true));

            lightPoss = new Vector3[4];
            lightCols = new Vector3[4];

            for (int i = 0; i < lights.Count; i++)
            {
                lightPoss[i] = lights[i].lightPosition;
                lightCols[i] = lights[i].lightColor;
            }

            GL.UseProgram(shader.programID);
            GL.Uniform3(ambientColor, new Vector3(0.1f, 0.1f, 0.1f));
            for (int i = 0; i < lights.Count; i++)
            {
                GL.Uniform3(lights[i].lightIDp, lightPoss[i]);
                GL.Uniform3(lights[i].lightIDc, lightCols[i]);
            }
        }

        // tick for background surface
        public void Tick()
        {
            inputTimer++;
            screen.Clear(0);
            if (inputTimer >= 10)
                HandleInput();
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            Matrix4 transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            Matrix4 toWorld = transform;
            transform *= Matrix4.CreateTranslation(c.cameraPos) * Matrix4.CreateTranslation(0, -4, -15);
            transform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), c.hRotate);
            transform *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), c.vRotate);
            transform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), c.zRotate);
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;
            if (c.vRotate < 0) c.vRotate = 2 * PI;

            // render scene to render target
            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                floorNode.Render(shader, transform, toWorld);

                // render quad
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                target.Bind();

                floorNode.nodeMesh.Render(shader, transform, toWorld, wood);
                floorNode.nodeChildren[0].nodeMesh.Render(shader, transform, toWorld, wood);

                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
        }

        public void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Key.Keypad4)) { currentLight--; if (currentLight < 0) currentLight = 3; inputTimer = 0; }
            if (Keyboard.GetState().IsKeyDown(Key.Keypad6)) { currentLight++; if (currentLight > 3) currentLight = 0; inputTimer = 0; }
            if (Keyboard.GetState().IsKeyDown(Key.Keypad5))
            { lights[currentLight].UpdateLight();
                inputTimer = 0;
                UpdateSceneLighting();
            }
            c.ExecuteCommand();
        }

        public void UpdateSceneLighting()
        {
            GL.UseProgram(shader.programID);
            lightPoss[currentLight] = lights[currentLight].lightPosition;
            lightCols[currentLight] = lights[currentLight].lightColor;
            GL.Uniform3(lights[currentLight].lightIDp, lightPoss[currentLight]);
            GL.Uniform3(lights[currentLight].lightIDc, lightCols[currentLight]);
        }
    }
} // namespace Template_P3