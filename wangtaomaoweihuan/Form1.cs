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

namespace wangtaomaoweihuan
{
    public partial class Form1 : Form
    {
        ArrayList accesspaths = new ArrayList();
        GetIcon getIcon = new GetIcon();
        Icon[] myIcon;
        int[] myindexs = { 15, 34, 43, 8, 11, 7, 101, 4, 2, 0, 16, 17 };

        string[] mykeys = { "computer", "desktop", "favorites", "localdriver", "cdrom", "movabledriver", "recycle", "defaultfolder", "defaultexeicon", "unkonwicon", "printer", "network" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            combo_url.SelectedIndexChanged += new EventHandler(combo_url_SelectedIndexChanged);
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode tn = treeView1.SelectedNode;
            if (tn.Tag.Equals("mycomputer"))
            {
                tn.Nodes.Clear();
                GetDriverTree(tn);
            }
            else
                if (!tn.Tag.Equals("favorites"))
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

        private void pictureBox2_Click(object sender, EventArgs e)
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
            text_search.Width = splitContainer1.Panel2.Width - btn_search.Width - 6;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDiverListview();
        }

        private void GetDiverListview()
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
                        newitem.Group = listView1.Groups["lvGroup1"];
                        break;
                    case DriveType.Removable:
                        keyname = "movabledriver";
                        lvtype = "移动储存";
                        if (lvname1.Equals("")) lvname1 = "移动存储";
                        newitem.Group = listView1.Groups["lvGroup2"];
                        break;
                    case DriveType.CDRom:
                        keyname = "cdrom";
                        lvtype = "光盘驱动器";
                        if (lvname1.Equals("")) lvname1 = "光盘驱动器";
                        newitem.Group = listView1.Groups["lvGroup3"];
                        break;
                    default:
                        keyname = "movabledriver";
                        lvtype = "未知设备";
                        if (lvname1.Equals("")) lvname1 = "未知设备";
                        newitem.Group = listView1.Groups["lvGroup4"];
                        break;
                }
                newitem.SubItems[0].Text = (lvname1 + "(" + lvname2.Substring(0, 2) + ")");
                newitem.SubItems.Add(lvtype);
                if (driver.IsReady)
                {
                    lvtotal = Math.Round(driver.TotalSize / (1024 * 1024 * 1.0), 1).ToString() + "G";
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
                        if (accesspaths.IndexOf("收藏夹") > -1) accesspaths.Remove("收藏夹");
                        accesspaths.Insert(0, "收藏夹");
                        GetDesktopListview(); break;
                    }
                case "我的电脑":
                    {
                        if (accesspaths.IndexOf("我的电脑") > -1) accesspaths.Remove("我的电脑");
                        accesspaths.Insert(0, "我的电脑");
                        GetDriveListview(); break;
                    }
                case "回收站":
                    {
                        if (accesspaths.IndexOf("回收站") > -1) accesspaths.Remove("回收站");
                        accesspaths.Insert(0, "回收站");
                        GetRecyleListView();
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
                combo_url.DataSource = null;
                combo_url.DataSource = accesspaths;
                combo_url.SelectedIndex = 0;
            }
            else MessageBox.Show("访问失败，缺少权限或设备未就绪", "错误");

        }

        private void GetDriveListview()
        {
            throw new NotImplementedException();
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
                    lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                    lv.SubItems.Add("文件夹");//类型
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
                    lv.SubItems.Add(finfo.LastWriteTime.ToString());//修改时间
                    string typenane = getIcon.GetTypeName(finfo.FullName);//获取文件类型名称lv.SubItens.Add(typename)://类型
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

        private void GetRecyleListView()
        {
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
                    lv.SubItems.Add(f.Type);
                    lv.SubItems.Add(f.Path);
                    lv.SubItems.Add(f.ModifyDate.ToString());
                    listView1.Items.Add(lv);
                }
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

                        lv.SubItems.Add(dinfo.LastWriteTime.ToString());//修改时间
                        lv.SubItems.Add("文件夹");//类型
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
                        lv.ImageKey = GetFileIconKey(finfo.Extension, finfo.FullName);//根据扩展名提取图标lv. SubItens.Add(finfo.LastWriteTine.ToString())://修改时间
                        string typenane = getIcon.GetTypeName(finfo.FullName);//获取文件类型名称lv.SubItens.Add(typenane)://类型
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

        }

        private string GetFileIconKey(string exten, string fullname)
        {
            string imgkey = "";
            Icon[] myIcon;
            //提取可执行文件/快捷方式的专用图标，如果失败则使用默认可执行文件图标/未知文件图标
            if (exten.ToUpper().Equals(".EXE") || exten.ToUpper().Equals("LNK"))
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
            if (urltext.Equals("桌面"))
            {
                MessageBox.Show("网上邻居功能还未实现！");
                return;
            }
            if (urltext.Equals("我的电脑") && fitem.SubItems[0].Text.Equals("网络"))
            {
                mytype = "文件夹";
            }
            switch (mytype)
            {
                case "文件夹":
                    break;
                case "":
                    if(fitem.SubItems[0].Text.Equals("我的电脑"))
                    {
                        if(accesspaths.IndexOf("我的电脑") > -1) accesspaths.Remove("我的电脑");
                        accesspaths.Insert(0, "我的电脑");
                        GetDriveListview();

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
                        GetRecyleListView();

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
                            accesspaths.Remove("点面");
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

                        GetDriveListview(); break;
                    }

                case "回收站"://回收站目前不提供还原功能
                    {
                        if (flag_pre_next == false)
                        {
                            accesspaths.Remove("回收站");
                            accesspaths.Insert(0, "回收站");
                        }

                        GetRecyleListView(); break;
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
    }
}

}
