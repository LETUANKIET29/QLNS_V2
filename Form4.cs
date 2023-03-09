using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class Form4 : Form
    {
        SqlConnection conn = ConnectionHelper.GetConnection();
        public Form4()
        {
            InitializeComponent();
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {
            // Lấy tên tệp tin từ đường dẫn và hiển thị lên tiêu đề của form
            this.Text = "Form nhập Developers từ Excel - " + Path.GetFileName(txtFilePath.Text);

            // Kiểm tra xem tệp tin đã chọn có tồn tại hay không
            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Lỗi: Tệp tin không tồn tại.");
                return;
            }

            // Đọc dữ liệu từ file Excel
            using (var stream = File.Open(txtFilePath.Text, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Đọc dữ liệu từ file Excel vào DataSet
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    // Lấy bảng dữ liệu từ DataSet
                    DataTable dataTable = result.Tables[0];

                    // Tạo một DataTable mới để lưu trữ dữ liệu
                    DataTable newDataTable = new DataTable();

                    // Thêm các cột vào DataTable mới
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        newDataTable.Columns.Add(column.ColumnName, column.DataType);
                    }

                    // Thêm các dòng vào DataTable mới, bỏ qua dòng đầu tiên (chứa tiêu đề cột)
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow row = dataTable.Rows[i];
                        newDataTable.Rows.Add(row.ItemArray);
                    }

                    // Hiển thị dữ liệu trong DataTable mới trên DataGridView
                    dataGridView1.DataSource = newDataTable;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                // Kiểm tra có insert được không
                bool check = false;

                // Lặp qua các dòng trong DataGridView và thêm dữ liệu vào CSDL
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    // Lấy dữ liệu từ DataGridView
                    String name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    String gender = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    String birthday = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    String phone = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    String email = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    String cccd = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    String address = dataGridView1.Rows[i].Cells[6].Value.ToString();
                    String status = dataGridView1.Rows[i].Cells[7].Value.ToString();
                    String certificate = dataGridView1.Rows[i].Cells[8].Value.ToString();

                    int _gender;
                    int _status;
                    if (gender.Equals("Nam"))
                        _gender = 0;
                    else
                        _gender = 1;

                    if (status.Equals("Đang làm việc"))
                        _status = 1;
                    else
                        _status = 0;

                    // Tạo câu lệnh SQL để kiểm tra xem cccd đã tồn tại hay chưa
                    string checkExistSql = "SELECT COUNT(*) FROM [dbo].[Developer] WHERE [CitizenID] = @CCCD";
                    SqlCommand checkExistCmd = new SqlCommand(checkExistSql, conn);
                    checkExistCmd.Parameters.AddWithValue("@CCCD", cccd);
                    int count = (int)checkExistCmd.ExecuteScalar();

                    // Nếu cccd đã tồn tại thì bỏ qua và chuyển sang dòng tiếp theo
                    if (count > 0)
                    {
                        continue;
                    }

                    check = true;
                    // Tạo câu lệnh SQL với tham số
                    string sql = "INSERT INTO [dbo].[Developer] ([Name], [Gender], [Birthday], [Phone], [Email], [CitizenID], [Address], [Status]) " +
                                 "VALUES (@Name, @Gender, @Birthday, @Phone, @Email, @CCCD, @Address, @Status) " +
                                 "DECLARE @DeveloperID int = SCOPE_IDENTITY() " +
                                 "INSERT INTO certificate (DeveloperID, [certificateDetailsName]) " +
                                 "VALUES (@DeveloperID, @Certificate)";

                    // Tạo đối tượng SqlCommand và thiết lập giá trị tham số
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Gender", _gender);
                    command.Parameters.AddWithValue("@Birthday", birthday);
                    command.Parameters.AddWithValue("@CCCD", cccd);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Status", _status);
                    command.Parameters.AddWithValue("@Certificate", certificate);

                    // Thực thi câu lệnh SQL
                    command.ExecuteNonQuery();
                }
                if (check)
                    MessageBox.Show("Đã thêm thành công!!");
                else
                    throw new Exception("Đã tồn tại dữ liệu, vui lòng kiểm tra lại sau!!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                FormAdmin f1 = new FormAdmin();
                f1.loadData();
                conn.Close();
                this.Close();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Thiết lập thư mục mặc định
            openFileDialog.InitialDirectory = @"C:\Users\tuank\OneDrive\Desktop\QLNS_v2";

            // Thêm bộ lọc vào Filter để chỉ hiển thị các tệp tin Excel
            // All Excel file format
            openFileDialog.Filter = "Excel files (*.xlsx;*.xlsm;*.xlsb;*.xls)|*.xlsx;*.xlsm;*.xlsb;*.xls|All files (*.*)|*.*";

            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Lấy đường dẫn đầy đủ của tệp tin đã chọn
                    string filePath = openFileDialog.FileName;

                    // Hiển thị đường dẫn tệp tin lên TextBox
                    txtFilePath.Text = filePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: Không thể mở tệp tin. Chi tiết: " + ex.Message);
                }
            }
        }
    }
}
