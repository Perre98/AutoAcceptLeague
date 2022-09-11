using System.Diagnostics;

namespace AutoAcceptLeagueWithForms
{
    public partial class settingsForm : Form
    {
        string settingsPath = Environment.CurrentDirectory + "\\settings.txt";
        public settingsForm()
        {
            this.Name = "AutoAcceptLeague Settings";
            InitializeComponent();
            linkLabel1.Click += LinkLabel1_Click;
        }

        private void LinkLabel1_Click(object? sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {linkLabel1.Text}"));
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if(int.TryParse(txtWidth.Text, out int width) && int.TryParse(txtHeight.Text, out int height))
            {
                File.Delete(settingsPath);
                File.WriteAllText(settingsPath, width.ToString() + "\n" + height.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}