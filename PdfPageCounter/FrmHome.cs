using iTextSharp.text.pdf;
using System.ComponentModel;
using System.Diagnostics;

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

                File.Create(filePath).Close();

                using (StreamWriter stramWriter = File.AppendText(filePath))
                {
                    int totalSortedFiles = 0;
                    int totalSortedPages = 0;
                    int totalUnsortedFiles = 0;
                    int totalUnsortedPages = 0;
                    stramWriter.WriteLine("-------------- Files with number --------------");
                    //Process sorted files
                    foreach (var item in sortedFilesList)
                    {
                        processCounter++;
                        using (PdfReader pdfReader = new PdfReader(item.FilePath))
                        {
                            pageCounter += pdfReader.NumberOfPages;
                            stramWriter.WriteLine(string.Format("{0, -4}", processCounter) + string.Format("{0, -60}", item.FullFileName) + "\t>\t" + pdfReader.NumberOfPages);
                        }

                        progressBar1.BeginInvoke(() =>
                        {
                            progressBar1.Value = (processCounter * 100 / fileCounter);
                        });
                    }
                    totalSortedFiles = processCounter;
                    totalSortedPages = pageCounter;

                    stramWriter.WriteLine("-------------- Unknown or Files with name --------------");
                    //Process unsorted files
                    foreach (var item in unsortedFilesList)
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

                Process.Start("notepad.exe",filePath);
            }
        }
    }
}