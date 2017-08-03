using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KegBot

{ 
    
  
    public partial class Form1 : Form  
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        //const uint SWP_NOSIZE = 0x0001;
        
        const uint SWP_NOZORDER = 0x0004;

        //moveGwent finds the process and moves it
        //found here https://stackoverflow.com/questions/1364440/how-to-get-and-set-the-window-position-of-another-application-in-c-sharp
        public void moveGwent()
        {
            //Get the time and assign to variable timeNow

            DateTime timeNow = DateTime.Now;

            // Find (the first-in-Z-order) Notepad window.
            IntPtr hWnd = FindWindow("ConsoleWindowClass", null);

            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                // Move the window to (0,0) 
                // in the Z order.
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 250, 250, SWP_NOZORDER);
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + " : Window has been resized";

            }

            else
            {
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + " : Unable to find Window. Please open Gwent";
            }
        }

        public Form1()
        {
            InitializeComponent();
            
            
           

        }

        private void button1_Click(object sender, EventArgs e)
        {

            moveGwent();
            

        }

        private void consoleRichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
