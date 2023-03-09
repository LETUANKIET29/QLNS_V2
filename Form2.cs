using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class Form2 : Form
    {
        // khởi tạo kết nối
        SqlConnection conn;
        SqlCommand cmd;
        String str = "Data Source=DESKTOP-BH3VIDG;Initial Catalog=QLNS_v2;Integrated Security=True";

        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        // hỗ trợ lấy đường dẫn ảnh
        OpenFileDialog openFileDialog = new OpenFileDialog();

        // khai báo biến
        string imgpath;

        public Form2()
        {
            InitializeComponent();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            int gender = comboBox_gioitinh.Text == "Nam" ? 0 : 1;
            int status = comboBox_trangthai.Text == "Đang làm việc" ? 1 : 0;

            if (string.IsNullOrWhiteSpace(textBox_Hovaten.Text) ||
                string.IsNullOrWhiteSpace(textBox_ngaysinh.Text) ||
                !Regex.IsMatch(textBox_sdt.Text, @"^[0-9]{10}$") ||
                !Regex.IsMatch(textBox_email.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$") ||
                !Regex.IsMatch(textBox_cccd.Text, @"^[0-9]{12}$") ||
                string.IsNullOrWhiteSpace(textBox_diachi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và đúng định dạng.");
                return;
            }
            else
            {
                try
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "" +
                        "INSERT INTO [dbo].[Developer] \r\n([Name], [Gender], [Birthday], [Phone], [Email], [CitizenID], [Address], [Status], [img_path])" +
                        " \r\nVALUES\r\n(N'" + textBox_Hovaten.Text + "'," + gender + ",'" + textBox_ngaysinh.Text + "','" + textBox_sdt.Text + "','" + textBox_email.Text + "','" + textBox_cccd.Text + "',N'" + textBox_diachi.Text + "',1, '" + imgpath + "')" +
                        "DECLARE @DeveloperID int = SCOPE_IDENTITY();\r\nINSERT INTO certificate (DeveloperID, [certificateDetailsName])" +
                        "\r\nVALUES (@DeveloperID, N'" + comboBox_Bangcap.Text + "')";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thêm thành công !");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi không thêm được Nhân viên" + ex);
                }
            }     
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(str);
            conn.Open();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Thiết lập các thuộc tính cho OpenFileDialog
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn của tệp tin ảnh được chọn
                imgpath = openFileDialog.FileName;

                // Hiển thị ảnh lên PictureBox
                try
                {
                    pictureBox1.Image = Image.FromFile(imgpath);
                }
                catch(Exception ex)
                { 
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox_trangthai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Hovaten_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_Hovaten.Text))
            {
                errorProviderHoVaTen.SetError(textBox_Hovaten, "Vui lòng nhập tên.");
            }
            else
            {
                errorProviderHoVaTen.SetError(textBox_Hovaten, null);
            }
        }

        private void textBox_ngaysinh_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_ngaysinh.Text))
            {   
                errorProviderNgaySinh.SetError(textBox_ngaysinh, "Vui lòng nhập đúng định dạng ngày tháng.");
            }
            else
            {
                errorProviderNgaySinh.SetError(textBox_ngaysinh, null);
            }
        }

        private void textBox_sdt_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox_sdt.Text, @"^[0-9]{10}$"))
            {
                 errorProviderSDT.SetError(textBox_sdt, "Số điện thoại cần đủ 10 ký tự");
            }
            else
            {
                errorProviderSDT.SetError(textBox_sdt, null);
            }
        }

        private void textBox_email_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox_email.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {            
                errorProviderEmail.SetError(textBox_email, "Vui lòng nhập email hợp lệ.");
            }
            else
            {
                errorProviderEmail.SetError(textBox_email, null);
            }
        }

        private void textBox_cccd_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox_cccd.Text, @"^[0-9]{12}$"))
            {
                errorProviderCCCD.SetError(textBox_cccd, "Vui lòng nhập số CCCD hợp lệ.");
            }
            else
            {
                errorProviderCCCD.SetError(textBox_cccd, null);
            }
        }

        private void textBox_diachi_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_diachi.Text))
            {
                errorProviderHoVaTen.SetError(textBox_diachi, "Vui lòng nhập địa chỉ.");
            }
            else
            {
                errorProviderHoVaTen.SetError(textBox_diachi, null);
            }
        }
    }
}
