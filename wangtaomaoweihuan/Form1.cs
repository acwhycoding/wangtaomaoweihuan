﻿using JHR_GetIcon;
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
            treeView1.Nodes.Add("root");

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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
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
            toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
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
    }
   
}
