//  Change Data: 16.09.01

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TVSender.Properties;

namespace TVSender
{
    public partial class FormNewChoice : Form
    {
        PathConfig pathconfig;        
        Label[] LabelArray = new Label[0];
        string[] LastSelect;

        public FormNewChoice(string pathRoot = null, string pathDisplay = null)
        {
            InitializeComponent();
            this.Text = PublicData.GetVersionProject();

            UpdateFromPath(pathRoot, pathDisplay);
        }

        public void UpdateFromPath(string pathRoot, string pathDisplay = null)
        {
            pathconfig = new PathConfig(pathRoot);
            PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.testsFolder);
            PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.imageFolder);

            if (pathDisplay != null)
            {
                pathconfig.EnterFullPath(pathDisplay);
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //SetListView1View();
            LoadSettings();
            GetItemsListView(pathconfig.ItemToSelect);
            AddLabelsDraw();
            PopulateTreeView();            
        }

        private void SetListView1View()
        {
            listView1.View = View.Details;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            ColumnHeader h = new ColumnHeader();
            h.Width = listView1.ClientSize.Width;
            listView1.Columns.Add(h);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Tab | Keys.Shift) )
            {
                if (splitContainer1.Panel2Collapsed == true) splitContainer1.Panel2Collapsed = false;
                else splitContainer1.Panel2Collapsed = true;
                return true;
            }
            if (keyData == Keys.F2)
            {
                ���������ToolStripMenuItem.PerformClick();
                return true;
            }
            if (keyData == Keys.F5)
            {
                ���������F5ToolStripMenuItem.PerformClick();
                return true;
            }
            if (keyData == Keys.F10)
            {
                �������ToolStripMenuItem.PerformClick();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            //
            //Uses in TreeView
            //
            if (treeView.Focused) return base.ProcessCmdKey(ref msg, keyData);
            //
            //NOT Uses TreeView
            //
            if (keyData == Keys.Back || keyData == Keys.Left)
            {
                BackTo(pathconfig.Index);
                return true;
            }
            if (keyData == Keys.Right)
            {
                EnterListView("RightArrow");
                return true;
            }
            if (keyData == Keys.Enter)
            {
                EnterListView();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
            //keyData = (Keys.Back | Keys.Control);                
        }

        private void PopulateTreeView()
        {
            #region example
            //
            // My Computer
            //
            //TreeNode MyComputer = new TreeNode("My Computer", 4, 4);
            //MyComputer.Name = "My Computer";
            //this.treeView.Nodes.Add(MyComputer);
            //TreeNode RootNode;
            //DriveInfo[] AllDrivers = DriveInfo.GetDrives();
            //foreach (DriveInfo d in AllDrivers)
            //{
            //    DirectoryInfo DirInfo = new DirectoryInfo(d.Name);
            //    RootNode = new TreeNode(DirInfo.Name);
            //    switch (d.DriveType)
            //    {
            //        case DriveType.Fixed:
            //            RootNode.ImageIndex = 6;
            //            RootNode.SelectedImageIndex = 6;
            //            break;
            //        case DriveType.CDRom:
            //            RootNode.ImageIndex = 5;
            //            RootNode.SelectedImageIndex = 5;
            //            break;
            //        default:
            //            RootNode.ImageIndex = 0;
            //            RootNode.SelectedImageIndex = 1;
            //            break;
            //    }
            //    RootNode.ContextMenuStrip = this.contextMenuStrip;
            //    MyComputer.Nodes.Add(RootNode);
            //    if (DirInfo.Exists)
            //    {
            //        // Create FileSystemWatcher
            //        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            //        fileSystemWatcher.IncludeSubdirectories = true;
            //        fileSystemWatcher.Path = d.RootDirectory.Name;
            //        fileSystemWatcher.EnableRaisingEvents = true;
            //        fileSystemWatcher.SynchronizingObject = this;
            //        fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.OnCreated);
            //        fileSystemWatcher.Deleted += new System.IO.FileSystemEventHandler(this.OnDeleted);
            //        fileSystemWatcher.Renamed += new System.IO.RenamedEventHandler(this.OnRenamed);
            //        //
            //        RootNode.Tag = DirInfo;
            //        GetDirectories(DirInfo.GetDirectories(), RootNode);
            //    }
            //}
            //
            // My Documents
            //
            

            //var nods = pathconfig.GetFolders();

            //var nods = new List<string>();
            //DirectoryInfo[] folders = new DirectoryInfo(pathconfig.StartPath).GetDirectories();
            //foreach (var tab in folders)
            //{
            //    nods.Add(tab.Name);
            //}
#endregion
            string FullPath = pathconfig.RootPath;
            var nods = new DirectoryInfo(FullPath).GetDirectories();

            foreach (var tab in nods)
            {
                TreeNode MyDoc = new TreeNode(tab.Name);
                MyDoc.ContextMenuStrip = this.contextMenuStrip;
                string nodPath = Path.Combine(FullPath, tab.Name);
                DirectoryInfo diMyDoc = new DirectoryInfo(nodPath);
                MyDoc.Tag = diMyDoc;
                this.treeView.Nodes.Add(MyDoc);
                GetDirectories(diMyDoc.GetDirectories(), MyDoc);
            }
            //
            // test
            //
            //TreeNode test = new TreeNode("test");
            //test.ContextMenuStrip = this.contextMenuStrip;
            //DirectoryInfo test1 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) );
            //test.Tag = test1;
            //this.treeView.Nodes.Add(test);
            //GetDirectories(test1.GetDirectories(), test);            
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name);
                if (subDir.Attributes == (FileAttributes.Hidden|FileAttributes.Directory) )
                {
                    aNode.ImageIndex = 2;
                    aNode.SelectedImageIndex = 3;
                }
                else
                {
                    aNode.ImageIndex = 0;
                    aNode.SelectedImageIndex = 1;
                }
                aNode.Tag = subDir;
                aNode.ContextMenuStrip = this.contextMenuStrip;
                try
                {
                    subSubDirs = subDir.GetDirectories();
                }
                catch (Exception) { continue; }
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private void NewFolder(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != null)
            {
                if (this.treeView.SelectedNode.Level != 0 && this.treeView.SelectedNode.Name != "My Computer")
                {
                    String path = ( (DirectoryInfo)this.treeView.SelectedNode.Tag).FullName;
                    try
                    {
                        if (Directory.Exists(path) )
                        {
                            return;
                        }
                        DirectoryInfo dirInfo = Directory.CreateDirectory(path);
                        dirInfo.Create();
                        TreeNode RootNode = new TreeNode(dirInfo.Name);
                        RootNode.Tag = dirInfo;
                        RootNode.BeginEdit();
                        this.treeView.SelectedNode.Nodes.Add(RootNode);
                    }
                    catch { }
	                finally { }
                }
            }
        }

        private void DeleteFolder(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != null)
            {
                if (this.treeView.SelectedNode.Level != 0 && this.treeView.SelectedNode.Name != "My Computer" && this.treeView.SelectedNode.Parent.Name != "My Computer")
                {
                    String path = ( (DirectoryInfo)this.treeView.SelectedNode.Tag).FullName;
                    try
                    {
                        if (MessageBox.Show("Delete this directory?", "Explorer", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(path);
                            dirInfo.Delete(true);
                            this.treeView.SelectedNode.Remove();
                        }
                    }
                    catch { }
	                finally { }
                }
            }
        }

        private void Navigate(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Tag != null)
            {
                string path = ( (DirectoryInfo)e.Node.Tag).FullName;                
            }
        }

        private void AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level != 0)
            {
                if (e.Node.Parent.Text != "My Computer")
                {
                    e.Node.ImageIndex = 1;
                }
            }
        }

        private void AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level != 0)
            {
                if (e.Node.Parent.Text != "My Computer")
                {
                    e.Node.ImageIndex = 0;
                }
            }
        }
        
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(e.FullPath);
            if (fileInfo.Attributes == FileAttributes.Directory)
            {
                String path = e.OldFullPath;
                RenameNode(this.treeView.Nodes, path, e.Name, fileInfo);
                path = path.Replace(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Documents");
                RenameNode(this.treeView.Nodes, path, e.Name, fileInfo);
            }
        }

        private void RenameNode(TreeNodeCollection nodeCollection, String oldFullPath, String name, FileInfo fileInfo)
        {
            String path;
            foreach (TreeNode subNode in nodeCollection)
            {
                path = ( (DirectoryInfo)subNode.Tag).FullName;
                if (path == oldFullPath)
                {
                    subNode.Name = name.Substring(name.LastIndexOf(treeView.PathSeparator) + 1);
                    subNode.Text = name.Substring(name.LastIndexOf(treeView.PathSeparator) + 1);
                    subNode.Tag = fileInfo;
                }
                if (subNode.Nodes.Count != 0)
                {
                    RenameNode(subNode.Nodes, oldFullPath, name, fileInfo);
                }
            }
        }
        
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(e.FullPath);
            if (fileInfo.Attributes == FileAttributes.Directory)
            {
                String path = e.FullPath;
                CreateNode(this.treeView.Nodes, path.Substring(0, path.LastIndexOf(treeView.PathSeparator) ), e.Name, fileInfo);
                path = path.Replace(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Documents");
                CreateNode(this.treeView.Nodes, path.Substring(0, path.LastIndexOf(treeView.PathSeparator) ), e.Name, fileInfo);
            }
        }

        private void CreateNode(TreeNodeCollection nodeCollection, String fullPath, String name, FileInfo fileInfo)
        {
            String path;
            foreach (TreeNode subNode in nodeCollection)
            {
                path = ( (DirectoryInfo)this.treeView.SelectedNode.Tag).FullName;
                if (path == fullPath)
                {
                    TreeNode RootNode = new TreeNode(name.Substring(name.LastIndexOf(treeView.PathSeparator) + 1) );
                    switch (fileInfo.Attributes)
                    {
                        case FileAttributes.Hidden:
                            RootNode.ImageIndex = 2;
                            RootNode.SelectedImageIndex = 3;
                            break;
                        default:
                            RootNode.ImageIndex = 0;
                            RootNode.SelectedImageIndex = 1;
                            break;
                    }
                    RootNode.Tag = fileInfo;
                    RootNode.ContextMenuStrip = this.contextMenuStrip;
                    subNode.Nodes.Add(RootNode);
                }
                if (subNode.Nodes.Count != 0)
                {
                    CreateNode(subNode.Nodes, fullPath, name, fileInfo);
                }
            }
        }
        
        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            FileInfo fileInfo = new FileInfo(e.FullPath);
            if (fileInfo.Attributes == FileAttributes.Directory)
            {
                String path = e.FullPath;
                DeleteNode(this.treeView.Nodes, path);
                path = path.Replace(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Documents");
                DeleteNode(this.treeView.Nodes, path);
            }
        }

        private void DeleteNode(TreeNodeCollection nodeCollection, String fullPath)
        {
            String path;
            foreach (TreeNode subNode in nodeCollection)
            {
                path = ( (DirectoryInfo)this.treeView.SelectedNode.Tag).FullName;
                if (path == fullPath)
                {
                    subNode.Remove();
                }
                if (subNode.Nodes.Count != 0)
                {
                    DeleteNode(subNode.Nodes, fullPath);
                }
            }
        }

        private void BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                string path = ( (DirectoryInfo)e.Node.Tag).FullName;
                //this.textBoxFileName.Text = ( (DirectoryInfo)e.Node.Tag).Name;
                //this.textBoxCreationTime.Text = ( (DirectoryInfo)e.Node.Tag).CreationTime.ToString();
                //this.toolStripComboBoxPath.Text = path;                            

                pathconfig.EnterFullPath(path);
                GetItemsListView();
                LabelDeleteSince(0);
                AddLabelsDraw();                
            }
            
        }

        public void GetItemsListView(string select = null)
        {        
            listView1.Clear();
            SetListView1View();
            foreach (var file in pathconfig.GetFilesNames() )
            {
                listView1.Items.Add(file.ToString(), 4);                
            }   
            foreach (var folder in pathconfig.GetFolders() )
            {
                listView1.Items.Add(folder.ToString(), 0);
            }            

            FocusItemListview(select);
        }

        private void FocusItemListview(string select)
        {
            if (listView1.Items.Count > 0)
            {
                if (select == null && LastSelect == null)
                {
                    FirstItemFocusListview();
                }
                else
                {
                    FindFocusListview(select);
                }
                var itemFocus = If_NoFoundFocus();
                listView1.EnsureVisible(itemFocus.Index);
            }
            else
            {
                listView1.Items.Add(" �����", 7);
                FirstItemFocusListview();
            }
        }

        private ListViewItem If_NoFoundFocus()
        {
            var itemFocus = listView1.FocusedItem;
            if (itemFocus == null)
                itemFocus = FirstItemFocusListview();
            return itemFocus;
        }

        private void FindFocusListview(string select)
        {
            Ex.Try(() =>
            {
                string findSTR = (select == null) ? CheckLastSelect() : select;
                ListViewItem findItem = null;
                if (findSTR != null)
                {
                    //findItem = listView1.FindItemWithText(findSTR);
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Text?.ToLower() == findSTR.ToLower())
                        {
                            findItem = item;
                        }
                    }
                }
                if (findItem != null)
                {
                    var item = listView1.Items[findItem.Index];
                    item.Focused = true;
                    item.Selected = true;
                }
            });
        }

        private ListViewItem FirstItemFocusListview()
        {
            listView1.Items[0].Selected = true;
            listView1.Items[0].Focused = true;
            return listView1.Items[0];
        }

        private string CheckLastSelect()
        {
            try
            {
                string _return = LastSelect[pathconfig.Index + 1];
                return _return;
            }
            catch
            {
                return null;
            }
        }

        private void Go(object sender, EventArgs e)
        {
            //this.webBrowser.Navigate(this.toolStripComboBoxPath.Text);
            
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            //base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                //this.webBrowser.Navigate( ((DirectoryInfo)(sender as TreeView).SelectedNode.Tag).FullName);
                (sender as TreeView).SelectedNode.Expand();                
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        public void changeFontListView(int size)
        {
            listView1.Font = new Font("Arial", size);
            int imgsize = 16 + (size-16) / 2;
            //listView1.SmallImageList.ImageSize = new Size(20, 20);
            //imageList.ImageSize = new Size(20, 20);
            //imageList.Images
                //= new Size(imgsize, imgsize);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EnterListView();
        }

        private void EnterListView(string InputRightArrow = null)
        {
            bool RightArrow = TrueFalseCheck(InputRightArrow);

            if (listView1.FocusedItem != null)
            {
                if (RightArrow && listView1.FocusedItem.ImageIndex != 0)
                {
                    return;
                }
                if (listView1.FocusedItem.ImageIndex == 4)
                {
                    LaunchPorgram();
                    return;
                }
                if (listView1.FocusedItem.ImageIndex == 7)
                {
                    BackTo(pathconfig.Index);
                    return;
                }
                if (listView1.FocusedItem.ImageIndex == 0)
                {
                    pathconfig.EnterNextFolder(listView1.FocusedItem.Text);
                    GetItemsListView();
                    AddLabelsDraw(1);
                }
            }
        }

        private void LaunchPorgram()
        {
            IniFile ini = new IniFile(pathconfig.GetFilePath(listView1.FocusedItem.Text) );
            var section = ini.GetSection("exe");
            if (section != null)
            {
                foreach (IniFile.IniSection.IniKey fileName in section.Keys)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(fileName.Name);
                    }
                    catch (Exception ex1)
                    {
                        var local = Path.Combine(Environment.CurrentDirectory, fileName.Name);
                        try
                        {                            
                            System.Diagnostics.Process.Start(local);
                        }
                        catch (Exception ex2)
                        {
                            Ex.Show($"{ex1.Message}:\n{fileName.Name}\n\n{ex2.Message}:\n{local}");
                        }
                    }
                    break;
                }
                this.Close();
                return;
            }

            PublicData.operation = listView1.FocusedItem.Text;
            PublicData.fileOperation = pathconfig.GetFilePath(listView1.FocusedItem.Text);            
            PublicData.cancelLaunch = false;
            PublicData.DisplayModel = pathconfig.displayPathParts;
            PublicData.model = (PublicData.DisplayModel.Length > 0) ? PublicData.DisplayModel[0] : PublicData.operation; 
            PublicData.FolderConfigPath = pathconfig.FullPath;            

            var set = new SavingManager();
            set.Key(Setting.LastTVSelection).Value = String.Join("�", pathconfig.displayPathParts) + "�" + PublicData.operation;
            set.Save();

            this.Close();
        }

        private bool TrueFalseCheck(string InputRightArrow)
        {
            bool RightArrow = false;
            if(InputRightArrow != null)
                RightArrow = (InputRightArrow.ToLower() == "RightArrow".ToLower() ) ? true : false;            
            return RightArrow;
        }

        private void labelPath_MouseEnter(object sender, EventArgs e)
        {
            var label = (sender as Label);
            label.BorderStyle = BorderStyle.None;
            label.Font = new Font(label.Font, FontStyle.Underline);     
        }

        private void labelPath_MouseLeave(object sender, EventArgs e)
        {
            var label = (sender as Label);
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Font = new Font(label.Font, FontStyle.Regular);   
        }

        private void labelPath_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Label).BorderStyle = BorderStyle.Fixed3D;
        }

        private void labelPath_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as Label).BorderStyle = BorderStyle.None;
        }

        private void labelPath_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32
                (
                (sender as Label).Name.Substring(9) //PathLabel = 9 letters
                );

            BackTo(index);
        }

        private void BackTo(int index)
        {            
            if (index < 0) return;
            string selectForBack = LabelArray[index].Text;
            pathconfig.BackToTarget(index);

            LabelDeleteSince(index);
            GetItemsListView(selectForBack);            
        }

        public void LabelDeleteSince(int index=0)
        {
            if (index < 0) return;
            for (int i = index; i < LabelArray.Length; i++)
            {
                this.Controls.Remove(LabelArray[i]);
                LabelArray[i].Dispose();
            }

            Array.Resize(ref LabelArray, index);
        }

        //���������� ���� AddLabelsDraw() ���� AddLabelsDraw(1)
        void AddLabelsDraw(int income = 0) 
        {       
            int YLabel = 30;
            int startIndex = 0;

            var parts = pathconfig.displayPathParts;
            int size = parts.Length;
            if (size < 1) return;

            if (income == 1)
            {
                startIndex = size - 1;
                Array.Resize(ref LabelArray, size);
            } 
            else LabelArray = new Label[size];

            this.SuspendLayout();  
            for (int index = startIndex; index < size; index++)
            {
                LabelArray[index] = new System.Windows.Forms.Label();                
                Label newLabel = LabelArray[index];
                int newLabelPointX = pathconfig.LabelStartLocationX;
                if (index > 0)
                {
                    Label prevLabel = LabelArray[index - 1];
                    newLabelPointX = prevLabel.Size.Width + prevLabel.Location.X + pathconfig.LabelStepLocationX;
                }
                newLabel.Location = new System.Drawing.Point(newLabelPointX, YLabel);
                newLabel.Text = parts[index];
                newLabel.Name = "Pathlabel" + index.ToString();

                
                newLabel.AutoSize = true;
                newLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                newLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
                newLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                newLabel.Click += new System.EventHandler(this.labelPath_Click);
                newLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelPath_MouseDown);
                newLabel.MouseEnter += new System.EventHandler(this.labelPath_MouseEnter);
                newLabel.MouseLeave += new System.EventHandler(this.labelPath_MouseLeave);
                newLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelPath_MouseUp);
                this.splitContainer4.Panel1.Controls.Add(newLabel);
            }
            this.ResumeLayout(false);
            ResizeForm();
        }

        void ResizeForm()
        {
            int margin = 20;
            var label = LabelArray[LabelArray.Length-1];
            int summSize = label.Location.X + label.Size.Width;
            if (summSize > this.Size.Width + margin)
                this.Size = new Size(summSize + margin, this.Size.Height);
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsSelectForm OptionsForm = new OptionsSelectForm(this);            
            OptionsForm.ShowDialog();             
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();            
        }

        void SaveSettings()
        {
            var setting = new SavingManager();

            setting.Key(Setting.Split1).ValueInt = splitContainer1.SplitterDistance;
            setting.Key(Setting.Split3).ValueInt = splitContainer3.SplitterDistance;
            setting.Key(Setting.Split4).ValueInt = splitContainer4.SplitterDistance;

            setting.Key(Setting.FormSizeX).ValueInt = this.Size.Width;
            setting.Key(Setting.FormSizeY).ValueInt = this.Size.Height;
            setting.Key(Setting.FormLocationX).ValueInt = this.Location.X;
            setting.Key(Setting.FormLocationY).ValueInt = this.Location.Y;

            setting.Save();
        }

        void LoadSettings()
        {
            var setting = new SavingManager();
            if (!(setting.Key(Setting.LastTVSelection).Value == "") ) LastSelect = setting.Key(Setting.LastTVSelection).Value.Split(new char[] {'�'}, StringSplitOptions.RemoveEmptyEntries);  

            this.Location = new Point(setting.Key(Setting.FormLocationX).ValueInt, setting.Key(Setting.FormLocationY).ValueInt);
            this.Size = new Size(setting.Key(Setting.FormSizeX).ValueInt, setting.Key(Setting.FormSizeY).ValueInt); 

            splitContainer1.Panel2Collapsed = true;
            try
            {
                splitContainer1.SplitterDistance = setting.Key(Setting.Split1).ValueInt;
                splitContainer3.SplitterDistance = setting.Key(Setting.Split3).ValueInt;
                splitContainer4.SplitterDistance = setting.Key(Setting.Split4).ValueInt;
            } catch { }

            listView1.Font = new Font("Arial", setting.Key(Setting.FontListView).ValueInt);                      
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ex.Show("\"Shift + Tab\" - ��� �������� ������ �����");
        }

        private void ���������F5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(pathconfig.FullPath);
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (listView1.Columns.Count > 0) listView1.Columns[0].Width = listView1.ClientSize.Width;
        }
    }
}