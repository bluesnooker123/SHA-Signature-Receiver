using CSharp_easy_RSA_PEM;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                label_fileName.Text = openFileDialog1.SafeFileName;
            }

        }

        private void button_verifySignature_Click(object sender, EventArgs e)
        {
            if (textBox_JSON.TextLength == 0)
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

                //dynamic deserializedObject = JObject.Parse(textBox_JSON.Text);
                //textBox_message.Text = deserializedObject.message;
                dynamic deserializedObject = JsonConvert.DeserializeObject<string>(textBox_JSON.Text);
                textBox_message.Text = deserializedObject;

                string importantMessage = textBox_JSON.Text;
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
