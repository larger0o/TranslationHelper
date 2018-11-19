using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace TranslationHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int contadorfichero = 3;
        public string ruta = null;
        public bool guardado = true;
        int max = 0;
        int EOF = 0;

        public void CreateTextBox(string[] info)
        {

            max = info.Length;
            EOF = info.Length;

            for (int x = 0; x < info.Length; x++)
            {
                if (info[x].Contains("; choice 0"))
                {
                    max = x - 1;
                    break;
                }
            }

            int n = max;
            TextBox[] textBoxENG = new TextBox[n+1];
            TextBox[] textBoxESP = new TextBox[n+1];

            TextBox choiceENG = null;
            TextBox choiceESP = null;

            Label[] NameENG = new Label[n+1];
            Label[] NameESP = new Label[n+1];

            Label[] SizeENG = new Label[n+1];
            Label[] SizeESP = new Label[n+1];


            for (int i = 3; i < n; i++)
            {

                textBoxENG[i] = new TextBox();
                textBoxESP[i] = new TextBox();
                
                //473,26
                textBoxENG[i].Size = new System.Drawing.Size(573, 26);
                textBoxENG[i].Font = new Font(textBoxENG[i].Font.FontFamily, 10);

                textBoxESP[i].Size = new System.Drawing.Size(573, 26);
                textBoxESP[i].Font = new Font(textBoxESP[i].Font.FontFamily, 10);

                textBoxENG[i].Name = "n" + i + "ENG";
                //textBoxENG[i].Text = info[i].Split(']')[1].TrimEnd('✓');

                //calculamos la primera ocurrencia de ] (inicio de diálogo) y sólo lo dividimos UNA vez
                int pos = info[i].IndexOf(']');
                textBoxENG[i].Text = info[i].Substring(++pos).TrimEnd('✓');


                /*if (info[i].Split(']').Length > 2)
                {
                    for (int j = 2; j < info[i].Split(']').Length; j++)
                        textBoxENG[i].Text += info[i].Split(']')[j]+"]".TrimEnd('✓');

                }*/
                textBoxENG[i].Enabled = false;
                if (textBoxENG[i].Text.Length>90)
                    textBoxENG[i].Font = new Font(textBoxENG[i].Font.FontFamily, 7);

                textBoxESP[i].Name = "n" + i + "ESP";
                textBoxESP[i].Text = "";
                textBoxESP[i].Enabled = true;

                textBoxESP[i].TextChanged += textBoxESP_TextChanged;

                choiceENG = new TextBox();
                choiceESP = new TextBox();

                choiceENG.Name = "choiceENG";
                choiceESP.Name = "choiceESP";
                choiceENG.Multiline = true;
                choiceENG.Size = new System.Drawing.Size(573, 286);
                choiceENG.Font = new Font(choiceENG.Font.FontFamily, 10);

                for (int q = max+1; q < EOF; q++)
                {
                    choiceENG.Text += info[q] + "\r\n";
                    choiceESP.Text += info[q] + "\r\n";
                }

                choiceESP.Multiline = true;
                choiceESP.Size = new System.Drawing.Size(573, 286);
                choiceESP.Font = new Font(choiceESP.Font.FontFamily, 10);
                choiceENG.Enabled = false;
                choiceESP.Enabled = true;


                NameENG[i] = new Label();
                NameESP[i] = new Label();


                NameENG[i].Font = new Font(NameENG[i].Font.FontFamily, 12);
                NameENG[i].Height = 32;
                NameENG[i].Padding = new Padding(0, 6, 0, 6);

                NameESP[i].Font = new Font(NameESP[i].Font.FontFamily, 12);
                NameESP[i].Height = 32;
                NameESP[i].Padding = new Padding(0, 6, 0, 6);

                NameENG[i].Name = "l" + i + "ENG";
                NameENG[i].Text = info[i].Split(']')[0].Substring(1);

                NameESP[i].Name = "l" + i + "ESP";
                NameESP[i].Text = info[i].Split(']')[0].Substring(1);


                SizeENG[i] = new Label();
                SizeESP[i] = new Label();

                SizeENG[i].Font = new Font(SizeENG[i].Font.FontFamily, 12);
                SizeENG[i].Height = 32;
                SizeENG[i].Padding = new Padding(0, 6, 0, 6);

                SizeESP[i].Font = new Font(SizeENG[i].Font.FontFamily, 12);
                SizeESP[i].Height = 32;
                SizeESP[i].Padding = new Padding(0, 6, 0, 6);


                SizeENG[i].Name = "Size" + i+ "ENG";


                SizeENG[i].Text = info[i].Substring(++pos).TrimEnd('✓').Length.ToString();
                /*SizeENG[i].Text = info[i].Split(']')[1].TrimEnd('✓').Length.ToString();
                if (info[i].Split(']').Length > 2)
                {
                    for (int j = 1; j < info[i].Split(']').Length; j++)
                    {
                        int total = 0;
                        total += info[i].Split(']')[j].TrimEnd('✓').Length;
                        SizeENG[i].Text = total.ToString();
                    }

                }*/

                SizeESP[i].Name = "Size" + i + "ESP";
                SizeESP[i].Text = "0";


            }

            for (int i = 3; i < n; i++)
            {
                Panel.Controls.Add(NameENG[i]);
                Panel.Controls.Add(textBoxENG[i]);
                Panel.Controls.Add(SizeENG[i]);

                Panel.Controls.Add(NameESP[i]);
                Panel.Controls.Add(textBoxESP[i]);
                Panel.Controls.Add(SizeESP[i]);            
            }
            Panel.Controls.Add(choiceENG);
            Panel.Controls.Add(choiceESP);
            guardado = false;


        }


        void textBoxESP_TextChanged(object sender, EventArgs e)
        {
            TextBox txtbx = (TextBox)sender;
            Label lblsz = (Label)Panel.Controls["Size"+ txtbx.Name.Substring(1)];
            Label lblsz1 = (Label)Panel.Controls["Size" + txtbx.Name.Substring(1).TrimEnd('E','S','P')+"ENG"];
            int longitud = txtbx.Text.Length;
            if (longitud > int.Parse(lblsz1.Text))
                lblsz.Font = new Font(lblsz.Font, FontStyle.Bold);
            else
                lblsz.Font = new Font(lblsz.Font, FontStyle.Regular);
            lblsz.Text = longitud.ToString();
        }

        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            ruta = CargarFicheros();
            cargarArchivoSeleccionadoToolStripMenuItem.Enabled=true;
            anteriorToolStripMenuItem.Enabled = true;
            siguienteToolStripMenuItem.Enabled = true;
            guardarToolStripMenuItem.Enabled = true;

        }
        public string CargarFicheros()
        {
            string ruta = null;
            if (File.Exists("MKconfig"))
                ruta = File.ReadAllText("MKconfig").Split(';')[0];
            else if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ruta = folderBrowserDialog1.SelectedPath;
                File.WriteAllText("MKconfig", ruta+";0");
            }

            toolStripComboBox1.Sorted = false;
            for (int i = 0; i < 981; i++)
                toolStripComboBox1.Items.Add(i + ".txt");

            if (File.Exists("MKconfig"))
                toolStripComboBox1.SelectedIndex = int.Parse(File.ReadAllText("MKconfig").Split(';')[1]);
            else
                toolStripComboBox1.SelectedIndex = 0;
            return ruta;
        }

        public string[] CargarInfo()
        {

            //string[] origen = File.ReadAllLines(ruta+"/"+contadorfichero+".txt");
            try
            {
                string[] origen = File.ReadAllLines(ruta + "/" + toolStripComboBox1.SelectedItem);
                return origen;
            }
            catch
            {
                MessageBox.Show("Error al cargar el fichero");
                return null;
            }
            

            //origen[0] => ; Event\(fichero).evd
            //origen[1] VACÍO
            //origen[2] => ; script
            //origen[origen.length-1] => ; end of file
            
        }

        private void cargarArchivoSeleccionadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!guardado)
            {
                DialogResult result = MessageBox.Show("No se han guardado los cambios, ¿continuar?", "Translation Helper", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;

            }
            Panel.Controls.Clear();
            CreateTextBox(CargarInfo());

        }

        private void siguienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex < toolStripComboBox1.Items.Count - 1)
            {
                if (!guardado)
                {
                    DialogResult result = MessageBox.Show("No se han guardado los cambios, ¿continuar?", "Translation Helper", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                        return;
                }
                    anteriorToolStripMenuItem.Enabled = true;
                toolStripComboBox1.SelectedIndex += 1;
                Panel.Controls.Clear();
                CreateTextBox(CargarInfo());
            }
            else
            {
                siguienteToolStripMenuItem.Enabled = false;
            }
        }

        private void anteriorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex > 0)
            {
                if (!guardado)
                {
                    DialogResult result = MessageBox.Show("No se han guardado los cambios, ¿continuar?", "Translation Helper", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                        return;
                }
                    siguienteToolStripMenuItem.Enabled = true;
                toolStripComboBox1.SelectedIndex -= 1;
                Panel.Controls.Clear();
                CreateTextBox(CargarInfo());
            }
            else
            {
                anteriorToolStripMenuItem.Enabled = false;
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarInfo();

        }

        public void GuardarInfo()
        {
            string[] info = new string[EOF];
            string infoc = null;
            info[0] = "; Event\\" + toolStripComboBox1.SelectedItem.ToString().TrimEnd('t', 'x', 't') + "evd\r\n";
            info[1] = "\r\n";
            info[2] = "; script\r\n";
            TextBox txtbxESP;
            Label NamESP;
            for (int i=3; i < max; i++)
            {
                txtbxESP = (TextBox)Panel.Controls["n" + i + "ESP"];
                NamESP = (Label)Panel.Controls["l" + i + "ESP"];
                info[i] = "[" + NamESP.Text + "]" + txtbxESP.Text + "✓\r\n";
            }
            txtbxESP = (TextBox)Panel.Controls["choiceESP"];
            info[max] = "\r\n"+txtbxESP.Text;

            if (!Directory.Exists(ruta+"\\hechos"))
                Directory.CreateDirectory(ruta + "\\hechos");
            for (int i = 0; i < info.Length; i++)
            {
                infoc += info[i];
            }
            try
            {

                File.WriteAllText(ruta + "\\hechos\\" + toolStripComboBox1.SelectedItem.ToString(), infoc,Encoding.Unicode);
                MessageBox.Show("Guardado");
                
                File.WriteAllText("MKconfig", ruta+";"+toolStripComboBox1.SelectedIndex);


                guardado = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al guardar");
            }


        }

        private void toolStripComboBox1_DropDownClosed(object sender, EventArgs e)
        {
            if (!guardado)
            {
                DialogResult result = MessageBox.Show("No se han guardado los cambios, ¿continuar?", "Translation Helper", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;

            }
            Panel.Controls.Clear();
            CreateTextBox(CargarInfo());
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (ruta != null)
            {
                if (!guardado)
                {
                    DialogResult result = MessageBox.Show("No se han guardado los cambios, ¿continuar?", "Translation Helper", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                        return;

                }
                Panel.Controls.Clear();
                CreateTextBox(CargarInfo());
            }*/
        }
    }
}
