using System.Diagnostics;

namespace VitroxProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            short lotsNotFound = 0;
            string server2 = "\\\\mex6vtrx02\\D\\Texas\\Report\\ICPLUS", server1 = "\\\\mex6vtrx01\\d\\Texas\\Report\\ICPLUS";
            string mainPath = "W:\\\\MC Back End\\Generic\\Molding and Singulation\\Emilia M\\VTRX REPORTS";
            bool flag = false;
            List<string> lotsList = new List<string>();
            List<string> lotsFound = new List<string>();
            string[] fileVariants =
            {
                ".1", ".2",".1-1", ".1-2", ".1-3",
            };

            //Create new folders to store the lots files
            string folder1 = mainPath + "\\" + "Folder1_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
            string folder2 = mainPath + "\\" + "Folder2_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
            if (!Directory.Exists(folder1))
            {
                Directory.CreateDirectory(folder1);
            }
            if (!Directory.Exists(folder2))
            {
                Directory.CreateDirectory(folder2);
            }

            //Pass the lots to a list
            while (textBox1.Text.Contains(";"))
            {
                string lot = textBox1.Text.Substring(0, textBox1.Text.IndexOf(";"));
                textBox1.Text = textBox1.Text.Remove(0, textBox1.Text.IndexOf(";") + 1);
                lotsList.Add(lot);
            }

            //search for the lots inside the drives
            foreach (string lot in lotsList)
            {
                foreach (string variant in fileVariants)
                {
                    string lotPath1 = server1 + "\\" + lot + variant + ".txt";
                    string lotPath2 = server2 + "\\" + lot + variant + ".txt";
                    if (File.Exists(lotPath1))
                    {
                        string newPath = folder1 + "\\" + lot + variant + ".txt";
                        if (File.Exists(newPath))
                        {
                            File.Delete(newPath);
                        }
                        File.Copy(lotPath1, newPath);
                        if (!lotsFound.Contains(lot))
                        {
                            lotsFound.Add(lot);
                        }
                        flag = true;
                    }
                    else if (File.Exists(lotPath2))
                    {
                        string newPath = folder2 + "\\" + lot + variant + ".txt";
                        if (File.Exists(newPath))
                        {
                            File.Delete(newPath);
                        }

                        File.Copy(lotPath2, newPath);
                        if (!lotsFound.Contains(lot))
                        {
                            lotsFound.Add(lot);
                        }
                        flag = true;
                    }
                }
                if (!flag)
                {
                    Debug.WriteLine("Lote: " + " no se encontro " + lot);
                    lotsNotFound++;
                }
            }
            if (lotsFound.Count > 0)
            {
                string message = "Following lots were found: \n";
                foreach (string lot in lotsFound)
                {
                    message = message + lot + "  ";
                }
                MessageBox.Show(message, "Lots Results");
            }
            else
            {
                MessageBox.Show("No lots found.", "Lots Results");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Format the lots
            while (textBox1.Text.Contains(".1\n"))
            {
                textBox1.Text = textBox1.Text.Replace(".1\n", ";");
            }
            while (textBox1.Text.Contains(".1\r\n"))
            {
                textBox1.Text = textBox1.Text.Replace(".1\r\n", ";");
            }

        }
    }
}