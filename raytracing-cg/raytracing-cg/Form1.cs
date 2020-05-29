using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace raytracing_cg
{
    public partial class Form1 : Form
    {
        GLGraphics GLGraphics;
        RayTracing RT;

        public Form1()
        {
            GLGraphics = new GLGraphics();
            RT = new RayTracing();
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            RT.Resize(glControl1.Width, glControl1.Height);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {

            RT.Update();
            glControl1.SwapBuffers();
            RT.closeProgram();
        }

        private void Application_Idle(object sender, PaintEventArgs e)
        {

            glControl1_Paint(sender, e);

        }
    }
}
