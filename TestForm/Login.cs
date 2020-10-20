using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace TestForm
{



    //public delegate void EventHandler(string userName);

    public partial class Login : Form 
    {
        public event EventHandler loginEventHandler;
        //Man_Page mp;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            DbManager login = new DbManager();
            if (ControlCheck())
            {
                if (login.LoginCheck(textBox1.Text, textBox2.Text, out string proiroty))
                {
                    string userName = textBox1.Text;
                    //loginEventHandler(userName);
                    DialogResult = DialogResult.OK;
                    Man_Page mp = new Man_Page(proiroty);
                    this.Visible = false;
                    mp.Show();
                }
                else
                {
                    MessageBox.Show("로그인 실패");
                    textBox1.Clear();
                    textBox2.Clear();
                    
                }
            }

        }

        private bool ControlCheck()
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("아이디와 비밀번호를 입력해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("비밀번호를 입력해 주세요", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }
            return true;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
