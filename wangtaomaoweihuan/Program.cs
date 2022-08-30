using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wangtaomaoweihuan
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< HEAD
            Application.Run(new GetIconByFileName());
=======
            Application.Run(new Form1());
>>>>>>> 35f13cb1d9bab82b84a0801561ed389d124df459
        }
    }
}
