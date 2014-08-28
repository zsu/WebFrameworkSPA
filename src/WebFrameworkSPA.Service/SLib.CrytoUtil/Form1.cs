using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SLib.Cryptography;

namespace SLib.CrytoUtil
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            btnDecrypt.Enabled = false;
            btnEncrypt.Enabled = false;
            tbInput.Enabled = false;
            try
            {
                TripleDesProtectedConfigurationProvider provider =
                new TripleDesProtectedConfigurationProvider();
                tbOutput.Text=provider.EncryptString(tbInput.Text);
            }
            catch (Exception oEx)
            { tbOutput.Text = "Error: " + oEx.Message; }
            finally
            {
                btnDecrypt.Enabled = true;
                btnEncrypt.Enabled = true;
                tbInput.Enabled = true;
            }

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            btnDecrypt.Enabled = false;
            btnEncrypt.Enabled = false;
            tbInput.Enabled = false;
            try
            {
                TripleDesProtectedConfigurationProvider provider =
                new TripleDesProtectedConfigurationProvider();
                tbOutput.Text=provider.DecryptString(tbInput.Text);
            }
            catch (Exception oEx)
            { tbOutput.Text = "Error: " + oEx.Message; }
            finally
            {
                btnDecrypt.Enabled = true;
                btnEncrypt.Enabled = true;
                tbInput.Enabled = true;
            }
        }

        private void tbInput_Click(object sender, EventArgs e)
        {
            tbInput.SelectAll();
        }

        private void tbOutput_Click(object sender, EventArgs e)
        {
            tbOutput.SelectAll();
        }
    }
}