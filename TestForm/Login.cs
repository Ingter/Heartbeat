using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{

    public delegate void EventHandler(string userName);

    public partial class Login : Form 
    {
        public event EventHandler loginEventHandler;
        //Man_Page mp;
        public Login()
        {
            InitializeComponent();
        }
        /*
        public Login(Man_Page mp)
        {
            InitializeComponent();
            this.mp = mp;
        }
        */
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Regis_Page RP = new Regis_Page();
            RP.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginHandler loginHandler = new LoginHandler();
            if (ControlCheck())
            {
                if (loginHandler.LoginCheck(textBox1.Text, textBox2.Text))
                {
                    string userName = textBox1.Text;
                    loginEventHandler(userName);
                    DialogResult = DialogResult.OK;
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
