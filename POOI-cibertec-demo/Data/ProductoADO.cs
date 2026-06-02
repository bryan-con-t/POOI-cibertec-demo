using Microsoft.Data.SqlClient;
using POOI_cibertec_demo.Models;
using System.Data;

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

        public List<ProductoOferta> Listar()
        {
            List<ProductoOferta> productos = new List<ProductoOferta>();
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                string sql = "sp_listarProductos";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ProductoOferta producto = new ProductoOferta();
                    producto.Nombre = dr["Nombre"].ToString();
                    producto.Precio = Convert.ToDecimal(dr["Precio"]);
                    producto.Cantidad = Convert.ToInt32(dr["Cantidad"]);
                    producto.Categoria = dr["Categoria"].ToString();
                    producto.Estado = dr["Estado"].ToString();
                    producto.Descuento = Convert.ToDecimal(dr["Descuento"]);
                    productos.Add(producto);
                }
            }
            return productos;
        }

        public ProductoOferta Buscar(string nombre)
        {
            ProductoOferta producto = null;
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                string sql = "sp_BuscarProducto";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    producto = new ProductoOferta()
                    {
                        Nombre = dr["Nombre"].ToString(),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Categoria = dr["Categoria"].ToString(),
                        Estado = dr["Estado"].ToString(),
                        Descuento = Convert.ToDecimal(dr["Descuento"])
                    };
                }
            }
            return producto;
        }
    }
}
