using CSharp_easy_RSA_PEM;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Receiver
{
    public partial class Form1 : Form
    {
        string FilePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog1.FileName;
            }

        }

        private void button_verifySignature_Click(object sender, EventArgs e)
        {
            if (textBox_message.TextLength == 0)
            {
                MessageBox.Show("Please type message!");
                return;
            }
            if (textBox_signature.TextLength == 0)
            {
                MessageBox.Show("Please type signature!");
                return;
            }
            if (FilePath == "")
            {
                MessageBox.Show("Please choose public key file!");
                return;
            }
            try
            {
                string loadedX509 = File.ReadAllText(FilePath);
                RSACryptoServiceProvider publicX509key = Crypto.DecodeX509PublicKey(loadedX509);

                SHA1Managed sha1 = new SHA1Managed();
                string importantMessage = textBox_message.Text;
                byte[] importantMessageBytes = Encoding.UTF8.GetBytes(importantMessage);
                byte[] hashedMessage = sha1.ComputeHash(importantMessageBytes);

                byte[] signatureBytes = Convert.FromBase64String(textBox_signature.Text);

                bool isSignatureOkay = publicX509key.VerifyHash(hashedMessage, CryptoConfig.MapNameToOID("SHA1"), signatureBytes);

                if (isSignatureOkay)
                {
                    MessageBox.Show("Signature is okay!");
                }
                else
                {
                    MessageBox.Show("Signature is wrong!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

        }
    }
}
