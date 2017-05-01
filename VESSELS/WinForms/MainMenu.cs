using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VESSELS.WinForms
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void quit_btn_Click(object sender, EventArgs e)
        {
            Globals.APP_RUNNING = false;
            this.Close();
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
