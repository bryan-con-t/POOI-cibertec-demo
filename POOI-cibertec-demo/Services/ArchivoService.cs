using Aspose.Cells;
using POOI_cibertec_demo.Models;
using System.Text;

namespace POOI_cibertec_demo.Services
{
    public class ArchivoService
    {
        public void GenerarReporteTxt(List<ProductoOferta> productos)
        {
            string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }
            string rutaArchivo = Path.Combine(carpeta, "reporte-compras.txt");
            using (StreamWriter writer = new StreamWriter(rutaArchivo, false, Encoding.UTF8))
            {
                writer.WriteLine("REPORTE DE COMPRAS");
                writer.WriteLine("====================");
                foreach (var producto in productos)
                {
                    writer.WriteLine($"Producto: {producto.Nombre}");
                    writer.WriteLine($"Precio: S/ {producto.Precio:0.00}");
                    writer.WriteLine($"Cantidad: {producto.Cantidad}");
                    writer.WriteLine($"Estado: {producto.Estado}");
                    writer.WriteLine($"Total: S/ {producto.CalcularTotal():0.00}");
                    writer.WriteLine("--------------------");
                }
            }
        }

        public string LeerReporteTxt()
        {
            string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Reportes", "reporte-compras.txt");
            if (!File.Exists(rutaArchivo))
            {
                return "No existe reporte";
            }
            return File.ReadAllText(rutaArchivo);
        }

        public MemoryStream ExportarExcel(List<ProductoOferta> productos)
        {
            Workbook workbook = new Workbook();
            Worksheet hoja = workbook.Worksheets[0];
            hoja.Cells["A1"].PutValue("Producto");
            hoja.Cells["B1"].PutValue("Precio");
            hoja.Cells["C1"].PutValue("Cantidad");
            hoja.Cells["D1"].PutValue("Estado");
            int fila = 2;
            foreach (var producto in productos)
            {
                hoja.Cells[$"A{fila}"].PutValue(producto.Nombre);
                hoja.Cells[$"B{fila}"].PutValue(producto.Precio);
                hoja.Cells[$"C{fila}"].PutValue(producto.Cantidad);
                hoja.Cells[$"D{fila}"].PutValue(producto.Estado);
                fila++;
            }
            MemoryStream stream = new MemoryStream();
            workbook.Save(stream, SaveFormat.Xlsx);
            stream.Position = 0;
            return stream;
        }

        public List<ProductoOferta> LeerExcel(Stream stream)
        {
            List<ProductoOferta> productos = new List<ProductoOferta>();
            Workbook workbook = new Workbook(stream);
            Worksheet hoja = workbook.Worksheets[0];
            Cells cells = hoja.Cells;
            int ultimaFila = cells.MaxDataRow;
            for (int i = 1; i <= ultimaFila; i++)
            {
                ProductoOferta producto = new ProductoOferta();
                producto.Nombre = cells[i, 0].StringValue;
                producto.Precio = Convert.ToDecimal(cells[i, 1].Value);
                producto.Cantidad = Convert.ToInt32(cells[i, 2].Value);
                producto.Categoria = cells[i, 2].StringValue;
                producto.Estado = cells[i, 3].StringValue;
                productos.Add(producto);
            }
            return productos;
        }
    }
}
