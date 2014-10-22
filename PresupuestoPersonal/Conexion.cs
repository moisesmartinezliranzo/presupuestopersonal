using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresupuestoPersonal
{
    class Conexion
    {
        public static SqlConnection getConexion() {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(PresupuestoPersonal.Properties.Resources.conexionString);
                conn.Open();
                return conn;
            }
            catch(Exception ex){                
                MessageBox.Show(ex.ToString());
                return null;
            }
        
        }

    }
}
