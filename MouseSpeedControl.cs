/**
* Using for TA:2015
* Modified from: 
*    1: [https://msdn.microsoft.com/ja-jp/library/system.windows.forms.systeminformation.mousespeed(v=vs.110).aspx]
*    2: [http://www.sparrowtail.com/changing-mouse-pointer-speed-c]
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace MouseSpeedControl
{

    public partial class Form1 : Form
    {
        public const UInt32 SPI_SETMOUSESPEED = 0x0071;

        [DllImport("User32.dll")]
        static extern Boolean SystemParametersInfo(
            UInt32 uiAction,
            UInt32 uiParam,
            UInt32 pvParam,
            UInt32 fWinIni);

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;

        
        public Form1()
        {
            InitializeComponent();
            this.SuspendLayout();
            InitForm();

            // Add each property of the SystemInformation class to the list box.
            Type t = typeof(System.Windows.Forms.SystemInformation);
            PropertyInfo[] pi = t.GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                listBox1.Items.Add(pi[i].Name);
                if (pi[i].Name.Equals("MouseSpeed")) {
                    Console.WriteLine("TEST");
                    //pi[i].SetValue(null, 20, null);
                }
            }
            textBox1.Text = "The SystemInformation class has " + pi.Length.ToString() + " properties.\r\n";

            // Configure the list item selected handler for the list box to invoke a 
            // method that displays the value of each property.
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            this.ResumeLayout(false);
            
            // Set mouse speed to the max
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, uint.Parse("20"), 0);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Return if no list item is selected.
            if (listBox1.SelectedIndex == -1) return;
            // Get the property name from the list item.
            string propname = listBox1.Text;

            if (propname == "PowerStatus")
            {
                // Cycle and display the values of each property of the PowerStatus property.
                textBox1.Text += "\r\nThe value of the PowerStatus property is:";
                Type t = typeof(System.Windows.Forms.PowerStatus);
                PropertyInfo[] pi = t.GetProperties();
                for (int i = 0; i < pi.Length; i++)
                {
                    object propval = pi[i].GetValue(SystemInformation.PowerStatus, null);
                    textBox1.Text += "\r\n    PowerStatus." + pi[i].Name + " is: " + propval.ToString();
                }
            }
            else
            {
                // Display the value of the selected property of the SystemInformation type.
                Type t = typeof(System.Windows.Forms.SystemInformation);
                PropertyInfo[] pi = t.GetProperties();
                PropertyInfo prop = null;
                for (int i = 0; i < pi.Length; i++)
                    if (pi[i].Name == propname)
                    {
                        prop = pi[i];
                        break;
                    }
                object propval = prop.GetValue(null, null);
                textBox1.Text += "\r\nThe value of the " + propname + " property is: " + propval.ToString();
            }
        }

        private void InitForm()
        {
            // Initialize the form settings
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Location = new System.Drawing.Point(8, 16);
            this.listBox1.Size = new System.Drawing.Size(172, 496);
            this.listBox1.TabIndex = 0;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(188, 16);
            this.textBox1.Multiline = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(420, 496);
            this.textBox1.TabIndex = 1;
            this.ClientSize = new System.Drawing.Size(616, 525);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox1);
            this.Text = "Select a SystemInformation property to get the value of";
        }

        uint DEFAULT_MOUSE_SPEED = 18;
        uint APPLICATION_MOUSE_SPEED = 5;
        private void Form1_Load(object sender, EventArgs e)
        {
            // Add each property of the SystemInformation class to the list box.
            Type t = typeof(System.Windows.Forms.SystemInformation);
            PropertyInfo[] pi = t.GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                if (pi[i].Name.Equals("MouseSpeed"))
                {
                    Console.WriteLine("TEST");
                    object propval = pi[i].GetValue(null, null);
                    DEFAULT_MOUSE_SPEED = uint.Parse(propval.ToString());
                    break;
                }
            }
        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {
            // Set mouse speed to the Application mouse speed
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, this.APPLICATION_MOUSE_SPEED, 0);
            Console.WriteLine("MouseHover");
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            // Set it back to the mouse default speed.
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, this.DEFAULT_MOUSE_SPEED, 0);
            Console.WriteLine("MouseLeave");
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseUp");
        }

        private void Form1_MouseCaptureChanged(object sender, EventArgs e)
        {
            Console.WriteLine("MouseCaptureChanged");
        }

        /*[STAThread]
        static void Main()
        {
            Application.Run(new SystemInfoBrowserForm());
        }*/
    }
}
