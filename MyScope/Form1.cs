using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyScope
{
    public partial class Form1 : Form
    {
        private AppData appData;

        public Form1()
        {
            InitializeComponent();

            appData = new AppData();
            loadData();

            dataGridViewTasks.DataSource = appData.aufgaben;
            dataGridViewTasks.Columns[0].HeaderText = "Name";
            dataGridViewTasks.Columns[0].Width = 612;
            dataGridViewTasks.Columns[1].Visible = false;
            dataGridViewTasks.Columns[2].Visible = false;
            dataGridViewTasks.Columns[3].HeaderText = "Geplantes Ende";
            dataGridViewTasks.Columns[3].Width = 250;
            dataGridViewTasks.Columns[4].Visible = false;
            dataGridViewTasks.Columns[5].Visible = false;
            dataGridViewTasks.Columns.Add(new DataGridViewImageColumn(true));
            dataGridViewTasks.Columns[6].HeaderText = "Status";
            dataGridViewTasks.Columns[6].Width = 100;
            // Hier fehlt noch der Image ColumnType
        }

        private void loadData()
        {
            //Daten einlesen aus Datei "udata.dat"
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream("udata.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                appData = (AppData)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message, "Dateifehler", MessageBoxButtons.OK);
                //throw;
            }
        }

        private void SaveData()
        {
            FileStream fs = new FileStream("udata.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, appData);
            }
            catch (SerializationException ec)
            {
                MessageBox.Show(ec.Message, "Speicherfehler", MessageBoxButtons.OK);
                //Console.WriteLine("Failed to serialize. Reason: " + ec.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            appData.aufgaben.Add(new Aufgabe("Testaufgabe 1", "Dies ist die erste Testaufgabe", DateTime.Now,
                DateTime.Now, false));
            appData.aufgaben.Add(new Aufgabe("Testaufgabe 2", "Dies ist die zweite Testaufgabe", DateTime.Now,
                DateTime.Now, false));
        }
    }
}
