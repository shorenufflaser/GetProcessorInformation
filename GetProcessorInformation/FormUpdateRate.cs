using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetProcessorInformation
{
    public partial class FormUpdateRate : Form
    {
        public FormUpdateRate()
        {
            InitializeComponent();
        }

        public int iUpdateRate
        {
            get { return Convert.ToInt32 (numericUpDown1.Value); }
            set { numericUpDown1.Value = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            iUpdateRate = Convert.ToInt32(numericUpDown1.Value);
        }
    }
}
