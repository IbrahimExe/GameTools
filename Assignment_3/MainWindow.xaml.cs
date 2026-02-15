using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TextureAtlasLib;


namespace Assignment_3
{
    public partial class MainWindow : Window
    {
        private SpriteSheetProject _project = new SpriteSheetProject();
        private string _currentProjectPath = string.Empty;

        // Undo/redo stacks store snapshots of ImagePaths
        private readonly Stack<List<string>> _undoStack = new Stack<List<string>>();
        private readonly Stack<List<string>> _redoStack = new Stack<List<string>>();

        public MainWindow()
        {
            InitializeComponent();
            RefreshUIFromProject();
        }

        #region File menu: New/Open/Save/SaveAs/Exit

        private void FileNew_Click(object sender, RoutedEventArgs e)
        {
            if (_project.ImagePaths.Any())
            {
                var res = MessageBox.Show("Save current project before creating a new one?", "Save", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel) return;
                if (res == MessageBoxResult.Yes)
                {
                    if (!SaveProjectToPath(_currentProjectPath))
                        return;
                }
            }
            _project = new SpriteSheetProject();
            _currentProjectPath = string.Empty;
            MenuSave.IsEnabled = false;
            RefreshUIFromProject();
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Sprite Project (*.xml)|*.xml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var proj = LoadProjectFromPath(dlg.FileName);
                    // check for missing images
                    var missing = proj.ImagePaths.Where(p => !File.Exists(p)).ToList();
                    if (missing.Count > 0)
                    {
                        string msg = "The following images were missing and have been removed:\n" + string.Join("\n", missing);
                        MessageBox.Show(msg, "Missing files", MessageBoxButton.OK, MessageBoxImage.Warning);
                        proj.ImagePaths = proj.ImagePaths.Where(p => File.Exists(p)).ToList();
                    }
                    _project = proj;
                    _currentProjectPath = dlg.FileName;
                    MenuSave.IsEnabled = true;
                    RefreshUIFromProject();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load project: " + ex.Message);
                }
            }
        }

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentProjectPath))
            {
                FileSaveAs();
                return;
            }
            if (!SaveProjectToPath(_currentProjectPath))
                MessageBox.Show("Failed to save project.");
        }

        private void FileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            FileSaveAs();
        }

        private void FileSaveAs()
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Sprite Project (*.xml)|*.xml|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                if (SaveProjectToPath(dlg.FileName))
                {
                    _currentProjectPath = dlg.FileName;
                    MenuSave.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Failed to save project.");
                }
            }
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            if (_project.ImagePaths.Any())
            {
                var res = MessageBox.Show("Save current project before exit?", "Save", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel) return;
                if (res == MessageBoxResult.Yes)
                {
                    if (!SaveProjectToPath(_currentProjectPath))
                        return;
                }
            }
            Application.Current.Shutdown();
        }

        #endregion

        #region Edit menu: Undo/Redo/Copy/Paste/Remove/RemoveAll

        private void EditUndo_Click(object sender, RoutedEventArgs e)
        {
            if (_undoStack.Any())
            {
                _redoStack.Push(CloneList(_project.ImagePaths));
                _project.ImagePaths = _undoStack.Pop();
                RefreshUIFromProject();
            }
        }

        private void EditRedo_Click(object sender, RoutedEventArgs e)
        {
            if (_redoStack.Any())
            {
                _undoStack.Push(CloneList(_project.ImagePaths));
                _project.ImagePaths = _redoStack.Pop();
                RefreshUIFromProject();
            }
        }

        private void EditCopy_Click(object sender, RoutedEventArgs e)
        {
            var selected = ImagesListBox.SelectedItem as string;
            if (selected != null)
            {
                Clipboard.SetText(selected);
            }
        }

        private void EditPaste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var text = Clipboard.GetText();
                if (File.Exists(text))
                {
                    PushUndo();
                    _project.ImagePaths.Add(text);
                    RefreshUIFromProject();
                }
            }
        }

        private void EditRemove_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelected();
        }

        private void EditRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            if (_project.ImagePaths.Any())
            {
                if (MessageBox.Show("Remove all images?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    PushUndo();
                    _project.ImagePaths.Clear();
                    RefreshUIFromProject();
                }
            }
        }

        #endregion

        #region Add / Remove buttons

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                PushUndo();
                foreach (var f in dlg.FileNames)
                {
                    if (File.Exists(f) && !_project.ImagePaths.Contains(f))
                        _project.ImagePaths.Add(f);
                }
                RefreshUIFromProject();
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            RemoveSelected();
        }

        private void RemoveSelected()
        {
            var selected = ImagesListBox.SelectedItem as string;
            if (selected == null) return;
            PushUndo();
            _project.ImagePaths.Remove(selected);
            RefreshUIFromProject();
        }

        private void BrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "PNG Files (*.png)|*.png|All files (*.*)|*.*";
                dlg.DefaultExt = ".png";
                if (dlg.ShowDialog() == true)
                {
                    string full = dlg.FileName;

                    tbOutputDir.Text = System.IO.Path.GetDirectoryName(full) ?? string.Empty;
                    tbOutputFile.Text = System.IO.Path.GetFileName(full) ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to choose output file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Serialization: Save / Load project xml

        private bool SaveProjectToPath(string path)
        {
            try
            {
                UpdateProjectFromUI();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpriteSheetProject));
                using (var fs = File.Open(path, FileMode.Create))
                {
                    serializer.Serialize(fs, _project);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save project: " + ex.Message);
                return false;
            }
        }

        private SpriteSheetProject LoadProjectFromPath(string path)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpriteSheetProject));
            using (var fs = File.OpenRead(path))
            {
                var proj = (SpriteSheetProject)serializer.Deserialize(fs);
                return proj;
            }
        }

        #endregion

        #region Generate (BackgroundWorker placeholder for calling TextureAtlasLib)

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string err))
            {
                MessageBox.Show("Cannot generate: " + err, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UpdateProjectFromUI();

            // Build final output path
            var outDir = _project.OutputDirectory;
            Directory.CreateDirectory(outDir);
            var outFileFull = System.IO.Path.Combine(outDir, _project.OutputFile);

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (s, ev) => DoGenerate(worker, outFileFull);
            worker.ProgressChanged += (s, ev) => progressBar.Value = ev.ProgressPercentage;
            worker.RunWorkerCompleted += (s, ev) =>
            {
                progressBar.Value = 0;
                if (ev.Error != null)
                {
                    MessageBox.Show("Generation failed: " + ev.Error.Message);
                    statusText.Text = "Failed";
                }
                else
                {
                    statusText.Text = "Done";
                    MessageBoxResult res = MessageBox.Show("Generated successfully. Open output folder?", "Done", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", outDir);
                    }
                }
            };
            statusText.Text = "Working...";
            worker.RunWorkerAsync();
        }

        // Calls TextureAtlasLib.Spritesheet
        private void DoGenerate(BackgroundWorker worker, string outputFileFull)
        {
            // snapshot the current project list
            var paths = CloneList(_project.ImagePaths);

            if (paths == null || paths.Count == 0)
                throw new Exception("No images provided.");

            // parse columns
            int columns = 1;
            if (!int.TryParse(tbColumns.Text, out columns) || columns <= 0) columns = 1;

            // Prepare the spritesheet object from the TextureAtlasLib
            var sheet = new Spritesheet()
            {
                Columns = columns,
                OutputDirectory = _project.OutputDirectory,
                OutputFile = _project.OutputFile,
                IncludeMetaData = _project.IncludeMetaData,
                InputPaths = paths
            };

            try
            {
                int total = Math.Max(1, paths.Count);
                for (int i = 0; i < total; i++)
                {
                    System.Threading.Thread.Sleep(10); // small tick so UI shows progress
                    int pct = (int)(((double)(i + 1) / (total * 2)) * 100.0); 
                    worker.ReportProgress(Math.Min(pct, 99));
                }


                sheet.Generate(overwrite: true);


                worker.ReportProgress(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Helpers: UI sync, validation, undo/redo support

        private void RefreshUIFromProject()
        {
            tbOutputDir.Text = _project.OutputDirectory ?? string.Empty;
            tbOutputFile.Text = _project.OutputFile ?? string.Empty;
            tbColumns.Text = "1";
            chkIncludeMeta.IsChecked = _project.IncludeMetaData;
            ImagesListBox.ItemsSource = null;
            ImagesListBox.ItemsSource = _project.ImagePaths;
            statusText.Text = string.Empty;
            progressBar.Value = 0;
        }

        private void UpdateProjectFromUI()
        {
            _project.OutputDirectory = tbOutputDir.Text?.Trim() ?? string.Empty;
            _project.OutputFile = tbOutputFile.Text?.Trim() ?? string.Empty;
            _project.IncludeMetaData = chkIncludeMeta.IsChecked == true;
        }

        private bool SaveProjectToPathAndEnable(string path)
        {
            bool ok = SaveProjectToPath(path);
            if (ok)
            {
                _currentProjectPath = path;
                MenuSave.IsEnabled = true;
            }
            return ok;
        }

        private bool ValidateInputs(out string error)
        {
            error = string.Empty;
            UpdateProjectFromUI();

            if (string.IsNullOrWhiteSpace(_project.OutputDirectory))
            {
                error = "Output directory is empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(_project.OutputFile))
            {
                error = "Output file is empty.";
                return false;
            }
            if (!_project.ImagePaths.Any())
            {
                error = "No images added.";
                return false;
            }
            if (!int.TryParse(tbColumns.Text, out int columns) || columns <= 0)
            {
                error = "Columns must be an integer > 0";
                return false;
            }
            return true;
        }

        // store snapshot for undo
        private void PushUndo()
        {
            _undoStack.Push(CloneList(_project.ImagePaths));
            _redoStack.Clear();
        }

        private List<string> CloneList(IEnumerable<string> list)
        {
            return list == null ? new List<string>() : new List<string>(list);
        }

        #endregion
    }
}
