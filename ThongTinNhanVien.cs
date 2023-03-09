using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QLNS
{
    public partial class Form3 : Form
    {
        // khởi tạo kết nối
        SqlConnection conn;
        SqlCommand cmd;
        String str = "Data Source=DESKTOP-BH3VIDG;Initial Catalog=QLNS_v2;Integrated Security=True";

        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        // khai báo biến
        string _devid;
        string _name;
        string _gender;
        string _birthday;
        string _cccd;
        string _address;
        string _phone;
        string _email;
        string _status;
        string _certificate;
        string _imgPath;

        //
        OpenFileDialog openFileDialog = new OpenFileDialog();

        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string devid, string name, string gender, string birthday,
                    string cccd, string address, string phone, string email, 
                    string status, string certificate, string imgPath)

        {
            InitializeComponent();
            _devid = devid;
            _name = name;
            _gender = gender;
            _birthday = birthday;
            _cccd = cccd;
            _address = address;
            _phone = phone;
            _email = email;
            _status = status;
            _certificate = certificate;
            _imgPath = imgPath;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // load bên bảng xem
            label_form3_manv.Text = _devid;
            label_form2_Hovaten.Text = _name;
            label_form2_gioitinh.Text = _gender;
            label_form2_ngaysinh.Text = _birthday;
            label_fom2_cccd.Text = _cccd;
            label_form2_diachi.Text = _address;
            label_form2_sdt.Text = _phone;
            label_form2_email.Text= _email;
            label_form2_trangthai.Text = _status;
            label_form2_bangcap.Text = _certificate;
            try
            {
                pictureBox1.Image = Image.FromFile(_imgPath);
            }
            catch
            {

            }
            
            
            
            // load bên bảng sửa
            label_tag2_manv.Text = _devid;
            textBox_Hovaten.Text = _name;
            comboBox_gioitinh.Text= _gender;
            textBox_ngaysinh .Text = _birthday;
            textBox_cccd.Text = _cccd;
            textBox_diachi .Text = _address;
            comboBox_trangthai.Text = _status;
            textBox_sdt.Text = _phone;
            textBox_email.Text = _email;
            comboBox_Bangcap.Text = _certificate;
            try
            {
                pictureBox2.Image= Image.FromFile(_imgPath);
            }
            catch
            {

            }
            
           
            //  khởi động kết nối
            conn = new SqlConnection(str);
            conn.Open();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn lưu ?","Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {

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
                        cmd.CommandText = "UPDATE Developer SET\r\n    " +
                            "Name = N'" + textBox_Hovaten.Text + "',\r\n    " +
                            "Gender = " + gender + ",\r\n    " +
                            "Birthday = '" + textBox_ngaysinh.Text + "',\r\n    " +
                            "Phone = '" + textBox_sdt.Text + "',\r\n    " +
                            "Email = '" + textBox_email.Text + "',\r\n    " +
                            "CitizenID = '" + textBox_cccd.Text + "',\r\n    " +
                            "Address = N'" + textBox_diachi.Text + "',\r\n    " +
                            "img_path = N'" + _imgPath + "',\r\n    " +
                            "Status = " + status + "\r\n" +
                            "WHERE DeveloperID = " + label_tag2_manv.Text + " -- Thay đổi ID cần cập nhật\r\n\r\n" +
                            "-- Cập nhật thông tin trong bảng certificate\r\n" +
                            "UPDATE certificate SET\r\n    " +
                            "certificateDetailsName = N'" + comboBox_Bangcap.Text + "'\r\n" +
                            "FROM certificate c\r\n" +
                            "INNER JOIN Developer d ON c.DeveloperID = d.DeveloperID\r\n" +
                            "WHERE d.DeveloperID = " + label_tag2_manv.Text + " -- Thay đổi ID cần cập nhật";
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Update thành công");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Update thất bại");
                    }
                }
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label_form2_Hovaten_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Thiết lập các thuộc tính cho OpenFileDialog
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn của tệp tin ảnh được chọn
                _imgPath = openFileDialog.FileName;

                // Hiển thị ảnh lên PictureBox
                try
                {
                    pictureBox2.Image = Image.FromFile(_imgPath);
                }
                catch
                {
                    MessageBox.Show("Ảnh quá lớn để thêm vào ");
                }   
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void tabPage6_Click(object sender, EventArgs e)
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
