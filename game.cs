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
	float a = 0;							// teapot rotation angle
	Stopwatch timer;						// timer for measuring frame duration
	Shader shader;							// shader to use for rendering
	Shader postproc;						// shader to use for post processing
	Texture wood;							// texture to use for rendering
	RenderTarget target;					// intermediate render target
	ScreenQuad quad;						// screen filling quad for post processing
	bool useRenderTarget = true;
        float x = 0, y = -4, z = -15, b = 0;

	// initialize
	public void Init()
	{
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
		// create the render target
		target = new RenderTarget( screen.width, screen.height );
		quad = new ScreenQuad();
        // set the light 
        int lightID = GL.GetUniformLocation(shader.programID, "lightPos");
            int lightID2 = GL.GetUniformLocation(shader.programID, "lightPos2");
            GL.UseProgram(shader.programID);
            GL.Uniform3(lightID, 10.0f, 10.0f, 10.0f);
            GL.Uniform3(lightID2, 10.0f, 10.0f, 10.0f);

        Vector4 ambientColor =  new Vector4(200,200,200,1);
        }

	// tick for background surface
	public void Tick()
	{
		screen.Clear( 0 );
		screen.Print( "hello world", 2, 2, 0xffff00 );
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
            if (keyboard[OpenTK.Input.Key.D]) x -= 0.1f;
            if (keyboard[OpenTK.Input.Key.A]) x += 0.1f;
            if (keyboard[OpenTK.Input.Key.W]) z += 0.1f;
            if (keyboard[OpenTK.Input.Key.S]) z -= 0.1f;
            if (keyboard[OpenTK.Input.Key.Space]) y -= 0.1f;
            if (keyboard[OpenTK.Input.Key.ShiftLeft]) y += 0.1f;
            if (keyboard[OpenTK.Input.Key.Left]) b -= 0.015f;
            if (keyboard[OpenTK.Input.Key.Right]) b += 0.015f;


            Matrix4 transform = Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );

            transform *= Matrix4.CreateTranslation( x, y, z );
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
			mesh.Render( shader, transform, toWorld, wood );
			floor.Render( shader, transform, toWorld, wood );

			// render quad
			target.Unbind();
			quad.Render( postproc, target.GetTextureID() );
		}
		else
		{
			// render scene directly to the screen
			mesh.Render( shader, transform, toWorld, wood );
			floor.Render( shader, transform, toWorld, wood );
		}
	}
}

} // namespace Template_P3