﻿using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3 {

class Game
{
	// member variables
	public Surface screen;					// background surface for printing etc.
	Mesh mesh, floor, teacup;				// a mesh to draw using OpenGL
	const float PI = 3.1415926535f;			// PI
	float a = 0;                            // teapot rotation angle
        Camera c;
	Stopwatch timer;                        // timer for measuring frame duration
	Shader shader;							// shader to use for rendering
	Shader postproc;						// shader to use for post processing
	Texture wood;                           // texture to use for rendering
        Texture porcelain;
	RenderTarget target;					// intermediate render target
	ScreenQuad quad;						// screen filling quad for post processing
	bool useRenderTarget = true;
        float d = 0,  b = 0;

	// initialize
	public void Init()
	{
            c = new Camera(new Vector3(0, -4, -15), new Vector3(0, 0, 1));
		// load teapot
		mesh = new Mesh( "../../assets/teapot.obj" );
		floor = new Mesh( "../../assets/floor.obj" );
            teacup = new Mesh("../../assets/Cup.obj");
		// initialize stopwatch
		timer = new Stopwatch();
		timer.Reset();
		timer.Start();
		// create shaders
		shader = new Shader( "../../shaders/vs.glsl", "../../shaders/fs.glsl" );
		postproc = new Shader( "../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl" );
		// load a texture
		wood = new Texture( "../../assets/wood.jpg" );
        porcelain = new Texture("../../assets/porcelain_texture.jpg");
		// create the render target
		target = new RenderTarget( screen.width, screen.height );
		quad = new ScreenQuad();
            // set the light
            int ambientColor = GL.GetUniformLocation(shader.programID, "ambientColor");
            Light L = new Light(new Vector3(20, 0, 0), new Vector3(1, 1, 0.5f), "lightPos", "lightCol", shader, true);
            Light L2 = new Light(new Vector3(-10, 0, 0), new Vector3(10, 0, 0), "lightPos2", "lightCol2", shader, false);
            Light L3 = new Light(new Vector3(0, 0, 0), new Vector3(0, 10, 0), "lightPos3", "lightCol3", shader, false);
            Light L4 = new Light(new Vector3(0, 10, 0), new Vector3(10, 10, 10), "lightPos4", "lightCol4", shader, false);

            /*int lightID = GL.GetUniformLocation(shader.programID, "lightPos");
            int lightID2 = GL.GetUniformLocation(shader.programID, "lightPos2");
            int lightID3 = GL.GetUniformLocation(shader.programID, "lightPos3");*/
            GL.UseProgram(shader.programID);
            GL.Uniform3(ambientColor, new Vector3(0.1f, 0.1f, 0.1f));
            GL.Uniform3(L.lightIDp, L.lightPosition);
            GL.Uniform3(L.lightIDc, L.lightColor);
            GL.Uniform3(L2.lightIDp, L2.lightPosition);
            GL.Uniform3(L2.lightIDc, L2.lightColor);
            GL.Uniform3(L3.lightIDp, L3.lightPosition);
            GL.Uniform3(L3.lightIDc, L3.lightColor);
            GL.Uniform3(L4.lightIDp, L4.lightPosition);
            GL.Uniform3(L4.lightIDc, L4.lightColor);
            /*GL.Uniform3(lightID, new Vector3(20, 0, 0));
            GL.Uniform3(lightID2, new Vector3(-10, 0, 0));
            GL.Uniform3(lightID3, new Vector3(0, 0, 0));    */

        }

	// tick for background surface
	public void Tick()
	{
		screen.Clear( 0 );
		screen.Print( "hello world", 2, 2, 0xffff00 );
            Console.WriteLine( c.cameraDir);
	}

	// tick for OpenGL rendering code
	public void RenderGL()
	{
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
		timer.Reset();
		timer.Start();

            // prepare matrix for vertex shader
            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[OpenTK.Input.Key.D]) c.cameraPos.X -= 0.1f;
            if (keyboard[OpenTK.Input.Key.A]) c.cameraPos.X += 0.1f;
            if (keyboard[OpenTK.Input.Key.W]) c.cameraPos.Z += 0.1f;
            if (keyboard[OpenTK.Input.Key.S]) c.cameraPos.Z -= 0.1f;
            if (keyboard[OpenTK.Input.Key.Space]) c.cameraPos.Y -= 0.1f;
            if (keyboard[OpenTK.Input.Key.ShiftLeft]) c.cameraPos.Y += 0.1f;
            if (keyboard[OpenTK.Input.Key.Left]) b -= 0.015f;
            if (keyboard[OpenTK.Input.Key.Right]) b += 0.015f;
            if (keyboard[OpenTK.Input.Key.Up]) d -= 0.015f;
            if (keyboard[OpenTK.Input.Key.Down]) d += 0.015f;

            Vector3 durp = (c.cameraDir / (float)Math.Cos(b)).Normalized();
            Matrix4 transform = Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
            transform *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), b);
            transform *= Matrix4.CreateTranslation(c.cameraPos + durp);
            /*transform *= Matrix4.CreateRotationY(b);
            transform *= Matrix4.CreateRotationX(d);*/
            transform *= Matrix4.CreatePerspectiveFieldOfView( 1.2f, 1.3f, .1f, 1000 );
            Matrix4 toWorld = transform;



            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            if (useRenderTarget)
		{
			// enable render target
			target.Bind();

			// render scene to render target
			mesh.Render( shader, transform, toWorld, wood );
			floor.Render( shader, transform, toWorld, wood );
                //teacup.Render(shader, transform, toWorld, porcelain);

			// render quad
			target.Unbind();
			quad.Render( postproc, target.GetTextureID() );
		}
		else
		{
			// render scene directly to the screen
			mesh.Render( shader, transform, toWorld, wood );
			floor.Render( shader, transform, toWorld, wood );
                //teacup.Render(shader, transform, toWorld, porcelain);
            }
	}
}

} // namespace Template_P3