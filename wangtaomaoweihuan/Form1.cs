using JHR_GetIcon;
using Shell32;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace wangtaomaoweihuan
{
    public partial class Form1 : Form
    {
        ArrayList accesspaths = new ArrayList();
        GetIcon getIcon = new GetIcon();
        
        bool flag_pre_next = false;
        ArrayList copyobj = new ArrayList();
        bool iscut = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon[] myIcon;
            int[] myindexs = { 15, 34, 43, 8, 11, 7, 101, 4, 2, 0, 16, 17 };

            string[] mykeys = { "computer", "desktop", "favorites", "localdriver", "cdrom", "movabledriver", "recycle", "defaultfolder", "defaultexeicon", "unkonwicon", "printer", "network" };
            for (int i = 0; i < myindexs.Length; i++)
            {
                myIcon = getIcon.GetIconByIndex(myindexs[i]);
                imageList1.Images.Add(mykeys[i], myIcon[0]);
                imageList2.Images.Add(mykeys[i], myIcon[1]);
            }
            //初始化树形目录
            treeView1.ImageList = imageList1;
            treeView1.BeginUpdate();
            treeView1.Nodes.Clear();
            //桌面节点
            string mypath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            TreeNode desk = new TreeNode("桌面");
            desk.ImageKey = desk.SelectedImageKey = "desktop";
            desk.Tag = mypath;
            treeView1.Nodes.Add(desk);
            
            //我的电脑节点
            mypath = "mycomputer";
            TreeNode root = new TreeNode("我的电脑");
            root.SelectedImageKey = root.ImageKey = "computer";
            root.Tag = mypath;
            treeView1.Nodes.Add(root);

            GetDriverTree(root);

            root.Expand();

            mypath = "favorites";
            TreeNode tnf = new TreeNode("收藏夹");
            tnf.SelectedImageKey = tnf.ImageKey = "favorites";
            tnf.Tag = mypath;
            treeView1.Nodes.Add(tnf);

            //在收藏夹下节点添加：我的文件，我的图片，我的音乐，我的视频
            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            TreeNode tnl = new TreeNode("我的文档");
            myIcon = getIcon.GetIconByFileName(mypath, FileAttributes.Directory);
            imageList1.Images.Add("mydocument", myIcon[0]);
            imageList2.Images.Add("mydocument", myIcon[1]);
            tnl.SelectedImageKey = tnl.ImageKey = "mydocument";
            tnl.Tag = mypath;
            tnf.Nodes.Add(tnl);

            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            tnl = new TreeNode("我的音乐");
            myIcon = getIcon.GetIconByFileName(mypath, FileAttributes.Directory);
            if (myIcon != null)
            {
                imageList1.Images.Add("mymusic", myIcon[0]);
                imageList2.Images.Add("mymusic", myIcon[1]);
                tnl.SelectedImageKey = tnl.ImageKey = "mymusic";
                tnl.Tag = mypath;
                tnf.Nodes.Add(tnl);
            }

            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            tnl = new TreeNode("我的图片");
            myIcon = getIcon.GetIconByFileName(mypath, FileAttributes.Directory);
            if (myIcon != null)
            {
                imageList1.Images.Add("mypictures", myIcon[0]);
                imageList2.Images.Add("mypictures", myIcon[1]);
                tnl.SelectedImageKey = tnl.ImageKey = "mypicture";
                tnl.Tag = mypath;
                tnf.Nodes.Add(tnl);
            }

            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            tnl = new TreeNode("我的视频");
            myIcon = getIcon.GetIconByFileName(mypath, FileAttributes.Directory);
            if (myIcon != null)
            {
                imageList1.Images.Add("myvideos", myIcon[0]);
                imageList2.Images.Add("myvideos", myIcon[1]);
                tnl.SelectedImageKey = tnl.ImageKey = "myvideos";
                tnl.Tag = mypath;
                tnf.Nodes.Add(tnl);
            }
            mypath = "recycle";
            TreeNode tnr = new TreeNode("回收站");
            tnr.SelectedImageKey = tnr.ImageKey = "recycle";
            tnr.Tag = mypath;
            treeView1.Nodes.Add(tnr);

            treeView1.EndUpdate();
            GetDriverListview();
            accesspaths.Add("我的电脑");
            combo_url.DataSource = accesspaths;
            combo_url.SelectedIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
        }
        private void GetDriverListview()
        {
            listView1.Items.Clear();
            CreateCol_D();
            DriveInfo[] drivers = DriveInfo.GetDrives();
            string lvname1, lvname2, lvtype, keyname, lvtotal = "", lvfree = "";
            foreach (DriveInfo driver in drivers)
            {
                ListViewItem newitem = new ListViewItem();
                newitem.IndentCount = 1;
                if (driver.IsReady) lvname1 = driver.VolumeLabel;
                else lvname1 = "";
                lvname2 = driver.Name;
                switch (driver.DriveType)
                {
                    case DriveType.Fixed:
                        keyname = "localdriver";
                        lvtype = "本地磁盘";
                        if (lvname1.Equals("")) lvname1 = "本地磁盘";
                        newitem.Group = listView1.Groups["listViewGroup1"];
                        break;
                    case DriveType.Removable:
                        keyname = "movabledriver";
                        lvtype = "移动储存";
                        if (lvname1.Equals("")) lvname1 = "移动存储";
                        newitem.Group = listView1.Groups["listViewGroup2"];
                        break;
                    case DriveType.CDRom:
                        keyname = "cdrom";
                        lvtype = "光盘驱动器";
                        if (lvname1.Equals("")) lvname1 = "光盘驱动器";
                        newitem.Group = listView1.Groups["listViewGroup3"];
                        break;
                    default:
                        keyname = "movabledriver";
                        lvtype = "未知设备";
                        if (lvname1.Equals("")) lvname1 = "未知设备";
                        newitem.Group = listView1.Groups["listViewGroup4"];
                        break;
                }
                newitem.SubItems[0].Text = (lvname1 + "(" + lvname2.Substring(0, 2) + ")");
                newitem.SubItems.Add(lvtype);
                if (driver.IsReady)
                {
                    lvtotal = Math.Round(driver.TotalSize / (1024 * 1024 * 1024 * 1.0), 1).ToString() + "G";
                    lvfree = Math.Round(driver.TotalFreeSpace / (1024 * 1024 * 1024 * 1.0), 1).ToString() + "G";
                }
                newitem.SubItems.Add(lvtotal);
                newitem.SubItems.Add(lvfree);
                newitem.ImageKey = keyname;
                newitem.Tag = lvname2;
                listView1.Items.Add(newitem);
            }
            lb_ojbnum.Text = listView1.Items.Count.ToString();
        }


        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode tn = treeView1.SelectedNode;
            if (tn.Tag.Equals("mycomputer"))
            {
                tn.Nodes.Clear();
                GetDriverTree(tn);
            }
            else if (!tn.Tag.Equals("favorites"))
            {
                tn.Nodes.Clear();
                GetFolderTree(tn);
            }
        }

        private void GetFolderTree(TreeNode tn)
        {
            string folderpath = tn.Tag.ToString();
            string[] f_names = Directory.GetDirectories(folderpath);
            foreach (string fn in f_names)
            {
                DirectoryInfo dinfo = new DirectoryInfo(fn);
                TreeNode newtn = new TreeNode(dinfo.Name);
                newtn.Tag = dinfo.FullName;
                newtn.SelectedImageKey = newtn.ImageKey = "defaultfolder";
                try
                {
                    string[] temos = Directory.GetDirectories(fn);
                    if (temos.Length > 0) newtn.Nodes.Add("temp");
                }
                catch { }
                tn.Nodes.Add(newtn);

            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 详细列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void GetDriverTree(TreeNode root)
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();
            string keyname = "";
            string drivername = "";
            string drivertag = "";
            foreach (DriveInfo driver in drivers)
            {
                if (driver.IsReady) drivername = driver.VolumeLabel;
                else drivername = "";
                switch (driver.DriveType)
                {
                    case DriveType.Fixed:
                        keyname = "localdriver";
                        if (drivername.Equals("")) drivername = "本地磁盘";
                        break;
                    case DriveType.Removable:
                        keyname = "movabledriver";
                        if (drivername.Equals("")) drivername = "移动存储";
                        break;
                    case DriveType.CDRom:
                        keyname = "cdrom";
                        if (drivername.Equals("")) drivername = "光盘驱动器";
                        break;
                    default:
                        keyname = "movabledriver";
                        if (drivername.Equals("")) drivername = "未知设备";
                        break;
                }
                drivername = drivername + "(" + driver.Name.Substring(0, 2) + ")";
                drivertag = driver.Name;
                TreeNode tn = new TreeNode(drivername);
                tn.SelectedImageKey = tn.ImageKey = keyname;
                tn.Tag = drivertag;
                if (driver.IsReady)
                {

                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(driver.Name);
                        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                        if (dirs.Length > 0) tn.Nodes.Add("temp");
                    }
                    catch { }
                }
                root.Nodes.Add(tn);
            }
        }

        private void splitContainer1_Panel1_SizeChanged(object sender, EventArgs e)
        {
            combo_url.Width = splitContainer1.Panel1.Width - btn_next.Width - btn_pre.Width - 6;
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            lb_searching.Width = splitContainer1.Panel2.Width - btn_search.Width - 6;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void CreateCol_F()
        {
            listView1.Columns.Clear();

            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "名称";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 200;
            columnHeader1.Name = "chname";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "类型";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 100;
            columnHeader1.Name = "chtype";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "修改时间";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 120;
            columnHeader1.Name = "chmodify";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "大小";
            columnHeader1.TextAlign = HorizontalAlignment.Right;
            columnHeader1.Width = 120;
            columnHeader1.Name = "chtotal";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "创建时间";
            columnHeader1.TextAlign = HorizontalAlignment.Center;
            columnHeader1.Width = 120;
            columnHeader1.Name = "chcreate";
            listView1.Columns.Add(columnHeader1);
        }
        private void CreateCol_D()
        {
            listView1.Columns.Clear();

            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "名称";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 200;
            columnHeader1.Name = "chname";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "类型";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 100;
            columnHeader1.Name = "chtype";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "总大小";
            columnHeader1.TextAlign = HorizontalAlignment.Right;
            columnHeader1.Width = 120;
            columnHeader1.Name = "chtotal";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "可用大小";
            columnHeader1.TextAlign = HorizontalAlignment.Right;
            columnHeader1.Width = 120;
            columnHeader1.Name = "chfree";
            listView1.Columns.Add(columnHeader1);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            int flag = 0;
            switch (tn.Text)
            {
                case "桌面":
                    {
                        if (accesspaths.IndexOf("桌面") > -1) accesspaths.Remove("桌面");
                        accesspaths.Insert(0, "桌面");
                        GetDesktopListview(); break;
                    }
                case "我的电脑":
                    {
                        if (accesspaths.IndexOf("我的电脑") > -1) accesspaths.Remove("我的电脑");
                        accesspaths.Insert(0, "我的电脑");
                        GetDriverListview(); break;
                    }
                case "回收站":
                    {
                        if (accesspaths.IndexOf("回收站") > -1) accesspaths.Remove("回收站");
                        accesspaths.Insert(0, "回收站");
                        GetRecycleListView();
                        break;
                    }
                case "收藏夹":
                    {
                        if (accesspaths.IndexOf("收藏夹") > -1) accesspaths.Remove("收藏夹");
                        accesspaths.Insert(0, "收藏夹");
                        GetfavoritesListViev(); break;
                    }
                default:
                    {
                        flag = GetFolderListview(tn.Tag.ToString());
                        if (flag == 0)
                        {
                            if (accesspaths.IndexOf(tn.Tag.ToString()) > -1) accesspaths.Remove(tn.Tag.ToString());
                            accesspaths.Insert(0, tn.Tag.ToString());
                        }
                        break;
                    }
            }
            if (flag == 0)
            {
                combo_url.SelectedIndexChanged -= new EventHandler(combo_url_SelectedIndexChanged);
                combo_url.DataSource = null;
                combo_url.DataSource = accesspaths;
                combo_url.SelectedIndex = 0;
                combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
            }
            else MessageBox.Show("访问失败，缺少权限或设备未就绪", "错误");

        }

       

        private int GetFolderListview(string p)
        {
            listView1.Items.Clear();
            CreateCol_F();
            string[] dirs;
            string[] files;
            try
            {
                dirs = Directory.GetDirectories(p);//获取路径p的子目录
                files = Directory.GetFiles(p);//获取路径p下的文件
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return 1;
            }
            foreach (string dir in dirs) //处理目录对象
            {
                try
                {
                    DirectoryInfo dinfo = new DirectoryInfo(dir);
                    ListViewItem lv = new ListViewItem(dinfo.Name);
                    lv.Tag = dinfo.FullName;
                    lv.ImageKey = "defaultfolder";
                    lv.SubItems.Add("文件夹");//类型
                    lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                  
                    lv.SubItems.Add("");//大小
                    lv.SubItems.Add(dinfo.CreationTime.ToString());//创建时间
                    listView1.Items.Add(lv);
                }
                catch { }
            }
            foreach (string f in files)//处理文件对象
            {
                try
                {
                    FileInfo finfo = new FileInfo(f);
                    ListViewItem lv = new ListViewItem(finfo.Name);//名称
                    lv.Tag = finfo.FullName;
                    lv.ImageKey = GetFileIconKey(finfo.Extension, finfo.FullName);//根据扩展名提取图标
                    string typenane = getIcon.GetTypeName(finfo.FullName);//获取文件类型名称lv.SubItens.Add(typename)://类型
                    long size = finfo.Length;
                    lv.SubItems.Add(finfo.LastWriteTime.ToString());//修改时间
                    
                    string sizestring = "";
                    if (size < 1024) sizestring = size.ToString() + "Byte";
                    else sizestring = (size / 1024).ToString() + "KB";
                    lv.SubItems.Add(sizestring);//大小；
                    lv.SubItems.Add(finfo.CreationTime.ToString());//创建时间
                    listView1.Items.Add(lv);
                }
                catch { }
            }
            lb_ojbnum.Text = listView1.Items.Count.ToString();
            return 0;
        }

        private void GetfavoritesListViev()
        {

            listView1.Items.Clear();
            CreateCol_F();
            string mypath = "";

            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ListViewItem lv = new ListViewItem("我的文档");//名称
            lv.Tag = mypath;
            lv.ImageKey = "mydocument";
            DirectoryInfo dinfo = new DirectoryInfo(mypath);
            lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
            lv.SubItems.Add("文件夹");//类型
            lv.SubItems.Add("");//大小
            lv.SubItems.Add(dinfo.CreationTime.ToString());//创建时间
            listView1.Items.Add(lv);


            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (mypath != null && !mypath.Equals(""))
            {
                lv = new ListViewItem("我的音乐");//名称
                lv.Tag = mypath;
                lv.ImageKey = "mymusic";
                dinfo = new DirectoryInfo(mypath);
                lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                lv.SubItems.Add("文件夹");//类型
                lv.SubItems.Add("");//大小
                lv.SubItems.Add(dinfo.CreationTime.ToString());//创建时间
                listView1.Items.Add(lv);
            }
            if (mypath != null && !mypath.Equals(""))
            {
                lv = new ListViewItem("我的图片");//名称
                lv.Tag = mypath;
                lv.ImageKey = "mypicture";
                dinfo = new DirectoryInfo(mypath);
                lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                lv.SubItems.Add("文件夹");//类型
                lv.SubItems.Add("");//大小
                lv.SubItems.Add(dinfo.CreationTime.ToString());//创建时间
                listView1.Items.Add(lv);
            }
            if (mypath != null && !mypath.Equals(""))
            {
                lv = new ListViewItem("我的视频");//名称
                lv.Tag = mypath;
                lv.ImageKey = "mypicture";
                dinfo = new DirectoryInfo(mypath);
                lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                lv.SubItems.Add("文件夹");//类型
                lv.SubItems.Add("");//大小
                lv.SubItems.Add(dinfo.CreationTime.ToString());//创建时间
                listView1.Items.Add(lv);
            }
            lb_ojbnum.Text = listView1.Items.Count.ToString();

        }

        private void GetRecycleListView()
        {
            List<string> tmp = new List<string>();
            foreach(ListViewItem it in listView1.SelectedItems)
            {
                tmp.Add(it.Tag.ToString());
            }
            listView1.Items.Clear();
            CreateCol_R();
            Shell shell = new Shell();//引用C:windows system32 shell32.dll，命名空间Shell32
            Folder recycleBin = shell.NameSpace(10);
            foreach (FolderItem f in recycleBin.Items())
            {
                ListViewItem lv = new ListViewItem(f.Name);
                lv.Tag = f.Path;// 路径
                lv.IndentCount = 1;
                
                if (f.IsFolder)// 文件夹
                {
                    lv.ImageKey = "defaultfolder";
                }
                else
                {
                    lv.ImageKey = GetFileIconKey(f.Path.Substring(f.Path.LastIndexOf('.')), f.Path);
                }
                lv.SubItems.Add(f.Type);
                lv.SubItems.Add(f.Path);
                lv.SubItems.Add(f.ModifyDate.ToString());
                if (tmp.Contains(lv.Tag.ToString()))
                {
                    f.Verbs().Item(0).DoIt();
                    continue;
                }
                listView1.Items.Add(lv);
                lb_ojbnum.Text = listView1.Items.Count.ToString();
            }
        }

        private void CreateCol_R()
        {
            listView1.Columns.Clear();
            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "名称";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 200;
            columnHeader1.Name = "chname";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "类型";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 100;
            columnHeader1.Name = "chtype";
            listView1.Columns.Add(columnHeader1);

            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "位置";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 200;

            columnHeader1.Name = "chpath";
            listView1.Columns.Add(columnHeader1);
            columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "修改日期";
            columnHeader1.TextAlign = HorizontalAlignment.Left;
            columnHeader1.Width = 120;
            columnHeader1.Name = "chdel";
            listView1.Columns.Add(columnHeader1);

        }



        private void GetDesktopListview()
        {
            listView1.Items.Clear();
            CreateCol_F();
            ListViewItem lv = new ListViewItem("我的电脑");
            lv.Tag = "mycomputer";
            lv.ImageKey = "computer";
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            listView1.Items.Add(lv);

            lv = new ListViewItem("我的文档");
            lv.Tag = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            lv.ImageKey = "mydocument";
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            listView1.Items.Add(lv);

            lv = new ListViewItem("网络");
            lv.Tag = "network";
            lv.ImageKey = "network";
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            listView1.Items.Add(lv);

            lv = new ListViewItem("回收站");
            lv.Tag = "recycle";
            lv.ImageKey = "recycle";
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            lv.SubItems.Add("");
            listView1.Items.Add(lv);
            string[] dirs;
            string[] files;
            string p = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                dirs = Directory.GetDirectories(p);//获取路径p的子目录
                files = Directory.GetFiles(p);//获取路径p下的文件

                foreach (string dir in dirs)  //处理目录
                {
                    try
                    {
                        DirectoryInfo dinfo = new DirectoryInfo(dir);
                        lv = new ListViewItem(dinfo.Name);
                        lv.Tag = dinfo.FullName;
                        lv.ImageKey = "defaultfolder";
                        lv.SubItems.Add("文件夹");//类型
                        lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                       
                        lv.SubItems.Add("");//大小
                        lv.SubItems.Add(dinfo.CreationTime.ToString());//创建时间
                        listView1.Items.Add(lv);
                    }
                    catch { }
                }
                foreach (string f in files)//读取文件
                {
                    try
                    {
                        FileInfo finfo = new FileInfo(f);
                        lv = new ListViewItem(finfo.Name);//名称
                        lv.Tag = finfo.FullName;
                        lv.ImageKey = GetFileIconKey(finfo.Extension, finfo.FullName);//根据扩展名提取图标
                        
                        string typenane = getIcon.GetTypeName(finfo.FullName);//获取文件类型名称
                        lv.SubItems.Add(typenane);//类型
                        lv.SubItems.Add(finfo.LastWriteTime.ToString());//修改时间
                        long size = finfo.Length;
                        string sizestring = "";
                        if (size < 1024) sizestring = size.ToString() + "Byte";
                        else sizestring = (size / 1024).ToString() + "KB";
                        lv.SubItems.Add(sizestring);//大小；
                        lv.SubItems.Add(finfo.CreationTime.ToString());//创建时间
                        listView1.Items.Add(lv);
                    }
                    catch { }
                }
            }
            catch { }

            lb_ojbnum.Text = listView1.Items.Count.ToString();

        }

        private string GetFileIconKey(string exten, string fullname)
        {
            //Console.WriteLine(exten + "   " + fullname);
            string imgkey = "";
            Icon[] myIcon;
            //提取可执行文件/快捷方式的专用图标，如果失败则使用默认可执行文件图标/未知文件图标
            if (exten.ToUpper().Equals(".EXE") || exten.ToUpper().Equals(".LNK"))
            {
                myIcon = getIcon.GetIconByFileName(fullname, FileAttributes.Normal);
                if (myIcon != null)
                {
                    if (myIcon[0] != null && myIcon[1] != null)
                    { //更新该类型文件的图标
                        if (imageList1.Images.ContainsKey(fullname)) imageList1.Images.RemoveByKey(fullname);
                        if (imageList2.Images.ContainsKey(fullname)) imageList2.Images.RemoveByKey(fullname);
                        imageList1.Images.Add(fullname, myIcon[0]);
                        imageList2.Images.Add(fullname, myIcon[1]);
                        imgkey = fullname;
                    }
                }
                if (imgkey == "") // 如果获取图标失败，则设置默认图标
                {
                    if (exten.ToUpper().Equals(".EXE")) imgkey = "defaultexeicon";
                    else imgkey = "unknowicon";
                }
            }
            else
            {
                myIcon = getIcon.GetIconByFileType(exten);
                if (myIcon != null)
                {
                    if (myIcon[0] != null && myIcon[1] != null)
                    {
                        if (imageList1.Images.ContainsKey(exten)) imageList1.Images.RemoveByKey(exten);
                        if (imageList2.Images.ContainsKey(exten)) imageList2.Images.RemoveByKey(exten);
                        imageList1.Images.Add(exten, myIcon[0]);
                        imageList2.Images.Add(exten, myIcon[1]);
                        imgkey = exten;
                    }
                    else imgkey = "unknowicon";
                }
                else imgkey = "unknowicon";
            }
            //Console.WriteLine(imgkey);
            return imgkey;
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem fitem = listView1.FocusedItem;
            string fullname = fitem.Tag.ToString();
            string urltext = combo_url.Text;
            string mytype = fitem.SubItems[1].Text;
            //Console.WriteLine(urltext);
            if (urltext.Equals("我的电脑"))
            {
                DriveInfo driveInfo = new DriveInfo(fullname);
                if (driveInfo.IsReady) mytype = "文件夹";
                else
                {
                    MessageBox.Show("设备未就绪，无法读取","提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            if (urltext.Equals("回收站"))
            {
                MessageBox.Show("回收站的对象不能直接访问！");
                return;
            }
            if (urltext.Equals("桌面") && fitem.SubItems[0].Text.Equals("网络"))
            {
                MessageBox.Show("网上邻居功能还未实现！");
                return;
            }
            if (urltext.Equals("桌面") && fitem.SubItems[0].Text.Equals("我的文档"))
            {
                mytype = "文件夹";
            }
            switch (mytype)
            {
                case "文件夹":
                    accesspaths.Insert(0, fullname);
                    GetFolderListview(fullname);
                    combo_url.SelectedIndexChanged -= new EventHandler(combo_url_SelectedIndexChanged);
                    combo_url.DataSource = null;
                    combo_url.DataSource = accesspaths;
                    combo_url.SelectedIndex = 0;
                    combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
                    break;
                case "":
                    if(fitem.SubItems[0].Text.Equals("我的电脑"))
                    {
                        if(accesspaths.IndexOf("我的电脑") > -1) accesspaths.Remove("我的电脑");
                        accesspaths.Insert(0, "我的电脑");
                        GetDriverListview();

                        combo_url.SelectedIndexChanged -= new EventHandler(combo_url_SelectedIndexChanged);
                        combo_url.DataSource = null;
                        combo_url.DataSource = accesspaths;
                        combo_url.SelectedIndex = 0;
                        combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
                    }
                    if (fitem.SubItems[0].Text.Equals("回收站"))
                    {
                        if (accesspaths.IndexOf("回收站") > -1) accesspaths.Remove("回收站");
                        accesspaths.Insert(0, "回收站");
                        GetRecycleListView();

                        combo_url.SelectedIndexChanged -= new EventHandler(combo_url_SelectedIndexChanged);
                        combo_url.DataSource = null;
                        combo_url.DataSource = accesspaths;
                        combo_url.SelectedIndex = 0;
                        combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
                    }
                    break;
                default:
                    try
                    {
                        System.Diagnostics.Process.Start(fullname);
                    }
                    catch
                    {
                        MessageBox.Show("无法打开或运行文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    break;
            }
        }
        private void btn_pre_Click(object sender, EventArgs e)
        {
            //后退按钮
            if (combo_url.SelectedIndex == combo_url.Items.Count - 1)
            {
                MessageBox.Show("已经是后退的最后一个目录了", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                flag_pre_next = true;
                combo_url.SelectedIndex += 1;
            }

        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            //前进按钮
            if (combo_url.SelectedIndex == 0)
            {
                MessageBox.Show("已经是前进的最后一个目录了", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                flag_pre_next = true;
                combo_url.SelectedIndex -= 1;
            }
        }

        private void toolStripSplitButton3_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripSplitButton tsb = (ToolStripSplitButton)sender;
            for (int i = 0; i < tsb.DropDownItems.Count; i++)
            {
                if (tsb.DropDownItems[i] != e.ClickedItem)
                    ((ToolStripMenuItem)tsb.DropDownItems[i]).Checked = false;
                else ((ToolStripMenuItem)tsb.DropDownItems[i]).Checked = true;

            }
            switch (e.ClickedItem.Text)
            {
                case "大图标": listView1.View = View.LargeIcon; break;
                case "小图标": listView1.View = View.SmallIcon; break;
                case "列表": listView1.View = View.List; break;
                case "详细列表": listView1.View = View.Details; break;
                default: listView1.View = View.Tile; break;
            }
        }

        private void contextMenu_lv_Opening(object sender, CancelEventArgs e)
        {
            //复制，剪切，粘贴，删除，重命名，新建，刷新，属性
            if (listView1.SelectedItems.Count == 0)//右键位置是1istview1的空白处
            {
                contextMenu_lv.Items["item_copy"].Enabled = false;
                contextMenu_lv.Items["item_cut"].Enabled = false;
                if (copyobj.Count == 0) contextMenu_lv.Items["item_paste"].Enabled = false;
                else contextMenu_lv.Items["item_paste"].Enabled = true;

                contextMenu_lv.Items["item_delete"].Enabled = false;
                contextMenu_lv.Items["item_rename"].Enabled = false;
                contextMenu_lv.Items["item_new"].Enabled = true;
                contextMenu_lv.Items["item_refresh"].Enabled = true;
                contextMenu_lv.Items["item_attr"].Enabled = false;
            }
            else
            {
                contextMenu_lv.Items["item_copy"].Enabled = true;
                contextMenu_lv.Items["item_cut"].Enabled = true;
                contextMenu_lv.Items["item_paste"].Enabled = false;
                contextMenu_lv.Items["item_delete"].Enabled = true;
                contextMenu_lv.Items["item_rename"].Enabled = true;
                contextMenu_lv.Items["item_new"].Enabled = false;
                contextMenu_lv.Items["item_refresh"].Enabled = false;
                contextMenu_lv.Items["item_attr"].Enabled = true;
            }
        }


        private void contextMenu_lv2_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)//右键位置是1istview1的空白处
            {
                //我的电脑和收藏夹：刷新有效； 回收站：刷新和清空有效
                contextMenu_lv2.Items["item_refresh2"].Enabled = true;
                contextMenu_lv2.Items["item_open"].Enabled = false;
                contextMenu_lv2.Items["item_revert"].Enabled = false;
                contextMenu_lv2.Items["item_del"].Enabled = false;
                if (combo_url.Text.Equals("回收站"))
                    contextMenu_lv2.Items["item_empty"].Enabled = true;
                else contextMenu_lv2.Items["item_empty"].Enabled = false;

            }
            else
            {
                //我的电脑和收藏夹：打开有效： 回收站：还原和删除有效
                contextMenu_lv2.Items["item_empty"].Enabled = false;
                contextMenu_lv2.Items["item_refresh2"].Enabled = false;
                if (combo_url.Text.Equals("回收站"))
                {
                    contextMenu_lv2.Items["item_open"].Enabled = false;
                    contextMenu_lv2.Items["item_revert"].Enabled = true;
                    contextMenu_lv2.Items["item_del"].Enabled = true;
                }
                else
                {
                    contextMenu_lv2.Items["item_open"].Enabled = true;
                    contextMenu_lv2.Items["item_revert"].Enabled = false;
                    contextMenu_lv2.Items["item_del"].Enabled = false;
                }

            }
        }
      

        private void doEmpty()
        {
            try
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    string fullname = listView1.Items[i].Tag.ToString();
                    if (File.Exists(fullname))
                    {
                        FileSystem.DeleteFile(fullname, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                    }
                    else if (Directory.Exists(fullname))
                        FileSystem.DeleteDirectory(fullname, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                    else MessageBox.Show(fullname + "，删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            combo_url_SelectedIndexChanged(null, null);
        }

        private void doRevert()
        {
            GetRecycleListView();
            combo_url_SelectedIndexChanged(null, null);
        }

        private void doRecycleDel()
        {
            if (listView1.SelectedItems.Count == 0) return;
            try
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    string fullname = listView1.SelectedItems[i].Tag.ToString();
                    Console.WriteLine(fullname);
                    if (File.Exists(fullname))
                    {
                        FileSystem.DeleteFile(fullname, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                    }
                    else
                        if (Directory.Exists(fullname))
                        FileSystem.DeleteDirectory(fullname, UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently);
                    else MessageBox.Show(fullname + "，删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                combo_url_SelectedIndexChanged(null, null);
            }
        }

        private void OpenObj(ListViewItem listViewItem)
        {
            
        }

        private void donew(string newtype)
        {
            string newname = "";
            string newext = "";
            switch (newtype)
            {
                case "folder": newname = "新建文件夹"; break;
                case "word": newname = "新建word文档"; newext = ".doc"; break;
                case "txt": newname = "新建文本文档"; newext = ".txt"; break;
                case "excel": newname = "新建excel文档"; newext = ".xls"; break;
                case "ppt": newname = "新建演示文稿"; newext = ".ppt"; break;
            }
            try
            {
                if (newtype.Equals("folder"))
                {
                    int i = 1;
                    string temp = newname;
                    while (Directory.Exists(Path.Combine(combo_url.Text, newname))) newname = temp + i++.ToString() + newext;
                    Directory.CreateDirectory(Path.Combine(combo_url.Text, newname));
                    ListViewItem lv = new ListViewItem(newname);
                    lv.Tag = Path.Combine(combo_url.Text, newname);
                    lv.ImageKey = "defaultfolder";
                    lv.IndentCount = 1;
                    lv.SubItems.Add(DateTime.Now.ToString());
                    lv.SubItems.Add("文件夹");
                    
                    lv.SubItems.Add("");
                    lv.SubItems.Add(DateTime.Now.ToString());
                    listView1.Items.Add(lv);
                    listView1.Items[listView1.Items.Count - 1].Selected = true;
                }
                else
                {
                    int i = 1;
                    string temp = newname;
                    newname += newext;
                    while (File.Exists(Path.Combine(combo_url.Text, newname))) newname = temp + i++.ToString() + newext;
                    File.Create(Path.Combine(combo_url.Text, newname));
                    ListViewItem lv = new ListViewItem(newname);
                    lv.Tag = Path.Combine(combo_url.Text, newname);
                    lv.ImageKey = GetFileIconKey(newext, Path.Combine(combo_url.Text, newname));
                    lv.IndentCount = 1;
                    string typename = getIcon.GetTypeName(Path.Combine(combo_url.Text, newname));
                    lv.SubItems.Add(typename);
                    lv.SubItems.Add(DateTime.Now.ToString());
                    lv.SubItems.Add("");
                    lv.SubItems.Add(DateTime.Now.ToString());
                    listView1.Items.Add(lv);
                    listView1.Items[listView1.Items.Count - 1].Selected = true;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void showattr(ListViewItem v)
        {
            Form2 form2 = new Form2(v);
            form2.ShowDialog();
        }

        private void dorename()
        {
            if (listView1.SelectedItems.Count == 0) return;
            string oldname = listView1.SelectedItems[0].SubItems[0].Text;
            if (oldname.Equals("我的电脑") || oldname.Equals("网络") || oldname.Equals("回收站") ||
            oldname.Equals("我的文档")) return;
            listView1.LabelEdit = true;
            listView1.SelectedItems[0].BeginEdit();

        }

        private void dodelete()
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    string fullname = listView1.SelectedItems[i].Tag.ToString();
                    //Console.WriteLine(fullname);
                    if (File.Exists(fullname))
                    {
                        FileSystem.DeleteFile(@fullname, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                       
                    }
                    else
                        if (Directory.Exists(fullname))
                        FileSystem.DeleteDirectory(fullname, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    else MessageBox.Show(fullname + ",删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
            combo_url_SelectedIndexChanged(null, null);
        }

        private void dopaste()
        {
            string currpath = combo_url.Text;
            if (currpath.Equals("桌面")) currpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Console.WriteLine(copyobj.Count);
            //如果复制对象数为0，或目录不存在，或源目录和目的目录相同，则返回
            if (copyobj.Count == 0 || !Directory.Exists(currpath) || currpath.Equals(Directory.GetParent(copyobj[0].ToString()).Name)) return;
            for (int i = 0; i < copyobj.Count; i++)
            {
                if (File.Exists(copyobj[i].ToString())) copycut_file(copyobj[i].ToString(), currpath);//文件
                else if (Directory.Exists(copyobj[i].ToString())) copycut_directory(copyobj[i].ToString(), currpath);//目录
            }
            if (iscut)
            {
                copyobj.Clear();
            }
            combo_url_SelectedIndexChanged(null, null);
        }

        private void copycut_directory(string s, string d)
        {
            try
            {
                DirectoryInfo sinfo = new DirectoryInfo(s);
                string dname = sinfo.Name;
                string destfullpath = Path.Combine(d, dname);
                DialogResult result = DialogResult.Yes;
                if (Directory.Exists(destfullpath))
                {
                    result = MessageBox.Show("目录“" + dname + "”已经存在，是否覆盖。”", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Directory.Delete(destfullpath, true);
                    }
                    else return;
                }
                DirectoryInfo dinfo = new DirectoryInfo(destfullpath);
                dinfo.Create();
                FileInfo[] files = sinfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.CopyTo(Path.Combine(destfullpath, file.Name), true);//复制源目录中所有文件
                }
                DirectoryInfo[] dirs = sinfo.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    copycut_directory(dir.FullName, destfullpath);//复制源目录中所有的子目录及其内容
                }
                if (iscut == true) sinfo.Delete(true);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void copycut_file(string fullname, string destpath)
        {
            try
            {
                FileInfo finfo = new FileInfo(fullname);
                string filename = finfo.Name;
                string currpath = destpath;
                string destfullnane = Path.Combine(currpath, filename);//目的完整留径及名称
                DialogResult result = DialogResult.Yes;
                if (File.Exists(destfullnane))
                    result = MessageBox.Show("文件" + filename + "已经存在，是否覆盖·", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    finfo.CopyTo(destfullnane, true);
                    if (iscut == true) File.Delete(fullname);//如果是剪切，则删除源文件
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void docut()
        {
            if (listView1.SelectedItems.Count == 0) return;
            iscut = true;
            copyobj.Clear();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
                copyobj.Add(listView1.SelectedItems[i].Tag.ToString());
        }

        private void docopy()
        {
            if (listView1.SelectedItems.Count == 0) return;
            iscut = false;
            copyobj.Clear();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                 copyobj.Add(listView1.SelectedItems[i].Tag.ToString());
                 //Console.WriteLine(listView1.SelectedItems[i].Tag.ToString());
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label.Trim() == "" || e.Label.Trim().Equals(listView1.Items[e.Item].SubItems[0].Text.Trim()))
                e.CancelEdit = true;
            else
            {
                string newname = e.Label.Trim();
                try
                {
                    if (File.Exists(listView1.Items[e.Item].Tag.ToString()))//如果是文件
                    {
                        if (File.Exists(Path.Combine(combo_url.Text, newname)))//如果新文件名已经存在
                        {
                            MessageBox.Show("文件名已经存在，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                        }
                        else
                        {
                            File.Move(listView1.Items[e.Item].Tag.ToString(), Path.Combine(combo_url.Text, newname));
                            listView1.Items[e.Item].Tag = Path.Combine(combo_url.Text, newname);
                        }
                    }
                    else
                        if (Directory.Exists(listView1.Items[e.Item].Tag.ToString()))//如果是文件夹
                    {
                        if (Directory.Exists(Path.Combine(combo_url.Text, newname)))//如果新文件名已经存在
                        {
                            MessageBox.Show("文件夹已经存在，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                        }
                        else
                        {
                            Directory.Move(listView1.Items[e.Item].Tag.ToString(), Path.Combine(combo_url.Text, newname));
                            listView1.Items[e.Item].Tag = Path.Combine(combo_url.Text, newname);
                        }
                    }
                }
                catch { }
            }
            listView1.LabelEdit = false;
        }

        private void doSearchFile(string topfolder, string content)
        {
            if (Directory.Exists(topfolder))
            {
                try
                {
                    string[] files = Directory.GetFiles(topfolder);
                    foreach (string f in files)
                    {
                        try
                        {
                            FileInfo finfo = new FileInfo(f);
                            if (finfo.Name.ToUpper().IndexOf(content.ToUpper()) != -1)
                            {
                                ListViewItem lv = new ListViewItem(finfo.Name);//名称
                                lv.Tag = finfo.FullName;
                                lv.IndentCount = 1;
                                lv.ImageKey = GetFileIconKey(finfo.Extension, finfo.FullName);//根据扩展名提取图标
                                string typename = getIcon.GetTypeName(finfo.FullName);//获取文件类型名称
                                lv.SubItems.Add(typename);//类型
                                lv.SubItems.Add(finfo.FullName);//位置
                                lv.SubItems.Add(finfo.LastWriteTime.ToString());//修改时间
                                listView1.Items.Add(lv);
                            }
                        }
                        catch { }
                    }
                    string[] dirs = Directory.GetDirectories(topfolder);
                    foreach (string d in dirs) doSearchFile(d, content);
                }
                catch { }
            }

        }

        private void combo_url_SelectedIndexChanged(object sender, EventArgs e)
        {
            int flag = 0;
            string newpath = combo_url.Text.Trim();
            switch (newpath)
            {
                case "桌面":
                    // 如果不是前进或后退引起的地址变换，则把选中的路径调整到最新位置
                    {
                        if (flag_pre_next == false)
                        {
                            accesspaths.Remove("桌面");
                            accesspaths.Insert(0, "桌面");
                        }

                        GetDesktopListview(); break;
                    }
                case "我的电脑":
                    {
                        if (flag_pre_next == false)
                        {
                            accesspaths.Remove("我的电脑");
                            accesspaths.Insert(0, "我的电脑");
                        }

                        GetDriverListview(); break;
                    }

                case "回收站"://回收站目前不提供还原功能
                    {
                        if (flag_pre_next == false)
                        {
                            accesspaths.Remove("回收站");
                            accesspaths.Insert(0, "回收站");
                        }

                        GetRecycleListView(); break;
                    }

                case "收藏夹":
                    {
                        if (flag_pre_next == false)
                        {
                            accesspaths.Remove("收藏夹");
                            accesspaths.Insert(0, "收藏夹");
                        }

                        GetfavoritesListViev(); break;
                    }
                default:
                    {
                        flag = GetFolderListview(newpath);
                        if (flag_pre_next == false)
                        {
                            accesspaths.Remove(newpath);
                            accesspaths.Insert(0, newpath);
                        }

                        break;
                    }

            }


            if (flag_pre_next == false)//重新绑定combo_ur1
            {
                combo_url.SelectedIndexChanged -= new EventHandler(combo_url_SelectedIndexChanged);//**** combo_url.DataSource = null;
                combo_url.DataSource = accesspaths;
                combo_url.SelectedIndex = 0;
                combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);//wr
            }
            if (flag == 1)
            {
                listView1.Items.Clear();
                MessageBox.Show("访问失败，缺少权限或设备未就绪", "错误");
            }
            flag_pre_next = false;

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string currpath = combo_url.Text;
            //string t = "";
            switch (currpath)
            {
                case "桌面":
                    break;
                case "回收站": combo_url.Text = "桌面"; break;
                case "我的电脑": combo_url.Text = "桌面"; break;
                case "收藏夹": combo_url.Text = "桌面"; break;
                case "C:\\":
                case "D:\\":
                case "E:\\":
                case "F:\\":
                case "G:\\":
                    combo_url.Text = "我的电脑";
                    break;
            
                default:
                    {
                        try
                        {
                            combo_url.Text = Directory.GetParent(currpath).FullName;
                        }
                        catch
                        {
                            combo_url.Text = "我的电脑";
                        }
                        break;
                    }

            }
            combo_url.SelectedIndexChanged -= new EventHandler(combo_url_SelectedIndexChanged);
            combo_url.DataSource = null;
            combo_url.DataSource = accesspaths;
            combo_url.SelectedIndex = 0;
            combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
            combo_url_SelectedIndexChanged(null, null);

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            combo_url_SelectedIndexChanged(null, null);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            donew("folder");
        }

        private void contextMenu_item_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            switch (tsi.Name)//根据菜单项的变量名区分单击的菜单项功能
            {
                case "item_copy": docopy(); break;//复制
                case "item_cut": docut(); break;//剪切
                case "item_paste": dopaste(); break;//粘贴
                case "item_delete": dodelete(); break;//删除
                case "item_rename": dorename(); break;//重命名
                case "item_refresh": combo_url_SelectedIndexChanged(null, null); break; //刷新" +
                case "item_attr": showattr(listView1.SelectedItems[0]); break;//属性
                case "item_newfolder": donew("folder"); break;//新建文件夹
                case "item_newword": donew("word"); break;//新建word文档
                case "item_newtxt": donew("txt"); break;//新建文本文档
                case "item_newexcel": donew("excel"); break;//新建excel文档
                case "item_newppt": donew("ppt"); break;//新建演示文稿
                case "item_open": listView1_ItemActivate(sender, e); break;//打开文件夹或设备
                case "item_del": doRecycleDel(); combo_url_SelectedIndexChanged(null, null); break;//回收站-删除(即彻底删除)
                case "item_revert": doRevert(); break;//回收站-还原
                case "item_empty": doEmpty(); break;//回收站-清空
                case "item_refresh2": combo_url_SelectedIndexChanged(null, null); break;//刷新

            }

        }

        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            string folderpath = combo_url.Text;
            if(folderpath.Equals("回收站")|| folderpath.Equals("收藏夹") || folderpath.Equals("我的电脑"))
                listView1.ContextMenuStrip = contextMenu_lv2;
            else listView1.ContextMenuStrip = contextMenu_lv;
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (lb_searching.Text.Trim().Equals(""))
            {
                combo_url_SelectedIndexChanged(null, null);
                return;
            }
            if (combo_url.Text.Equals("回收站"))
            {
                for (int i = listView1.Items.Count - 1; i >= 0; i--)
                {
                    string temp = listView1.Items[i].SubItems[0].Text.ToUpper();
                    if (temp.IndexOf(lb_searching.Text.Trim().ToUpper()) == -1) listView1.Items.RemoveAt(i);
                }
            }
            else
            {
                lb_searching.Visible = true;
                lb_ojbnum.Text = "0";
                statusStrip1.Refresh();//刷新显示，让正在搜索的文字得以显示
                                       //this.Refresh();
                listView1.Items.Clear();
                CreateCol_R();
                string topfolder = combo_url.Text;
                if (topfolder.Equals("我的电脑"))
                {
                    DriveInfo[] drives = DriveInfo.GetDrives();
                    foreach (DriveInfo drive in drives)
                        if (drive.IsReady) doSearchFile(drive.Name, lb_searching.Text.Trim());
                }
                else if (topfolder.Equals("收藏夹"))
                {
                    doSearchFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), lb_searching.Text.Trim());
                    doSearchFile(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), lb_searching.Text.Trim());
                    doSearchFile(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), lb_searching.Text.Trim());
                    doSearchFile(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), lb_searching.Text.Trim());
                }
                else
                    doSearchFile(topfolder, lb_searching.Text.Trim());
                

            }
            lb_ojbnum.Text = listView1.Items.Count.ToString();
            lb_searching.Visible = true;
        }
      
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView1.Columns[e.Column].Tag == null)
            {
                listView1.Columns[e.Column].Tag = true;
            }
            bool tabK = (bool)listView1.Columns[e.Column].Tag;
            if (tabK)
            {
                listView1.Columns[e.Column].Tag = false;
            }
            else
            {
                listView1.Columns[e.Column].Tag = true;
            }
            listView1.ListViewItemSorter = new ListViewSort(e.Column, listView1.Columns[e.Column].Tag);
            // 指定排序器并传送列索引与升序降序关键字
            listView1.Sort();
        }
        public class ListViewSort : IComparer
        {
            private int col;
            private bool descK;

            public ListViewSort()
            {
                col = 0;
            }
            public ListViewSort(int column, object Desc)
            {
                descK = (bool)Desc;
                col = column;  // 当前列,0,1,2...,参数由ListView控件的ColumnClick事件传递
            }
            public int Compare(object x, object y)
            {
                int tempInt = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                if (descK)
                {
                    return -tempInt;
                }
                else
                {
                    return tempInt;
                }
            }
        }
    }

}
