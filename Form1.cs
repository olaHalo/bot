using System;
using System.IO;
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
        //Public Variables used by all methods

        //Create a variable to hold the folder named "files\images" in the current directory
        string imagesFolder = AppDomain.CurrentDomain.BaseDirectory + "files\\images\\";

        //Create a variable to hold the folder named "files\logs" in the current directory
        string logsFolder = AppDomain.CurrentDomain.BaseDirectory + "files\\logs\\";
        string logsFile = AppDomain.CurrentDomain.BaseDirectory + "files\\logs\\logs.txt";
        //File.AppendAllLines(logsFile, new[] { timeNow + " : " });

        bool isGwentOpen;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        
        //Get the time and assign to variable timeNow

        DateTime timeNow = DateTime.Now;

        // Find (the first-in-Z-order) Notepad window.

        //Define variables
        IntPtr hWnd = FindWindow("ConsoleWindowClass", null);
        const int swRestore = 9;
        const uint SWP_NOZORDER = 0x0004;
        //const uint SWP_NOSIZE = 0x0001;

        public Form1()
        {
            InitializeComponent();
            //Main Method
            //Check if the directories exist, if they don't then create them

            consoleRichTextBox1.Text += Environment.NewLine + timeNow + ": Checking directories ";
            File.AppendAllLines(logsFile, new[] { timeNow + " : Checking directories" });

            if (Directory.Exists(imagesFolder))
            {
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + ": Images directory already exists here " + imagesFolder;
                File.AppendAllLines(logsFile, new[] { timeNow + " : Images directory already exists" });
            }

            else
            {
                Directory.CreateDirectory(imagesFolder);
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + ": Creating images folder " + imagesFolder;
                File.AppendAllLines(logsFile, new[] { timeNow + " : Creating images folder" });
            }

            if (Directory.Exists(logsFolder) && (File.Exists(logsFile)))
            {
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + ": Logs directory already exists here " + imagesFolder;
                File.AppendAllLines(logsFile, new[] { timeNow + " : Logs directory already exists" });
            }

            else
            {
                //Create the directory. You do not need to check if it already exists. It will NOT override if it already exists
                System.IO.Directory.CreateDirectory(logsFolder);
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + ": Creating logs folder " + logsFolder;
                File.AppendAllLines(logsFile, new[] { timeNow + " : Creating logs folder" });
            }

        }

       

        //moveGwent finds the process and moves it
        //found here https://stackoverflow.com/questions/1364440/how-to-get-and-set-the-window-position-of-another-application-in-c-sharp
        // and here http://blog.billsdon.com/2011/10/c-sharp-check-if-application-is-already-running-then-set-focus/
        public void moveGwent()
        {
            // If found, check if its minimized, bring it to front, move to 0, then resize
            if (hWnd != IntPtr.Zero)
            {
                isGwentOpen = true;

                //check if its minimized
                if (IsIconic(hWnd))
                {
                    ShowWindowAsync(hWnd, swRestore);
                }
                // Move to foreground
                
                SetForegroundWindow(hWnd);

                // Resize and move to corner
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 250, 250, SWP_NOZORDER);

                //Output to console for debugging
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + " : Window has been resized";
                File.AppendAllLines(logsFile, new[] { timeNow + " : Window has been resized" });



            }

            else
            {
                isGwentOpen = false;
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + " : Unable to find Window. Please open Gwent";
                File.AppendAllLines(logsFile, new[] { timeNow + " : Unable to find Window. Please open Gwent" });
            }
        }
        
        //takeScreenshot creates a directory named files\images, then takes a screen shot and stores the file in it
        public void takeScreenshot()
        {
            if (isGwentOpen == true)
                {
                //Take a screenshot and store it in /files/images/
                Bitmap screenshotBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics graphic = Graphics.FromImage(screenshotBitmap);
                graphic.CopyFromScreen(0, 0, 0, 0, screenshotBitmap.Size);
                screenshotBitmap.Save(imagesFolder + "./screenshotBitmap.bmp");
                consoleRichTextBox1.Text += Environment.NewLine + timeNow + " : Screenshot taken and saved";
                File.AppendAllLines(logsFile, new[] { timeNow + " : Screenshot taken and saved" });
            }
        }

       
       

        private void button1_Click(object sender, EventArgs e)
        {

            moveGwent();
            takeScreenshot();
            

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
