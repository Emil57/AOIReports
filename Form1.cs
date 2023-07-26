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
            //VitroxProcess();
            IcosProcess();
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
            textBox1.Text = textBox1.Text.Replace(".1", ";");

        }

        void IcosProcess()
        {
            List<string> lotsList = new List<string>();
            List<string> lotsFound = new List<string>();

            //aqui va todo el proceso de icos
            List<string> PathListIcos = new()
            {
                "\\\\MEX6ICOS01\\_results\\ascii\\global",
                "\\\\MEX6ICOS02\\_results\\ascii\\global",
                "\\\\MEX6ICOS03\\_results\\ascii\\global",
                "\\\\mex6icos04\\_results\\ascii\\global",
                "\\\\mex6icos05\\_results\\ascii\\global",
                "\\\\mex6icos06\\_results\\ascii\\global",
                "\\\\mex6icos07\\_results\\ascii\\global",
                "\\\\mex6icos08\\_Results\\ascii\\global",
                "\\\\mex6icos09\\_results\\ascii\\global",
                "\\\\mex6icos10\\_results\\ascii\\global",
                "\\\\mex6icos11\\_results\\ascii\\global",
                "\\\\mex6icos12\\_results\\ascii\\global",
                "\\\\mex6icos13\\_results\\ascii\\global",
                "\\\\mex6icos14\\_results\\ascii\\global",
                "\\\\mex6icos15\\_results\\ascii\\global",
                "\\\\mex6icos16\\_results\\ascii\\global"
            };

            List<string> VariantIcos = new()
            {
                ".1",".2",".1_R1",".1_R2",".1_R3"
            };

            string global = "_global";
            string ext = ".mhtml";

            //Pass the lots to a list
            while (textBox1.Text.Contains(";"))
            {
                string lot = textBox1.Text.Substring(0, textBox1.Text.IndexOf(";"));
                textBox1.Text = textBox1.Text.Remove(0, textBox1.Text.IndexOf(";") + 1);
                lotsList.Add(lot);
            }


            foreach (string lot in lotsList)
            {
                foreach(string path in PathListIcos)
                {
                    foreach(string variant in VariantIcos)
                    {
                        string pathicos = path + lot + variant + "\\" + global + ext;
                    }
                }
            }
        }
        

        // 1. arma los paths para cada caso path 1 
        // 2. revisa con file.exist() si existe el path, si no existe es pq no esta en esa carpeta
        // 3. cuando encuentres el archivo, muevelo a la carpeta que quieres con file.copy()

        private void VitroxProcess()
        {
            short lotsNotFound = 0;
            string server2 = "\\\\mex6vtrx02\\D\\Texas\\Report\\ICPLUS", server1 = "\\\\mex6vtrx01\\d\\Texas\\Report\\ICPLUS";
            string mainPath = "\\\\mexhome03\\Data\\MC Back End\\Generic\\Molding and Singulation\\AOI REPORTS";
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

                Process.Start(new ProcessStartInfo
                {
                    FileName = mainPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                MessageBox.Show("No lots found.", "Lots Results");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}