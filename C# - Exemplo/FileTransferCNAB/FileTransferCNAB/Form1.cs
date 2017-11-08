using FileTransferCNAB.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransferCNAB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Encrypt enc = new Encrypt();

            enc.Criptografa();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Decrypt dec = new Decrypt();
            dec.Decriptografa();
        }
    }
}
