using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GetID;

namespace GetIDApp
{
    public partial class Form1 : Form
    {
        private GetID.GetID generator;

        public Form1()
        {
            InitializeComponent();
            generator = new GetID.GetID();

            generator.ValueChanged += Generator_ValueChanged;
            generator.ErrorChanged += Generator_ErrorChanged;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                generator.Go();
                LogMessage("Generálás elindítva...");
            }
            catch (Exception ex)
            {
                LogMessage("Hiba az indításkor: " + ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                generator.Stop();
                LogMessage("Generálás leállítva.");
            }
            catch (Exception ex)
            {
                LogMessage("Hiba a leállításkor: " + ex.Message);
            }
        }

        private void Generator_ValueChanged(object sender, EventArgs e)
        {
            string value = generator.Value;

            // UI frissítés biztonságosan
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => ProcessValue(value)));
            }
            else
            {
                ProcessValue(value);
            }
        }

        private void Generator_ErrorChanged(object sender, EventArgs e)
        {
            string error = generator.ErrorMessage;

            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => LogMessage("Hiba: " + error)));
            }
            else
            {
                LogMessage("Hiba: " + error);
            }
        }

        private void ProcessValue(string value)
        {
            string result = $"{value} - ";

            if (Regex.IsMatch(value, @"^[A-Z][0-9]{4}$"))
            {
                result += "MEGFELELŐ";
            }
            else
            {
                result += "NEM MEGFELELŐ";
            }

            LogMessage(result);
        }

        private void LogMessage(string message)
        {
            txtOutput.AppendText(message + Environment.NewLine);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Szöveges fájl (*.txt)|*.txt",
                Title = "Mentés fájlba"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, txtOutput.Text);
                    MessageBox.Show("Mentés sikeres!", "Mentés", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a mentés során: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
