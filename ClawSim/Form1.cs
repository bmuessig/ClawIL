using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClawIL;
using ClawIL.Standard;
using ClawIL.Minimal;
using System.IO;

namespace ClawSim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private byte[] Compile(string File)
        {
            var preproc = new Clawsemble.Preprocessor();
            var comp = new Clawsemble.Compiler();
            Clawsemble.Binary binary;

            preproc.DoFile(File);
            comp.Precompile(preproc.Tokens, preproc.Files);
            binary = comp.Compile();
            return binary.Bake();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MemoryStream flashImage = new MemoryStream(Compile("Sample.csm"));
            MinimalHardware hw = new MinimalHardware(flashImage);
            MemoryPool pool = new MemoryPool();
            Processor16 cpu = new Processor16();

            ClawInstance instance = new ClawInstance(hw, pool, cpu);
            if (!instance.Load(String.Empty))
                MessageBox.Show("Loading failed!", "Failure!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
