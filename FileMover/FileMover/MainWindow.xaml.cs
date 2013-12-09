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
        /// <value>
        /// The _source folder.
        /// </value>
        private string SourceFolder { get; set; }

        /// <summary>
        /// The _destination folder
        /// </summary>
        /// <value>
        /// The _destination folder.
        /// </value>
        private string DestinationFolder { get; set; }

        /// <summary>
        /// The _file extention to move
        /// </summary>
        /// <value>
        /// The _file extention to move.
        /// </value>
        private string FileExtentionToMove { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [search recursive].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [search recursive]; otherwise, <c>false</c>.
        /// </value>
        private Boolean SearchRecursive { get; set; }

        /// <summary>
        /// Gets or sets the file name filter.
        /// </summary>
        /// <value>
        /// The file name filter.
        /// </value>
        private string FileNameFilter { get; set; }

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
            srcDirTxtbx.Text = SourceFolder;
            destDirTxtbx.Text = DestinationFolder;
        }

        /// <summary>
        /// Handles the Click event of the MoveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            var consolidatedFileLocation = new DirectoryInfo(DestinationFolder);
            if (!consolidatedFileLocation.Exists)
                Directory.CreateDirectory(DestinationFolder);

            var filesToMove = Directory.GetFiles(SourceFolder, (!string.IsNullOrWhiteSpace(FileNameFilter) ? "*" + FileNameFilter + "*" : "*") + FileExtentionToMove, SearchRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
            foreach (var file in filesToMove)
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
            FileExtentionToMove = FileTypeComboBox.SelectedValue.ToString();
        }

        /// <summary>
        /// Handles the Click event of the SourceBrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SourceBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SourceFolder = (Directory.Exists(SourceFolder)) ? SourceFolder : "";
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
            SourceFolder = dlg1.SelectedPath;
            srcDirTxtbx.Text = SourceFolder;
        }

        /// <summary>
        /// Handles the Click event of the DestinationBrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DestinationBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            DestinationFolder = (Directory.Exists(DestinationFolder)) ? DestinationFolder : "";
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
            DestinationFolder = dlg1.SelectedPath;
            destDirTxtbx.Text = DestinationFolder;
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

        /// <summary>
        /// Handles the KeyUp event of the FilterTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
        private void FilterTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FileNameFilter = FilterTextBox.Text;
        }

    }
}
