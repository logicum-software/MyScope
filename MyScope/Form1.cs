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
        internal AppData appData;

        public Form1()
        {
            InitializeComponent();

            appData = new AppData();
            loadData();

            //Initialize DataGridViewTasks
            dataGridViewTasks.DataSource = appData.aufgaben;
            dataGridViewTasks.Columns[0].HeaderText = "Name";
            dataGridViewTasks.Columns[0].Width = 612;
            dataGridViewTasks.Columns[1].Visible = false;
            dataGridViewTasks.Columns[2].Visible = false;
            dataGridViewTasks.Columns[3].HeaderText = "Geplanter Abschluss";
            dataGridViewTasks.Columns[3].Width = 250;
            dataGridViewTasks.Columns[4].Visible = false;
            dataGridViewTasks.Columns[5].Visible = false;
            dataGridViewTasks.Columns[6].Visible = false;
            dataGridViewTasks.Columns.Add(new DataGridViewImageColumn(true));
            dataGridViewTasks.Columns[7].HeaderText = "Status";
            dataGridViewTasks.Columns[7].Width = 100;
            // Hier fehlt noch der Image ColumnType

            //Initialize DataGridView Projects
            dataGridViewProjects.DataSource = appData.projekte;
            dataGridViewProjects.Columns[0].HeaderText = "Name";
            dataGridViewProjects.Columns[0].Width = 612;
            dataGridViewProjects.Columns[1].Visible = false;
            dataGridViewProjects.Columns[2].Visible = false;
            dataGridViewProjects.Columns[3].HeaderText = "Geplanter Abschluss";
            dataGridViewProjects.Columns[3].Width = 250;
            dataGridViewProjects.Columns[4].Visible = false;
            dataGridViewProjects.Columns[5].Visible = false;
            dataGridViewProjects.Columns.Add(new DataGridViewImageColumn(true));
            dataGridViewProjects.Columns[6].HeaderText = "Status";
            dataGridViewProjects.Columns[6].Width = 100;
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

        private void saveData()
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

        private void sortLists()
        {
            // Sort Lists ascending to dtPlannedEnd
            List<Aufgabe> tmpListTasks = appData.aufgaben.OrderBy(x => x.dtPlannedEnd).ToList();
            appData.aufgaben = new BindingList<Aufgabe>(tmpListTasks);

            List<Projekt> tmpListProjects = appData.projekte.OrderBy(x => x.dtPlannedEnd).ToList();
            appData.projekte = new BindingList<Projekt>(tmpListProjects);

            // Set new DataSources
            dataGridViewTasks.DataSource = appData.aufgaben;
            dataGridViewProjects.DataSource = appData.projekte;
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

        private void buttonNewTask_Click(object sender, EventArgs e)
        {
            NewTask dlgNewTask = new NewTask();
            dlgNewTask.tmpAufgaben = new BindingList<Aufgabe>(appData.aufgaben);
            dlgNewTask.ShowDialog();

            if (dlgNewTask.DialogResult == DialogResult.OK)
            {
                if (dlgNewTask.bOverride)
                {
                    foreach (Aufgabe item in appData.aufgaben)
                    {
                        if (item.strName.Equals(dlgNewTask.textBoxName.Text))
                        {
                            item.strDescription = dlgNewTask.textBoxDescription.Text;
                            item.dtPlannedStart = dlgNewTask.dateTimePickerStart.Value;
                            item.dtPlannedEnd = dlgNewTask.dateTimePickerEnd.Value;
                            if (dlgNewTask.checkBoxStart.Checked)
                                item.iStatus = 1;
                            else
                                item.iStatus = 0;
                        }
                    }

                }
                else
                {
                    appData.aufgaben.Add(new Aufgabe(dlgNewTask.textBoxName.Text, dlgNewTask.textBoxDescription.Text,
                        dlgNewTask.dateTimePickerStart.Value, dlgNewTask.dateTimePickerEnd.Value,
                        (Boolean)dlgNewTask.checkBoxStart.Checked));
                }
                
                sortLists();
                saveData();
                loadData();
                MessageBox.Show("Die Aufgabe wurde gespeichert", "Aufgabe gespeichert", MessageBoxButtons.OK);
            }
        }

        private void buttonDelTask_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Möchten Sie die ausgewählte Aufgabe wirklich dauerhaft löschen?", 
                "Aufgabe löschen", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                appData.aufgaben.RemoveAt(dataGridViewTasks.CurrentRow.Index);
                sortLists();
                saveData();
                loadData();
                MessageBox.Show("Die ausgewählte Aufgabe wurde gelöscht.", "Aufgabe gelöscht", 
                    MessageBoxButtons.OK);
            }
        }

        private void buttonNewProject_Click(object sender, EventArgs e)
        {

        }
    }
}
