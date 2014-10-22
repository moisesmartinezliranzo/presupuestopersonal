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

namespace PresupuestoPersonal
{
    public partial class Form1 : Form
    {
        DataTable table;
        SqlCommand cmd;

        public Form1()
        {
            table = new DataTable();
            InitializeComponent();
        }        

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AddData();
            ShowData();
            txtConcepto.Text = "";
            txtValor.Text = "";
            cmbTipo.Text = "";
        }       

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowData();          
        }

        private void AddData()
        {
            String insert = "insert into tb_presupuesto_personal(concepto,valor,fecha,tipo)values('{0}',{1},'{2}',{3})";
            insert = string.Format(insert, txtConcepto.Text, txtValor.Text, DateTime.Now.ToString(), cmbTipo.SelectedIndex);

            try
            {
                cmd = new SqlCommand(insert, Conexion.getConexion());

                cmd.ExecuteNonQuery();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }            
        }

        private void ShowData()
        {
            DataTable table = new DataTable();
            cmd = new SqlCommand("select * from vistapresupuesto", Conexion.getConexion());
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(table);
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }

            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Visible = false;
            decimal colGastos = 0M;
            decimal colIngresos = 0;
            decimal poderAdquisitivo = 0M;           

            foreach (DataRow dr in table.Rows)
            {
                if (dr["valor"] != DBNull.Value)
                {
                    if (dr["tipo"].ToString() == "Gastos")
                    {
                        colGastos += decimal.Parse(dr["valor"].ToString());
                    }
                    if (dr["tipo"].ToString() == "Ingresos")
                    {
                        colIngresos += decimal.Parse(dr["valor"].ToString());
                    }
                }                
            }

            poderAdquisitivo = colIngresos - colGastos;

            lblPoderAdquisitivo.Text = poderAdquisitivo.ToString();

            lblTotalIngresos.Text = colIngresos.ToString();
            lblTotalGastos.Text = colGastos.ToString();

            if (poderAdquisitivo < 0)
            {
                lblPoderAdquisitivo.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblPoderAdquisitivo.ForeColor = System.Drawing.Color.Blue;
            }
        }       

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var databaseRecordId = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            FormEdit fEdit = new FormEdit(databaseRecordId.ToString());

            DialogResult result = fEdit.ShowDialog();
            if (result == DialogResult.OK)
            {
                ShowData();
            }
        }
    }
}
