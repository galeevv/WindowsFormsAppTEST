using MySql.Data.MySqlClient;
using Org.BouncyCastle.Math;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            /////////////////
            try
            {
                DB db = new DB();

                MySqlCommand command = new MySqlCommand($"UPDATE `Users` SET `Name` = '{nameT.Text}' WHERE UserID = {LocalDB.userID}", db.GetConnection());

                db.openConnection();
                command.ExecuteNonQuery();
                db.closeConnection();

                MessageBox.Show("Данные успешно обновлены в базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных в базу данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ////////////////

            try
            {
                DB db = new DB();

                MySqlCommand command = new MySqlCommand($"INSERT INTO `Flights` (`Departure`, `Arrival`, `DepartureDateTime`, `ArrivalDateTime`, `Price`, `AvailableSeats`, `AircraftType`) " +
                    $"VALUES ('{departureT.Text}', '{arrivalT.Text}', '{departureDate.Value.ToString("yyyy.MM.dd ")}{departureTime.Text}', '{arrivalDate.Value.ToString("yyyy.MM.dd ")}{arrivalTime.Text}', '{priceT.Text}', '{passengerT.Text}', '{type}')", db.GetConnection());

                db.openConnection();
                command.ExecuteNonQuery();
                db.closeConnection();

                MessageBox.Show("Данные успешно обновлены в базе данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных в базу данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /////////////////////

            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DB db = new DB();

                MySqlCommand command = new MySqlCommand($"DELETE FROM Flights WHERE FlightID = '{flightID.Text}'", db.GetConnection());
                db.openConnection();
                command.ExecuteNonQuery();
                db.closeConnection();

                Admin.Instance.getFlights();
            }

            ///////////////////////
        }
    }
}
