namespace TestForm
{
    partial class Detail_Page
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.시간 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.심박수 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.체온 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.emp_id = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.na = new System.Windows.Forms.Label();
            this.emp_d = new System.Windows.Forms.Label();
            this.emp_addr = new System.Windows.Forms.Label();
            this.emp_bl = new System.Windows.Forms.Label();
            this.emp_tel = new System.Windows.Forms.Label();
            this.emp_etel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(64, 48);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 168);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(64, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 48);
            this.button1.TabIndex = 1;
            this.button1.Text = "회원수정";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(64, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 48);
            this.button2.TabIndex = 2;
            this.button2.Text = "회원삭제";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.시간,
            this.심박수,
            this.체온});
            this.dataGridView1.Location = new System.Drawing.Point(312, 336);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(576, 112);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // 시간
            // 
            this.시간.HeaderText = "시간";
            this.시간.MinimumWidth = 6;
            this.시간.Name = "시간";
            // 
            // 심박수
            // 
            this.심박수.HeaderText = "심박수";
            this.심박수.MinimumWidth = 6;
            this.심박수.Name = "심박수";
            // 
            // 체온
            // 
            this.체온.HeaderText = "체온";
            this.체온.MinimumWidth = 6;
            this.체온.Name = "체온";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(312, 504);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "주소";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(592, 472);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "연락처: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(312, 472);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "부서번호";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(592, 504);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "긴급연락처";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(312, 536);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "혈액형";
            // 
            // emp_id
            // 
            this.emp_id.AutoSize = true;
            this.emp_id.Location = new System.Drawing.Point(112, 272);
            this.emp_id.Name = "emp_id";
            this.emp_id.Size = new System.Drawing.Size(48, 15);
            this.emp_id.TabIndex = 10;
            this.emp_id.Text = "직원id";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(312, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(576, 120);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.Location = new System.Drawing.Point(312, 192);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(576, 120);
            this.panel2.TabIndex = 12;
            // 
            // na
            // 
            this.na.AutoSize = true;
            this.na.Location = new System.Drawing.Point(112, 240);
            this.na.Name = "na";
            this.na.Size = new System.Drawing.Size(23, 15);
            this.na.TabIndex = 13;
            this.na.Text = "na";
            // 
            // emp_d
            // 
            this.emp_d.AutoSize = true;
            this.emp_d.Location = new System.Drawing.Point(392, 472);
            this.emp_d.Name = "emp_d";
            this.emp_d.Size = new System.Drawing.Size(50, 15);
            this.emp_d.TabIndex = 14;
            this.emp_d.Text = "emp_d";
            // 
            // emp_addr
            // 
            this.emp_addr.AutoSize = true;
            this.emp_addr.Location = new System.Drawing.Point(392, 504);
            this.emp_addr.Name = "emp_addr";
            this.emp_addr.Size = new System.Drawing.Size(70, 15);
            this.emp_addr.TabIndex = 15;
            this.emp_addr.Text = "emp_addr";
            // 
            // emp_bl
            // 
            this.emp_bl.AutoSize = true;
            this.emp_bl.Location = new System.Drawing.Point(392, 536);
            this.emp_bl.Name = "emp_bl";
            this.emp_bl.Size = new System.Drawing.Size(53, 15);
            this.emp_bl.TabIndex = 16;
            this.emp_bl.Text = "emp_bl";
            // 
            // emp_tel
            // 
            this.emp_tel.AutoSize = true;
            this.emp_tel.Location = new System.Drawing.Point(648, 472);
            this.emp_tel.Name = "emp_tel";
            this.emp_tel.Size = new System.Drawing.Size(57, 15);
            this.emp_tel.TabIndex = 17;
            this.emp_tel.Text = "emp_tel";
            // 
            // emp_etel
            // 
            this.emp_etel.AutoSize = true;
            this.emp_etel.Location = new System.Drawing.Point(688, 504);
            this.emp_etel.Name = "emp_etel";
            this.emp_etel.Size = new System.Drawing.Size(65, 15);
            this.emp_etel.TabIndex = 18;
            this.emp_etel.Text = "emp_etel";
            // 
            // Detail_Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 582);
            this.Controls.Add(this.emp_etel);
            this.Controls.Add(this.emp_tel);
            this.Controls.Add(this.emp_bl);
            this.Controls.Add(this.emp_addr);
            this.Controls.Add(this.emp_d);
            this.Controls.Add(this.na);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.emp_id);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Detail_Page";
            this.Text = "Detail_Page";
            this.Load += new System.EventHandler(this.Detail_Page_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 시간;
        private System.Windows.Forms.DataGridViewTextBoxColumn 심박수;
        private System.Windows.Forms.DataGridViewTextBoxColumn 체온;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label emp_id;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label na;
        private System.Windows.Forms.Label emp_d;
        private System.Windows.Forms.Label emp_addr;
        private System.Windows.Forms.Label emp_bl;
        private System.Windows.Forms.Label emp_tel;
        private System.Windows.Forms.Label emp_etel;
    }
}

