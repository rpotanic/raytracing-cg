using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.IO;

namespace raytracing_cg
{
    class GLGraphics
    {
        Vector3 cameraPosition = new Vector3(2, 1, 3);
        Vector3 cameraDirection = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        string glVersion;
        string glsVersion;
        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;
        int vaoHandle;


        public void Render()
        {
            DrawTriangle();
        }
        void loadShader(String filename, ShaderType type, int program, out int addres)
        {

            glVersion = GL.GetString(StringName.Version);
            glsVersion = GL.GetString(StringName.ShadingLanguageVersion);
            addres = GL.CreateShader(type);
            if (addres == 0)
            {
                throw new Exception("Error, can't create shader");
            }
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(addres, sr.ReadToEnd());
                Console.WriteLine(GL.GetShaderInfoLog(addres));
            }
            GL.CompileShader(addres);
            GL.AttachShader(program, addres);
            Console.WriteLine(GL.GetShaderInfoLog(addres));
        }

        public void Resize(int width, int height)
        {
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)width / height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirection, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            Render();
        }

        public void DrawTriangle()
        {

            InitShaders();

            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        }

        private void InitShaders()
        {
            //Создание программы
            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\basic.vert.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("..\\..\\basic.frag.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);

            //Линковка программы
            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };
            //  float[] colorData = { 255.0f, 0.0f, 0.0f, 233.0f, 150.0f, 122.0f, 250.0f, 150.0f, 122.0f }
            int[] vboHandlers = new int[2];
            GL.GenBuffers(2, vboHandlers);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * positionData.Length),
                           positionData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                         (IntPtr)(sizeof(float) * colorData.Length),
                          colorData, BufferUsageHint.StaticDraw);
            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void closeProgram()
        {
            GL.UseProgram(0);
        }
    }
}
