using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wangtaomaoweihuan
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
            ArrayList accesspaths = new ArrayList();
            GetIcon getIcon = new GetIcon;
            Icon[] myIcon;
            int[] myindexs = { 15, 34, 43, 8, 11, 7, 101, 4, 2, 0, 16, 17 };

            string[] mykeys = { "computer", "desktop", "favorites", "localdriver", "cdrom", "movabledriver", "recycle", "defaultfolder", "defaultexeicon", "unkonwicon", "printer", "network" };
            
            for(int i=0;i<myindexs.Length;i++)
            {
                myIcon = getIcon.GetIcon(myindexs[i]);
                imageList1.Images.Add(mykeys[i],myIcon[0]);
                imageList2.Images.Add(mykeys[i], myIcon[1]);

            }
        }
        GetIconByFileName(string fileName, FileAttributes att);

        private void 资源管理器_Load(object sender, EventArgs e)
        {

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

        private void 详细liebiaoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {   //初始化树形目录
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
            tnf.Tag= mypath;
            treeView1.Nodes.Add(tnf);

            //在收藏夹下节点添加：我的文件，我的图片，我的音乐，我的视频
            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            TreeNode tnl = new TreeNode("我的文档");
            myIcon = geticon,GetIconbyFileName(mypath, FileAttributes.Directory);
            imageList1.Images.Add("mydocument",myIcon[0]);
            imageList2.Images.Add("mydocument", myIcon[1]);
            tnl.SelectedImageKey = tnl.ImageKey = "mydocument";
            tnl.Tag = mypath;
            tnf.Nodes.Add(tnl);

            mypath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);


        }

        private void splitContainer1_Panel1_SizeChanged(object sender, EventArgs e)
        {
            combo_url.Width = splitContainer1.Panel1.Width - btn_next.Width - btn_pre.Width - 6; 
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            text_search.Width=splitContainer1.Panel2.Width-btn_search.Width-6;
        }
    }
}
