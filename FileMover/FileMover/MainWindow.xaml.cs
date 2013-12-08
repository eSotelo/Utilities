using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FileMover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// The _datasrc
        /// </summary>
        private readonly List<string> _datasrc = new List<string>();

        /// <summary>
        /// The _source folder
        /// </summary>
        private string _sourceFolder = string.Empty;

        /// <summary>
        /// The _destination folder
        /// </summary>
        private string _destinationFolder = string.Empty;

        /// <summary>
        /// The _file extention to move
        /// </summary>
        private string _fileExtentionToMove = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether [search recursive].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [search recursive]; otherwise, <c>false</c>.
        /// </value>
        private Boolean SearchRecursive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _datasrc.Add(".mp3");
            _datasrc.Add(".mp4");
            _datasrc.Add(".m4a");
            FileTypeComboBox.ItemsSource = _datasrc;
            srcDirTxtbx.Text = _sourceFolder;
            destDirTxtbx.Text = _destinationFolder;
        }

        /// <summary>
        /// Handles the Click event of the MoveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            var consolidatedFileLocation = new DirectoryInfo(_destinationFolder);
            if (!consolidatedFileLocation.Exists)
                Directory.CreateDirectory(_destinationFolder);

            List<String> filesToMove = Directory.GetFiles(_sourceFolder, "*" + _fileExtentionToMove, SearchRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
            foreach (string file in filesToMove)
            {
                var mFile = new FileInfo(file);
                if (!(new FileInfo(consolidatedFileLocation + "\\" + mFile.Name).Exists))
                    mFile.MoveTo(consolidatedFileLocation + "\\" + mFile.Name);
            }

        }

        /// <summary>
        /// Handles the SelectionChanged event of the FileTypeComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void FileTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _fileExtentionToMove = FileTypeComboBox.SelectedValue.ToString();
        }

        /// <summary>
        /// Handles the Click event of the SourceBrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SourceBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            _sourceFolder = (Directory.Exists(_sourceFolder)) ? _sourceFolder : "";
            var dlg1 = new Ionic.Utils.FolderBrowserDialogEx
            {
                Description = "Select a folder for the extracted files:",
                ShowNewFolderButton = true,
                ShowEditBox = true,
                ShowFullPathInEditBox = false,
                RootFolder = Environment.SpecialFolder.MyComputer,
            };

            var result = dlg1.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _sourceFolder = dlg1.SelectedPath;
                srcDirTxtbx.Text = _sourceFolder;
            }
        }

        /// <summary>
        /// Handles the Click event of the DestinationBrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DestinationBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            _destinationFolder = (Directory.Exists(_destinationFolder)) ? _destinationFolder : "";
            var dlg1 = new Ionic.Utils.FolderBrowserDialogEx
            {
                Description = "Select a folder for the extracted files:",
                ShowNewFolderButton = true,
                ShowEditBox = true,
                ShowFullPathInEditBox = false,
                RootFolder = Environment.SpecialFolder.MyComputer,
            };

            var result = dlg1.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK) return;
            _destinationFolder = dlg1.SelectedPath;
            destDirTxtbx.Text = _destinationFolder;
        }

        /// <summary>
        /// Handles the Checked event of the SearchRecursiveCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SearchRecursiveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SearchRecursive = SearchRecursiveCheckBox.IsChecked ?? true;
        }

    }
}
