using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace VitroxProject
{
    public partial class Form1 : Form
    {

        string chainOfLots = "";
        const string globalPath = "\\\\mexhome03\\Data\\MC Back End\\Generic\\Molding and Singulation\\AOI REPORTS";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VitroxProcess();
            IcosProcess();

            OpenDirectory(globalPath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormatLots();
        }

        private void IcosProcess()
        {
            List<string> IcosPathsList = new()
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
            List<string> IcosVariantsList = new()
            {
                ".1",".2",".1_R1",".1_R2",".1_R3"
            };

            string lotsWithoutFormat = textBox1.Text;
            List<string> lotsFound = new List<string>();

            //Step 1
            List<string> ListOfLots = ConvertLotChainToList();
            //Step 2
            LookUpForLots(ListOfLots, IcosPathsList, IcosVariantsList, "Icos");
            //------------------------------------------------------------------------------------
        }

        private void VitroxProcess()
        {
            List<string> VitroxPathsList = new()
            {
                "\\\\mex6vtrx01\\d\\Texas\\Report\\ICPLUS",
                "\\\\mex6vtrx02\\D\\Texas\\Report\\ICPLUS"
            };
            List<string> VitroxVariantsList = new()
            {
                ".1", ".2", ".1-1", ".1-2", ".1-3",
            };

            bool flag = false;
            List<string> lotsFound = new List<string>();

            //Step 1
            List<string> ListOfLots = ConvertLotChainToList();
            //Step 2
            LookUpForLots(ListOfLots, VitroxPathsList, VitroxVariantsList, "Vitrox");
        }
        private void FormatLots()
        {
            //Format the lots from textbox
            while (textBox1.Text.Contains(".1\n"))
            {
                textBox1.Text = textBox1.Text.Replace(".1\n", ";");
            }
            while (textBox1.Text.Contains(".1\r\n"))
            {
                textBox1.Text = textBox1.Text.Replace(".1\r\n", ";");
            }
            textBox1.Text = textBox1.Text.Replace(".1", ";");
            textBox1.Text = textBox1.Text;
        }
        private List<string> ConvertLotChainToList()
        {
            List<string> ListOfLots = new List<string>();
            chainOfLots = textBox1.Text;

            //Pass the lots to a list
            while (chainOfLots.Contains(";"))
            {
                string lot = chainOfLots.Substring(0, chainOfLots.IndexOf(";"));
                chainOfLots = chainOfLots.Remove(0, chainOfLots.IndexOf(";") + 1);
                ListOfLots.Add(lot);
            }
            return ListOfLots;
        }

        private void LookUpForLots(List<string> ListOfLots, List<string> PathsList, List<string> VariantsList, string ReferenceOfData)
        {
            Dictionary<string, string> ExtensionsList = new Dictionary<string, string>()
            {
                { "Vitrox",".txt" },
                {"Icos",".mht" },

            };
            const string global = "_global";
            string lotPath = "", folderToCreate = "", newLotPath = "";

            foreach (string lot in ListOfLots)
            {
                foreach (string path in PathsList)
                {
                    foreach (string variant in VariantsList)
                    {
                        if (ReferenceOfData.Equals("Vitrox"))
                        {
                            //Vitrox changes that apply
                            lotPath = path + "\\" + lot + variant + ExtensionsList[ReferenceOfData];
                        }
                        else if (ReferenceOfData.Equals("Icos"))
                        {
                            //Icos changes that apply
                            lotPath = path + "\\" + lot + variant + global + ExtensionsList[ReferenceOfData];
                        }

                        if (File.Exists(lotPath))
                        {
                            folderToCreate = FindFolderForPath(PathsList, path, ReferenceOfData);
                            if (folderToCreate != null)
                            {
                                newLotPath = ConcatenateNewLotPath(folderToCreate, lot, ReferenceOfData, variant, global);
                                if (File.Exists(newLotPath))
                                {
                                    File.Delete(newLotPath);
                                }
                                File.Copy(lotPath, newLotPath);
                            }
                            else
                            {
                                //Stop program
                                throw new Exception();
                            }

                        }
                    }
                }
            }
        }

        private string FindFolderForPath(List<string> PathsList, string path, string ReferenceOfData)
        {
            string folderToCreate = "";
            switch (PathsList.IndexOf(path))
            {
                case 0:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "01_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 1:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "02_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 2:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "03_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 3:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "04_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 4:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "05_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 5:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "06_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 6:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "07_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 7:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "08_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 8:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "09_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 9:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "10_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 10:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "11_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 11:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "12_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 12:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "13_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 13:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "14_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 14:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "15_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
                case 15:
                    folderToCreate = globalPath + "\\" + ReferenceOfData.ToUpper() + "16_" + DateTime.Now.ToShortDateString().ToString().Replace('/', '-');
                    break;
            }
            if (!Directory.Exists(folderToCreate))
            {
                Directory.CreateDirectory(folderToCreate);
            }

            return folderToCreate;
        }

        private string ConcatenateNewLotPath(string folderToCreate, string lot, string ReferenceOfData, string variant, string global)
        {
            string lotPath = "";
            if (ReferenceOfData.Equals("Vitrox"))
            {
                lotPath = folderToCreate + "\\" + lot + variant + ".txt";
            }
            else if (ReferenceOfData.Equals("Icos"))
            {
                lotPath = folderToCreate + "\\" + lot + variant + global + ".mht";
            }
            return lotPath;
        }

        private void OpenDirectory(string path)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
        }

    }
}