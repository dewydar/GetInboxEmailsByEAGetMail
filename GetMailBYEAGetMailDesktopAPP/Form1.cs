using System;
using GetMailBYEAGetMailDesktopAPP.Common;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace GetMailBYEAGetMailDesktopAPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new GetAllMails().GetAllUnseenMails();
        }
    }
}
