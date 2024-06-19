using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Authorization
    {
        private BackgroundWorker worker, worker2;
        public string role, reg;

        public Authorization()
        {
            InitializeComponent();
            // Инициализация BackgroundWorker
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            // Инициализация BackgroundWorker2
            worker2 = new BackgroundWorker();
            worker2.WorkerReportsProgress = true;
            worker2.DoWork += Worker2_DoWork;
            worker2.RunWorkerCompleted += Worker2_RunWorkerCompleted;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (loginT.Text == "")
            {
                loginT.BorderColor = Color.Firebrick;
                return;
            }

            loginT.BorderColor = Color.FromArgb(213, 218, 223);

            if (passT.Text == "")
            {
                loginT.BorderColor = Color.Firebrick;
                return;
            }

            passT.BorderColor = Color.FromArgb(213, 218, 223);

            if (!worker.IsBusy)
            {
                // Запускаем BackgroundWorker
                worker.RunWorkerAsync();
            }

            guna2ProgressIndicator1.Visible = true;  // включение прогресс бара
            guna2ProgressIndicator1.AutoStart = true;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Этот метод выполняется во втором потоке
            // Здесь выполняем длительную операцию, например, обработку данных

            DB db = new DB();

            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            db.openConnection();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Login = '{loginT.Text}' AND Password = '{passT.Text}'", db.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();

            if (dt.Rows.Count > 0)
            {
                role = reader["Role"].ToString();
                LocalDB.userID = reader["UserID"].ToString();
                LocalDB.userName = reader["Name"].ToString();
                LocalDB.userLogin = reader["Login"].ToString();
                LocalDB.userPass = reader["Password"].ToString();
            }

            reader.Close();
            db.closeConnection();

            // выключение прогресс бара

            guna2ProgressIndicator1.Visible = false;
            guna2ProgressIndicator1.AutoStart = false;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (role == "user")
            {
                LocalDB.userMode = "user";
                Form form = new Shop();
                form.Show();
                this.Hide();
            }
            else if (role == "admin")
            {
                LocalDB.userMode = "admin";
                Form form = new Admin();
                form.Show();
                this.Hide();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (nameT.Text == "")
            {
                nameT.BorderColor = Color.Firebrick;
                return;
            }
            nameT.BorderColor = Color.FromArgb(213, 218, 223);

            if (loginT2.Text == "")
            {
                loginT2.BorderColor = Color.Firebrick;
                return;
            }
            loginT2.BorderColor = Color.FromArgb(213, 218, 223);

            if (passT2.Text == "")
            {
                loginT2.BorderColor = Color.Firebrick;
                return;
            }
            passT2.BorderColor = Color.FromArgb(213, 218, 223);

            if (!worker.IsBusy)
            {
                // Запускаем BackgroundWorker
                worker2.RunWorkerAsync();
            }

            guna2ProgressIndicator1.Visible = true;  // включение прогресс бара
            guna2ProgressIndicator1.AutoStart = true;
        }

        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            // Этот метод выполняется во втором потоке
            // Здесь выполняем длительную операцию, например, обработку данных

            DB db = new DB();

            if (checkUser()) // проверка (есть ли такой же логин)
            {
                return;
            }

            MySqlCommand command = new MySqlCommand($"INSERT INTO `Users` (`Name`, `Login`, `Password`) " +
                $"VALUES ('{nameT.Text}', '{loginT2.Text}', '{passT2.Text}')", db.GetConnection());

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                reg = "Регистрация прошла успешно!";
            }
            else
            {
                reg = "Регистрация не удалась!";
            }

            db.closeConnection();

            // выключение прогресс бара

            guna2ProgressIndicator1.Visible = false;
            guna2ProgressIndicator1.AutoStart = false;
        }

        private void Worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(reg);
        }

        public Boolean checkUser() // проверка (есть ли такой же логин)
        {
            DB db = new DB();

            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE login='{loginT2.Text}'", db.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                reg = "Такой логин уже есть!";
                guna2ProgressIndicator1.Visible = false;
                guna2ProgressIndicator1.AutoStart = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
