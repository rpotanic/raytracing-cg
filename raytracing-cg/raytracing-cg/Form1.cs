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
        public Form1()
        {
            GLGraphics = new GLGraphics();
            InitializeComponent();
        }
    }
}
