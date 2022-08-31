using System;
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
    public partial class Form2 : Form
    {
        ListViewItem p;
        public Form2(ListViewItem v)
        {
            InitializeComponent();
            this.p = v;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            item_name.Text = p.Text;
            item_type.Text = p.SubItems[1].Text;
            item_ctime.Text = p.SubItems[4].Text;
            item_mname.Text = p.SubItems[2].Text;
            item_size.Text = p.SubItems[3].Text;
        }
    }
}
