using System;
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
	Mesh mesh, floor;						// a mesh to draw using OpenGL
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
        float x = 0, y = -4, z = -15, b = 0;

	// initialize
	public void Init()
	{
            c = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
		// load teapot
		mesh = new Mesh( "../../assets/teapot.obj" );
		floor = new Mesh( "../../assets/floor.obj" );
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
            Light L = new Light(new Vector3(20, 0, 0), new Vector3(10, 10, 8), "lightPos", "lightCol", shader);

            /*int lightID = GL.GetUniformLocation(shader.programID, "lightPos");
            int lightID2 = GL.GetUniformLocation(shader.programID, "lightPos2");
            int lightID3 = GL.GetUniformLocation(shader.programID, "lightPos3");*/
            GL.UseProgram(shader.programID);
            GL.Uniform3(ambientColor, new Vector3(1, 1, 0.8f));
            GL.Uniform3(L.lightIDp, L.lightPosition);
            GL.Uniform3(L.lightIDc, L.lightColor);
            /*GL.Uniform3(lightID, new Vector3(20, 0, 0));
            GL.Uniform3(lightID2, new Vector3(-10, 0, 0));
            GL.Uniform3(lightID3, new Vector3(0, 0, 0));    */

        }

	// tick for background surface
	public void Tick()
	{
		screen.Clear( 0 );
		screen.Print( "hello world", 2, 2, 0xffff00 );
            Console.WriteLine(c.cameraPos);
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
            if (keyboard[OpenTK.Input.Key.Left]) b += 0.1f;
            if (keyboard[OpenTK.Input.Key.Right]) b -= 0.1f;

            Vector3 durp = (c.cameraDir / (float)Math.Cos(b)).Normalized();
            Matrix4 transform = Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
            transform *= Matrix4.CreateTranslation(c.cameraPos + durp);
            transform *= Matrix4.CreateRotationY(b);
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
			mesh.Render( shader, transform, toWorld, porcelain );
			floor.Render( shader, transform, toWorld, wood );

			// render quad
			target.Unbind();
			quad.Render( postproc, target.GetTextureID() );
		}
		else
		{
			// render scene directly to the screen
			mesh.Render( shader, transform, toWorld, porcelain );
			floor.Render( shader, transform, toWorld, wood );
		}
	}
}

} // namespace Template_P3