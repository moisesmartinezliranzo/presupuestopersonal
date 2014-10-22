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
    public partial class FormEdit : Form
    {
        SqlCommand cmd;
        private string idOfRowSelected;
        public FormEdit(string idOfRowSelected)
        {
            InitializeComponent();

            this.idOfRowSelected = idOfRowSelected;
        }

        private void FormEdit_Load(object sender, EventArgs e)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand("select * from ViewEditPPSelected where id = '" + idOfRowSelected + "'", Conexion.getConexion());

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                txtConcepto.Text = (reader["concepto"].ToString());
                cmbTipo.Text = (reader["tipo"].ToString());
                txtValor.Text = (reader["valor"].ToString());
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatePP();
        }

        private void UpdatePP()
        {
            string comUpdate = "update tb_presupuesto_personal set concepto='{0}', tipo={1},valor={2} where id = {3}";
            comUpdate = string.Format(comUpdate, txtConcepto.Text, cmbTipo.SelectedIndex, txtValor.Text, idOfRowSelected);

            try
            {
                cmd = new SqlCommand(comUpdate, Conexion.getConexion());
                cmd.ExecuteNonQuery();
                if(cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                this.DialogResult = DialogResult.OK;
             

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void DeletePP()
        {
            string cmdDelete = "delete from tb_presupuesto_personal where id = {0}";
            cmdDelete = string.Format(cmdDelete,idOfRowSelected.ToString());            

            try 
            {
                cmd = new SqlCommand(cmdDelete, Conexion.getConexion());
                cmd.ExecuteNonQuery();
                if(cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                MessageBox.Show("Registro eliminado", "Eliminado",MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void btnDeleteRegister_Click(object sender, EventArgs e)
        {
            DeletePP();            
        }
    }
}
