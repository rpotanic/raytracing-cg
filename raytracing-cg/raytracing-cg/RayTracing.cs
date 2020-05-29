using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.IO;

namespace raytracing_cg
{
    class RayTracing
    {


        public float latitude = 47.98f;
        public float longitude = 60.41f;
        public float radius = 5.385f;



        int VertexArrayID;
        Vector3 cameraPosition = new Vector3(0, 0, 0.8f);
        Vector3 cameraDirection = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 1, 0);
        float[] vertdata = { -1f, -1f, 0.0f, -1f, 1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0f };
        
        string glVersion;
        string glsVersion;
        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;
        int vaoHandle;
        int vertexbuffer;
        int width, height;


        public void Resize(int _width, int _height)
        {
            width = _width;
            height = _height;
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)width / height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
           

            InitShaders();
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

            GL.UseProgram(BasicProgramID);
            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, "campos"), cameraPosition);
            GL.Uniform1(GL.GetUniformLocation(BasicProgramID, "aspect"), width / (float)height);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GL.UseProgram(0);

        }

        public void Render()
        {
            cameraPosition += new Vector3(0, 0, 0.2f);
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

        private void InitShaders()
        {
            //Создание программы
            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\..\\raytracing.vert.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("..\\..\\..\\raytracing.frag.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);

            //Линковка программы
            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));



            GL.GenBuffers(1, out vertexbuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * vertdata.Length),
                           vertdata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void closeProgram()
        {
            GL.UseProgram(0);
        }
    }
}
