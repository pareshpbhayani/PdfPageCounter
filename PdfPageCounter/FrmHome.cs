using iTextSharp.text.pdf;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace PdfPageCounter
{
    public partial class FrmHome : Form
    {
        //private delegate void ProgressHandler(string);

        class UnsortedFilesInfo
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }

        class SortedFilesInfo
        {
            public int FileName { get; set; }
            public string FullFileName { get; set; }
            public string FilePath { get; set; }
        }

        public FrmHome()
        {
            InitializeComponent();
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dailog = new FolderBrowserDialog())
            {
                dailog.ShowDialog();
                txtFolderPath.Text = dailog.SelectedPath;
                lblProcessStatus.Visible = false;
                txtTotalCount.Text = "Wait...";
                progressBar1.Value = 0;
                lblProcessStatus.Text = "Process completed!";
                lblProcessStatus.ForeColor = Color.Green;
            }
            Thread pageCounterThread = new Thread(() => CountFileAndPages(txtFolderPath.Text));
            pageCounterThread.Start();
        }

        private void CountFileAndPages(string folderPath)
        {
            if (!string.IsNullOrEmpty(folderPath))
            {
                int pageCounter = 0;
                var files = Directory.GetFiles(folderPath, "*.pdf", SearchOption.AllDirectories);
                int fileCounter = files.Length;
                int processCounter = 0;

                List<int> sortedFiles = new();
                //List<string> unsortedFiles = new();

                //ScanFilesInfo scanFilesInfo = n

                List<SortedFilesInfo> sortedFilesList = new();
                List<UnsortedFilesInfo> unsortedFilesList = new();

                foreach (var item in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(item);
                    var splitedFileNameArray = fileName.Split(new char[] { '-', '_', '(' });
                    string splitedFileName;
                    if (splitedFileNameArray.Length > 1)
                    {
                        splitedFileName = splitedFileNameArray[0];
                    }
                    else
                    {
                        splitedFileName = fileName;
                    }

                    if (int.TryParse(splitedFileName, out int intFileName))
                    {
                        sortedFilesList.Add(new SortedFilesInfo { FileName = intFileName, FullFileName = fileName, FilePath = item });
                        //sortedFiles.Add(intFileName);
                    }
                    else
                    {
                        unsortedFilesList.Add(new UnsortedFilesInfo { FileName = fileName, FilePath = item });
                        //unsortedFiles.Add(splitedFileName);
                    }
                }
                //sortedFiles.Sort();

                sortedFilesList = sortedFilesList.OrderBy(o => o.FileName).ToList();

                string filePath = "Scan Report.txt";
                string filePath2 = "Missing Report.txt";

                StringBuilder missingReportBuilder = new StringBuilder();

                File.Create(filePath).Close();

                using (StreamWriter stramWriter = File.AppendText(filePath))
                {
                    int totalSortedFiles = 0;
                    int totalSortedPages = 0;
                    int totalUnsortedFiles = 0;
                    int totalUnsortedPages = 0;
                    stramWriter.WriteLine("-------------- Files with number --------------");
                    //Process sorted files
                    int previousFileNumber = 0;
                    int totalMissingCount = 0;
                    foreach (var item in sortedFilesList)
                    {
                        processCounter++;
                        try
                        {
                            using (PdfReader pdfReader = new PdfReader(item.FilePath))
                            {
                                pageCounter += pdfReader.NumberOfPages;
                                stramWriter.WriteLine(string.Format("{0, -4}", processCounter) + string.Format("{0, -60}", item.FullFileName) + "\t>\t" + pdfReader.NumberOfPages);
                            }

                            if (chkCountMissing.Checked)
                            {
                                int missingCount = item.FileName - previousFileNumber;
                                if (missingCount > 1)
                                {
                                    totalMissingCount += (missingCount - 1);
                                    missingReportBuilder.AppendLine(string.Format("{0, -20}", previousFileNumber + 1) + " to " + string.Format("{0, -20}", item.FileName - 1) + " = " + string.Format("{0, -5}", missingCount - 1));
                                }

                                previousFileNumber = item.FileName;
                            }

                            progressBar1.BeginInvoke(() =>
                            {
                                progressBar1.Value = (processCounter * 100 / fileCounter);
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString() + Environment.NewLine + item.FilePath, "PDF Page Counter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lblProcessStatus.BeginInvoke(() =>
                            {
                                lblProcessStatus.ForeColor = Color.Red;
                                lblProcessStatus.Text = "Error! Operation breaked.";
                            });
                            break;
                        }

                    }
                    if (chkCountMissing.Checked)
                    {
                        File.Create(filePath2).Close();
                        using (StreamWriter stramWriter2 = File.AppendText(filePath2))
                        {
                            stramWriter2.WriteLine(missingReportBuilder.ToString());
                            stramWriter2.WriteLine("----------------------------");
                            stramWriter2.WriteLine($"Total Missing = {totalMissingCount}");
                            stramWriter2.Close();
                        }
                    }

                    totalSortedFiles = processCounter;
                    totalSortedPages = pageCounter;

                    stramWriter.WriteLine("-------------- Unknown or Files with name --------------");
                    //Process unsorted files

                    foreach (var item in unsortedFilesList)
                    {
                        try
                        {
                            processCounter++;
                            using (PdfReader pdfReader = new PdfReader(item.FilePath))
                            {
                                pageCounter += pdfReader.NumberOfPages;
                                stramWriter.WriteLine(string.Format("{0, -4}", processCounter) + string.Format("{0, -60}", item.FileName) + "\t>\t" + pdfReader.NumberOfPages);
                            }

                            progressBar1.BeginInvoke(() =>
                            {
                                progressBar1.Value = (processCounter * 100 / fileCounter);
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString() + Environment.NewLine + item.FilePath, "PDF Page Counter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lblProcessStatus.BeginInvoke(() =>
                            {
                                lblProcessStatus.ForeColor = Color.Red;
                                lblProcessStatus.Text = "Error! Operation breaked.";
                            });
                            break;
                        }
                    }


                    totalUnsortedFiles = processCounter - totalSortedFiles;
                    totalUnsortedPages = pageCounter - totalSortedPages;
                    stramWriter.WriteLine("-------------- -------------- --------------");
                    stramWriter.WriteLine("Total Number Files: " + totalSortedFiles + " | Total Number Pages: " + totalSortedPages);
                    stramWriter.WriteLine("Total Unknown : " + totalUnsortedFiles + " | Total Unknown Pages: " + totalUnsortedPages);
                    stramWriter.WriteLine("Total Files: " + fileCounter + " | Total Pages: " + pageCounter);
                    stramWriter.Close();
                }

                txtTotalCount.BeginInvoke(() =>
                {
                    txtTotalCount.Text = "Files: " + fileCounter + " | Pages: " + pageCounter;
                });

                lblProcessStatus.BeginInvoke(() =>
                {
                    lblProcessStatus.Visible = true;
                });

                Process.Start("notepad.exe", filePath);
                if (chkCountMissing.Checked)
                {
                    Process.Start("notepad.exe", filePath2);
                }
            }
        }
    }
}