using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;


namespace ziveeDB
{
    public partial class Form1 : Form
    {
        private SqlConnection SqlConnection = null;
        private SqlTransaction nrthwndConnection = null;
        public string TestDBConnection { get; private set; }

        public Form1()
        {
            InitializeComponent();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);
            SqlConnection.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Products", SqlConnection);
            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridView2.DataSource = db.Tables[0];

            if (SqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }

            {
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);
                sqlConnection.Open();

                string query = "SELECT C.CategoryName, COUNT(*) AS TotalProducts " +
                               "FROM Products AS P " +
                               "JOIN Categories AS C ON P.CategoryID = C.CategoryID " +
                               "GROUP BY C.CategoryName";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);


                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                
                List<string> categories = new List<string>();
                List<int> productCounts = new List<int>();

                while (dataReader.Read())
                {
                    string categoryName = dataReader["CategoryName"].ToString();
                    int productCount = Convert.ToInt32(dataReader["TotalProducts"]);

                    categories.Add(categoryName);
                    productCounts.Add(productCount);
                }

               
                dataReader.Close();
                sqlConnection.Close();

              
                Series series = new Series("Категории");
                series.ChartType = SeriesChartType.Pie;
                series["PieLabelStyle"] = "Outside"; 
                series["PieLineColor"] = "Black";

              
                for (int i = 0; i < categories.Count; i++)
                {
                    
                    string translatedCategory = TranslateCategory(categories[i]);

                    series.Points.AddXY(translatedCategory, productCounts[i]);
                    series.Points[i].LegendText = translatedCategory;
                    series.Points[i].Label = $"{translatedCategory} ({productCounts[i]})"; 
                }

          
                chart1.Series.Add(series);

              
                chart1.ChartAreas[0].Area3DStyle.Enable3D = true; 
                chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Настройки шрифта для меток оси X
                chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Настройки шрифта для меток оси Y
                chart1.Legends[0].Enabled = true; 
            }
        }

        private string TranslateCategory(string category)
        {
            switch (category)
            {
                case "Beverages":
                    return "Напитки";
                case "Condiments":
                    return "Приправы";
                case "Confections":
                    return "Сладости";
                case "Dairy Products":
                    return "Молочные продукты";
                case "Grains/Cereals":
                    return "Злаки/Крупы";
                case "Meat/Poultry":
                    return "Мясо/Птица";
                case "Produce":
                    return "Овощи/Фрукты";
                case "Seafood":
                    return "Морепродукты";
                default:
                    return category;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
               $"INSERT INTO [Products] (ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, CategoryID) VALUES (@ProductName, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @CategoryID)",
                SqlConnection);

            command.Parameters.AddWithValue("@ProductName", textBox1.Text);
            command.Parameters.AddWithValue("@QuantityPerUnit", textBox2.Text);
            command.Parameters.AddWithValue("@UnitPrice", textBox3.Text);
            command.Parameters.AddWithValue("@UnitsInStock", textBox4.Text);
            command.Parameters.AddWithValue("@CategoryID", textBox5.Text);
          

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Запись успешно добавлена в базу данных.");

                SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Products", SqlConnection);
                DataSet db = new DataSet();
                dataAdapter.Fill(db);
                dataGridView2.DataSource = db.Tables[0];
            }



            else
            {
                MessageBox.Show("Ошибка при добавлении записи в базу данных.");
            }

        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <= 10";
                    break;
                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 10 AND UnitsInStock <= 50";
                    break;
                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 50";
                    break;
                case 3:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            listView4.Items.Clear();

            SqlDataReader dataReader = null;
            List<string[]> rows = new List<string[]>();

            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT P.ProductName, P.QuantityPerUnit, P.UnitPrice, P.UnitsInStock, P.CategoryID, C.CategoryName " +
                                                       "FROM Products AS P " +
                                                       "JOIN Categories AS C ON P.CategoryID = C.CategoryID", SqlConnection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    string[] row = new string[]
                    {
                Convert.ToString(dataReader["ProductName"]),
                Convert.ToString(dataReader["QuantityPerUnit"]),
                Convert.ToString(dataReader["UnitPrice"]),
                Convert.ToString(dataReader["UnitsInStock"]),
                Convert.ToString(dataReader["CategoryID"]),
                TranslateCategory(Convert.ToString(dataReader["CategoryName"]))
                    };

                    rows.Add(row);
                }

                foreach (string[] row in rows)
                {
                    ListViewItem item = new ListViewItem(row);
                    listView4.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }


        }



        
        


        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count > 0)
            {
                string productName = listView4.SelectedItems[0].Text;

                try
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand("DELETE FROM Products WHERE ProductName = @ProductName", connection);
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Запись успешно удалена.");

                        SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Products", SqlConnection);
                        DataSet db = new DataSet();
                        dataAdapter.Fill(db);
                        dataGridView2.DataSource = db.Tables[0];

                        // Обновить список после удаления
                        button4_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");

            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            SqlDataReader dataReader = null;
            List<string[]> rows = new List<string[]>();

            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT CompanyName, ContactName, ContactTitle, Address, PostalCode  FROM Suppliers", SqlConnection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    string[] row = new string[]
                    {
                        Convert.ToString(dataReader["CompanyName"]),
                        Convert.ToString(dataReader["ContactName"]),
                        Convert.ToString(dataReader["ContactTitle"]),
                        Convert.ToString(dataReader["Address"]),
                        Convert.ToString(dataReader["PostalCode"])
                    };

                    rows.Add(row);
                }

                foreach (string[] row in rows)
                {
                    ListViewItem item = new ListViewItem(row);
                    listView2.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {
          
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string categoryInfo = "Номер категории - Название категории\n" +
                          "1 - Напитки\n" +
                          "2 - Приправы\n" +
                          "3 - Сладости\n" +
                          "4 - Молочные продукты\n" +
                          "5 - Злаки/Крупы\n" +
                          "6 - Мясо/Птица\n" +
                          "7 - Овощи/Фрукты\n" +
                          "8 - Морепродукты\n";

            MessageBox.Show(categoryInfo, "Памятка о категориях");
        }
    }



   
}

  







