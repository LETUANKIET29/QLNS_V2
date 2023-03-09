using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class FormAdmin : Form
    {
        // khởi tạo kết nối
        SqlConnection conn;
        SqlCommand cmd;
        String str = "Data Source=DESKTOP-BH3VIDG;Initial Catalog=QLNS_v2;Integrated Security=True";

        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        // khai báo biến
        String accid;
        String name;
        String gender;
        String birthday;
        String cccd;
        String address;
        String phone;
        String email;
        String status;
        String certificate;
        String imgPath;

        public void loadData()
        {
            try
            {
                conn = new SqlConnection(str);
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select d.DeveloperID, " +
                    "\r\nd.Name,\r\ncase when d.Gender= 0 then N'Nam' " +
                    "else N'Nữ' END as 'Gender',\r\n CONVERT(varchar, d.Birthday, 23) as 'ngày sinh',\r\nd.CitizenID, d.img_path," +
                    "\r\nd.Address,\r\nd.Phone,\r\nd.email,\r\ncase " +
                    "when d.status= 0 then N'Đã thôi việc' else N'Đang làm việc'" +
                    " END as 'Trạng thái',\r\nc.certificateDetailsName as " +
                    "'Certificate'\r\n\r\nfrom\r\nDeveloper d join certificate c on d.DeveloperID = c.DeveloperID";
                adapter.SelectCommand = cmd;
                table.Clear();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[5].Visible = false;
            }
            catch
            {

            }
        }

        public FormAdmin()
        {
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(str);
            conn.Open();
            loadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hệThốngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // hiển thị thông tin dựa theo hàng đã bấm vào
            int i;
            i = dataGridView1.CurrentRow.Index;

            accid = dataGridView1.Rows[i].Cells[0].Value.ToString();
            name = dataGridView1.Rows[i].Cells[1].Value.ToString();
            gender = dataGridView1.Rows[i].Cells[2].Value.ToString();
            birthday = dataGridView1.Rows[i].Cells[3].Value.ToString();
            cccd = dataGridView1.Rows[i].Cells[4].Value.ToString();
            imgPath = dataGridView1.Rows[i].Cells[5].Value.ToString();
            address = dataGridView1.Rows[i].Cells[6].Value.ToString();
            phone = dataGridView1.Rows[i].Cells[7].Value.ToString();
            email = dataGridView1.Rows[i].Cells[8].Value.ToString();
            status = dataGridView1.Rows[i].Cells[9].Value.ToString();
            certificate = dataGridView1.Rows[i].Cells[10].Value.ToString();          

            Form3 f3 = new Form3(accid, name, gender, birthday,
                cccd,address, phone, email, status, certificate, imgPath);
            f3.Show();
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {
            Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // hiển thị thông tin dựa theo hàng đã bấm vào
            int i;
            i = dataGridView1.CurrentRow.Index;

            accid = dataGridView1.Rows[i].Cells[0].Value.ToString();
            name = dataGridView1.Rows[i].Cells[1].Value.ToString();
            gender = dataGridView1.Rows[i].Cells[2].Value.ToString();
            birthday = dataGridView1.Rows[i].Cells[3].Value.ToString();
            cccd = dataGridView1.Rows[i].Cells[4].Value.ToString();
            address = dataGridView1.Rows[i].Cells[5].Value.ToString();
            phone = dataGridView1.Rows[i].Cells[6].Value.ToString();
            email = dataGridView1.Rows[i].Cells[7].Value.ToString();
            status = dataGridView1.Rows[i].Cells[8].Value.ToString();
            certificate = dataGridView1.Rows[i].Cells[9].Value.ToString();

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();     
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            conn = new SqlConnection(str);
            conn.Open();
            loadData();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            conn.Close();
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này ?", "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText =
                        "DELETE FROM certificate " +
                        "WHERE DeveloperID IN (SELECT DeveloperID FROM Developer " +
                        "WHERE DeveloperID = @accid); " +
                        "DELETE FROM Developer " +
                        "WHERE DeveloperID = @accid";
                    cmd.Parameters.AddWithValue("@accid", accid);
                    cmd.ExecuteNonQuery();
                    loadData();
                    MessageBox.Show("Đã xóa thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa nhân viên: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comming Soon !!");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                // Tạo câu lệnh SQL với chuỗi tìm kiếm đầy đủ và truyền giá trị vào trước đó
                string sql = "Select * From Developer Where Name Like '%@Name%'";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@Name", txtSearch.Text);

                // Thực thi câu lệnh SQL
                command.ExecuteNonQuery();
                loadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tìm thấy, vui lòng thử lại" + ex.Message);
            }
            finally
            {
                // Đóng kết nối sau khi truy vấn
                conn.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }

        private void ToExcel(DataGridView dataGridView1, string fileName)
        {
            //khai báo thư viện hỗ trợ Microsoft.Office.Interop.Excel
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            try
            {
                //Tạo đối tượng COM.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                //tạo mới một Workbooks bằng phương thức add()
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                //đặt tên cho sheet
                worksheet.Name = "Quản lý Developers";

                if (dataGridView1 != null)
                {
                    // export headers in the first row
                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }

                    // export data in the remaining rows
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                        }
                    }
                }

                // Thiết lập độ rộng cho các cột.
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    Microsoft.Office.Interop.Excel.Range columnRange = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                    columnRange.EntireColumn.AutoFit();
                }

                // sử dụng phương thức SaveAs() để lưu workbook với filename
                workbook.SaveAs(fileName);
                //đóng workbook
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            string defaultFileName = "Developer-Export.xlsx"; // Tên file mặc định

            // Khởi tạo SaveFileDialog với tên file mặc định
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel Files|*.xlsx;*.xls";
            saveFileDialog1.Title = "Save Developer-Export";
            saveFileDialog1.FileName = defaultFileName;

            // Nếu người dùng chọn OK
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Gọi hàm ToExcel() với tham số là dtgDSHS và tên file từ SaveFileDialog
                ToExcel(dataGridView1, saveFileDialog1.FileName);
            }
        }
    }
}
