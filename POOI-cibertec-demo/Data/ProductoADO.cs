using Microsoft.Data.SqlClient;
using POOI_cibertec_demo.Models;

namespace POOI_cibertec_demo.Data
{
    public class ProductoADO
    {
        private readonly string _cadenaConexion;

        public ProductoADO(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public void Registrar(ProductoOferta producto)
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                string sql = @"INSERT INTO Producto (Nombre, Precio, Cantidad, Categoria, Estado, Descuento) 
                    VALUES (@Nombre, @Precio, @Cantidad, @Categoria, @Estado, @Descuento)";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                cmd.Parameters.AddWithValue("@Estado", producto.Estado);
                cmd.Parameters.AddWithValue("@Descuento", producto.Descuento);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
