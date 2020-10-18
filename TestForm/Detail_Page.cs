using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.IO;
using ChartDirector;

namespace TestForm
{
    public partial class Detail_Page : Form
    {
        string imgPath = "";
        string ImgName = "";
        public string strConn = "Server=192.168.0.173;" +
                                 "Database=heartbeat;" +
                                 "Uid=test;" +
                                 "Pwd=1234;" +
                                 "charset=utf8;";


        public MySqlConnection conn;
        public MySqlCommand cmd;
        public MySqlDataReader rdr;


        private DateTime[] timeStamps1;
        private DateTime[] timeStamps2;

        private double[] dataTemp = null;
        private double[] dataHeartbeat = null;


        private string dp_value;

        public string Passvalue
        {
            get { return this.dp_value; }
            set { this.dp_value = value; }  // 다른폼에서 전달받은 값을 쓰기
        }

        Man_Page mp;
        public Detail_Page(Man_Page _mp)
        {
            InitializeComponent();
            mp = _mp;
        }

        public Detail_Page()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Detail_Page_Load(object sender, EventArgs e)
        {


            UInt32 FileSize;
            byte[] rawData;
            FileStream fs;
            int ImgNum = 0;

            string SQL = "";
            string SQL2 = "";

            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            emp_id.Text = Passvalue;

            // 직원 이미지 불러오기
            cmd.CommandText = ($"select * from image where ImageNo = {Passvalue}");
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string imgNum = rdr["ImageNo"].ToString();
                ImgName = rdr["Image_name"] as string;

                if (imgNum != "")   // 등록해뒀던 이미지가 있는 직원만 이미지 불러오도록 함
                {
                    pictureBox1.Image = Image.FromFile(ImgName);
                }
            }
            rdr.Close();

            // 직원 정보 불러오기
            cmd.CommandText = ($"select * from emp_info where emp_id = {Passvalue}");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"].ToString();

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string blood_type = rdr["blood_type"] as string;

                string Dept_id = rdr["dept_id"].ToString();

                na.Text = Emp_name;
                emp_d.Text = Dept_id;
                emp_tel.Text = Emp_tel;
                emp_addr.Text = Emp_addr;
                emp_etel.Text = Emp_emer_tel;
                emp_bl.Text = blood_type;



            }

            conn.Close();

            conn.Open();
            cmd.CommandText = ($"select * from emp_detail  where emp_id = {Passvalue}");
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"].ToString();

                string Emp_name = rdr["emp_name"] as string;

                string Body_temp = rdr["body_temp"].ToString();

                string Heart_rate = rdr["heart_rate"].ToString();

                string[] rw = new string[] { Emp_name, Body_temp, Heart_rate };

                dataGridView1.Rows.Add(rw);

            }


            loadData();

            initChartViewer(winChartViewer1);
            initChartViewer(winChartViewer2);

            drawChart(winChartViewer1, timeStamps1, dataHeartbeat, 0x70ff46, true); //RED, GREEN, BLUE
            drawChart(winChartViewer2, timeStamps2, dataTemp, 0xff5846, true);

            winChartViewer1.updateViewPort(true, true);
            winChartViewer2.updateViewPort(true, true);



            conn.Close();





        }



        private void loadData()
        {
            string pathHeart = System.Windows.Forms.Application.StartupPath + "\\" + "Heartbeat.log";

            string[] strBuffer = System.IO.File.ReadAllLines(pathHeart);

            timeStamps1 = new DateTime[strBuffer.Length];
            dataHeartbeat = new double[strBuffer.Length];

            //MessageBox.Show(Convert.ToString(strBuffer.Length));
            //System.Diagnostics.Trace.Write(strTime.ToString());

            for (int i = 0; i < strBuffer.Length; i++)
            {
                bool blineCheck;
                string[] Str_temp = strBuffer[i].Split(new Char[] { });

                blineCheck = DateTime.TryParse(Str_temp[0], out timeStamps1[i]);

                if (!blineCheck)
                {
                    MessageBox.Show(string.Format("로그 파일에 알수 없는 문자열이 있습니다. Line : [{0}]", i));
                    continue;
                }
                dataHeartbeat[i] = Convert.ToDouble(Str_temp[1]);
            }

            //string pathTemp = @"E:\TrendTest\WindowsFormsApp1\WindowsFormsApp1\Temperature.log";
            string pathTemp = System.Windows.Forms.Application.StartupPath + "\\" + "Temperature.log";

            string[] strBuffer2 = System.IO.File.ReadAllLines(pathTemp);

            timeStamps2 = new DateTime[strBuffer2.Length];
            dataTemp = new double[strBuffer2.Length];

            for (int i = 0; i < strBuffer2.Length; i++)
            {
                bool blineCheck;
                string[] Str_temp = strBuffer2[i].Split(new Char[] { });

                blineCheck = DateTime.TryParse(Str_temp[0], out timeStamps2[i]);

                if (!blineCheck)
                {
                    MessageBox.Show(string.Format("로그 파일에 알수 없는 문자열이 있습니다. Line : [{0}]", i));
                    continue;
                }
                dataTemp[i] = Convert.ToDouble(Str_temp[1]);
            }
        }



        private void initChartViewer(WinChartViewer viewer)
        {
            viewer.MouseWheelZoomRatio = 1.1;
            viewer.MouseUsage = WinChartMouseUsage.ScrollOnDrag;
        }




        private void winChartViewer1_Click(object sender, EventArgs e)
        {
            trackLineLabel((XYChart)winChartViewer1.Chart, winChartViewer1.PlotAreaMouseX);
            winChartViewer1.updateDisplay();
        }


        StringBuilder sb_d = new StringBuilder();

        //public void createChart(WinChartViewer viewer, int chartIndex)
        public void drawChart(WinChartViewer viewer, DateTime[] timeStamps, double[] data, int color, bool init)
        {
            int startIndex, endIndex;

            // Get the start date and end date that are visible on the chart.
            DateTime viewPortStartDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft));
            DateTime viewPortEndDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft +
                viewer.ViewPortWidth));

            // Extract the part of the data arrays that are visible.
            DateTime[] viewPortTimeStamps = null;
            double[] viewPortData = null;

            if (init == true)
            {
                startIndex = 0;
                endIndex = data.Length;
            }
            else
            {
                startIndex = (int)Math.Floor(Chart.bSearch(timeStamps, viewPortStartDate));//Trend
                endIndex = (int)Math.Ceiling(Chart.bSearch(timeStamps, viewPortEndDate));//Trend
            }

            int noOfPoints = endIndex - startIndex + 1;

            viewPortTimeStamps = (DateTime[])Chart.arraySlice(timeStamps, startIndex, noOfPoints);
            viewPortData = (double[])Chart.arraySlice(data, startIndex, noOfPoints);

            XYChart c = new XYChart(776, 197, 0x000000, 0x000000);

            c.setPlotArea(30, 10, c.getWidth() - 40, c.getHeight() - 50,
                0x000000, -1, Chart.Transparent,
                c.dashLineColor(0xffffff, 0x000303), c.dashLineColor(0xffffff, 0x000303));

            c.xAxis().setColors(0xffffff, 0xffffff, 0xffffff);
            c.yAxis().setColors(0xffffff, 0xffffff, 0xffffff);

            c.setClipping();

            sb_d.Clear();
            sb_d.AppendFormat("{{value|hh:nn:ss.f}}");

            c.xAxis().setLabelFormat(sb_d.ToString());

            c.setClipping();

            LineLayer layer = c.addLineLayer2();

            layer.setLineWidth(2);
            layer.setFastLineMode();

            layer.setXData(viewPortTimeStamps);
            layer.addDataSet(viewPortData, color, "Heart");

            c.xAxis().setDateScale(viewPortStartDate, viewPortEndDate);

            viewer.syncDateAxisWithViewPort("x", c.xAxis());

            c.xAxis().setTickDensity(75);
            c.yAxis().setTickDensity(30);

            c.xAxis().setLabelStep(100);

            c.xAxis().setMinTickInc(1);

            viewer.Chart = c;

            if (viewer.ImageMap == null)
            {
                viewer.ImageMap = viewer.Chart.getHTMLImageMap("", "", "");
            }
        }

        //
        // Draw the track line with legend
        //
        private void trackLineLabel(XYChart c, int mouseX)
        {
            // Clear the current dynamic layer and get the DrawArea object to draw on it.
            DrawArea d = c.initDynamicLayer();

            // The plot area object
            PlotArea plotArea = c.getPlotArea();

            // Get the data x-value that is nearest to the mouse, and find its pixel coordinate.
            double xValue = c.getNearestXValue(mouseX);
            int xCoor = c.getXCoor(xValue);

            // Draw a vertical track line at the x-position
            d.vline(plotArea.getTopY(), plotArea.getBottomY(), xCoor, d.dashLineColor(0x000000, 0x0101));

            // Draw a label on the x-axis to show the track line position.
            string xlabel = "<*font,bgColor=000000*> " + c.xAxis().getFormattedLabel(xValue, "hh:nn:ss.f") +
                " <*/font*>";
            TTFText t = d.text(xlabel, "Arial Bold", 8);

            // Restrict the x-pixel position of the label to make sure it stays inside the chart image.
            int xLabelPos = Math.Max(0, Math.Min(xCoor - t.getWidth() / 2, c.getWidth() - t.getWidth()));
            t.draw(xLabelPos, plotArea.getBottomY() + 6, 0xffffff);

            // Iterate through all layers to draw the data labels
            for (int i = 0; i < c.getLayerCount(); ++i)
            {
                Layer layer = c.getLayerByZ(i);

                // The data array index of the x-value
                int xIndex = layer.getXIndexOf(xValue);

                // Iterate through all the data sets in the layer
                for (int j = 0; j < layer.getDataSetCount(); ++j)
                {
                    ChartDirector.DataSet dataSet = layer.getDataSetByZ(j);

                    // Get the color and position of the data label
                    int color = dataSet.getDataColor();
                    int yCoor = c.getYCoor(dataSet.getPosition(xIndex), dataSet.getUseYAxis());

                    // Draw a track dot with a label next to it for visible data points in the plot area
                    if ((yCoor >= plotArea.getTopY()) && (yCoor <= plotArea.getBottomY()) && (color !=
                        Chart.Transparent) && (!string.IsNullOrEmpty(dataSet.getDataName())))
                    {

                        d.circle(xCoor, yCoor, 4, 4, color, color);

                        string label = "<*font,bgColor=" + color.ToString("x") + "*> " + c.formatValue(
                            dataSet.getValue(xIndex), "{value|P4}") + " <*/font*>";
                        t = d.text(label, "Arial Bold", 8);

                        // Draw the label on the right side of the dot if the mouse is on the left side the
                        // chart, and vice versa. This ensures the label will not go outside the chart image.
                        if (xCoor <= (plotArea.getLeftX() + plotArea.getRightX()) / 2)
                        {
                            //t.draw(xCoor + 5, yCoor, 0xffffff, Chart.Left);
                            t.draw(xCoor + 5, yCoor, 0x000000, Chart.Left);
                        }
                        else
                        {
                            //t.draw(xCoor - 5, yCoor, 0xffffff, Chart.Right);
                            t.draw(xCoor - 5, yCoor, 0x000000, Chart.Right);
                        }
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Detail_Page dp = new Detail_Page();

                    conn = new MySqlConnection(strConn);
                    cmd = new MySqlCommand();
                    conn.Open();
                    cmd.Connection = conn;

                    cmd.CommandText = ($"delete from emp_info where emp_id = {Passvalue}");
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    conn.Open();
                    cmd.CommandText = ($"delete from image where imageNO = {emp_id.Text}");
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("직원 삭제 완료");

                    dp.Close();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    MessageBox.Show("직원 삭제 실패");
                    this.Close();
                }



                this.Close();
            }
            else { }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Detail_Page dp = this;
            Emp_Update eu = new Emp_Update(dp);
            eu.Passvalue2 = Passvalue;  // 전달자(Passvalue)를 통해서 dp페이지로 전달
            eu.ShowDialog(this);


        }

        private void Detail_Page_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn = new MySqlConnection(strConn);
            cmd = new MySqlCommand();
            conn.Open();
            cmd.Connection = conn;

            cmd.CommandText = ("select * from emp_info;");
            rdr = cmd.ExecuteReader();
            mp.dataGridView1.Rows.Clear();

            while (rdr.Read())
            {
                string Emp_id = rdr["emp_id"].ToString();

                string Emp_name = rdr["emp_name"] as string;

                string Emp_email = rdr["emp_email"] as string;

                string Emp_tel = rdr["emp_tel"] as string;

                string Emp_addr = rdr["emp_addr"] as string;

                string Emp_emer_tel = rdr["emp_emer_tel"] as string;

                string Blood_type = rdr["blood_type"] as string;

                string dept_id = rdr["dept_id"].ToString();

                string[] emp_info = new string[] { Emp_id, Emp_name, Emp_email, Emp_tel, Emp_addr, Emp_emer_tel, Blood_type, dept_id };

                mp.dataGridView1.Rows.Add(emp_info);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void winChartViewer1_ViewPortChanged_1(object sender, WinViewPortEventArgs e)
        {
            if (e.NeedUpdateChart)
                drawChart(winChartViewer1, timeStamps1, dataHeartbeat, 0x70ff46, false);

        }

        private void winChartViewer2_ViewPortChanged_1(object sender, WinViewPortEventArgs e)
        {
            if (e.NeedUpdateChart)
                drawChart(winChartViewer2, timeStamps2, dataTemp, 0xff5846, false);

        }

        private void winChartViewer2_Click_1(object sender, EventArgs e)
        {
            trackLineLabel((XYChart)winChartViewer2.Chart, winChartViewer2.PlotAreaMouseX);
            winChartViewer2.updateDisplay();
        }
    }
}
