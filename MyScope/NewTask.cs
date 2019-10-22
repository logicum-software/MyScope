using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyScope
{
    public partial class NewTask : Form
    {
        internal BindingList<Aufgabe> tmpAufgaben;
        internal Boolean bOverride;

        public NewTask()
        {
            InitializeComponent();

            bOverride = false;
            textBoxName.SelectAll();
            textBoxName.Focus();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            foreach (Aufgabe item in tmpAufgaben)
            {
                if (item.strName.Equals(textBoxName.Text))
                {
                    if (MessageBox.Show("Eine Aufgabe mit dem Namen\n\n" + textBoxName.Text +
                        "\n\nexistiert bereits. Möchten Sie die vorhandene Aufgabe überschreiben?", "Aufgabe vorhanden",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bOverride = true;
                    }
                    else
                    {
                        textBoxName.SelectAll();
                        textBoxName.Focus();
                        return;
                    }
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
    }
}
