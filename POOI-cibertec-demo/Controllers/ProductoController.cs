using Microsoft.AspNetCore.Mvc;
using POOI_cibertec_demo.Data;
using POOI_cibertec_demo.Exceptions;
using POOI_cibertec_demo.Models;
using POOI_cibertec_demo.Services;
using System.Threading.Tasks;

namespace POOI_cibertec_demo.Controllers
{
    public class ProductoController : Controller
    {
        private readonly CompraService _compraService;
        private readonly JsonService _jsonService;
        private readonly ArchivoService _archivoService;
        private readonly ProductoADO _productoADO;

        public ProductoController(IConfiguration configuration)
        {
            _compraService = new CompraService();
            _jsonService = new JsonService();
            _archivoService = new ArchivoService();
            string cadenaConexion = configuration.GetConnectionString("CibertecDemoConnection");
            _productoADO = new ProductoADO(cadenaConexion);
        }
        public IActionResult Registro()
        {
            Producto producto = new Producto();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Registro(Producto producto)
        {
            return RedirectToAction("Detalle", new
            {
                nombre = producto.Nombre,
                precio = producto.Precio,
                cantidad = producto.Cantidad,
                categoria = producto.Categoria
            });
        }

        public IActionResult Detalle(
            string nombre,
            decimal precio,
            int cantidad,
            string categoria)
        {
            ProductoOferta producto = new ProductoOferta()
            {
                Nombre = nombre,
                Precio = precio,
                Cantidad = cantidad,
                Categoria = categoria
            };
            return View(producto);
        }

        [HttpPost]
        public IActionResult Detalle(ProductoOferta producto)
        {
            ViewBag.Subtotal = 0;
            ViewBag.Total = 0;
            try
            {
                ViewBag.Subtotal = producto.CalcularSubtotal();
                ViewBag.Total = producto.CalcularTotal();
                if (producto.Estado == "Comprado")
                {
                    producto.MarcarComoComprado();
                }
                else
                {
                    producto.MarcarComoPendiente();
                }
                var productoExistente = ListaComprasData.Productos.FirstOrDefault(p => p.Nombre == producto.Nombre);
                if (productoExistente == null)
                {
                    ListaComprasData.Productos.Add(producto);
                    _productoADO.Registrar(producto);
                }
                else
                {
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Cantidad = producto.Cantidad;
                    productoExistente.Categoria = producto.Categoria;
                    productoExistente.Estado = producto.Estado;
                    productoExistente.Descuento = producto.Descuento;
                    productoExistente.PrecioCambio = producto.PrecioCambio;
                }
                return RedirectToAction("Lista");
            }
            catch (PrecioInvalidoException ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            }
            catch (CantidadInvalidaException ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            }
            catch (DescuentoInvalidoException ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            }
        }

        public IActionResult Lista()
        {
            return View(ListaComprasData.Productos);
        }

        public IActionResult Eliminar(string nombre)
        {
            var producto = ListaComprasData.Productos.FirstOrDefault(p => p.Nombre == nombre);
            if (producto != null)
            {
                ListaComprasData.Productos.Remove(producto);
            }
            return RedirectToAction("Lista");
        }

        public IActionResult Comprados()
        {
            var productosComprados = ListaComprasData.Productos.Where(p => p.Estado == "Comprado").ToList();
            return View(productosComprados);
        }

        public IActionResult PrimerProducto()
        {
            var producto = ListaComprasData.Productos.FirstOrDefault();
            return View(producto);
        }

        public IActionResult Nombres()
        {
            var nombres = ListaComprasData.Productos.Select(p => p.Nombre).ToList();
            return View(nombres);
        }

        public IActionResult Editar(string nombre)
        {
            var producto = ListaComprasData.Productos.FirstOrDefault(p => p.Nombre == nombre);
            if (producto == null)
            {
                return RedirectToAction("Lista");
            }
            return View("Detalle", producto);
        }

        public async Task<IActionResult> Guardar()
        {
            ViewBag.Mensaje = await _compraService.GuardarCompraAsync();
            return View();
        }

        public async Task<IActionResult> Historial()
        {
            var productos = await _compraService.ObtenerHistorialAsync();
            return View(productos);
        }

        public async Task<IActionResult> ProcesarCompra()
        {
            TempData["Mensaje"] = await _compraService.ProcesarCompraAsync();
            ListaComprasData.Productos.Clear();
            return RedirectToAction("Lista");
        }

        public IActionResult ExportarJson()
        {
            string json = _jsonService.ConvertirAJson(ListaComprasData.Productos);
            ViewBag.Json = json;
            return View();
        }

        public IActionResult GuardarHistorial()
        {
            _jsonService.GuardarJson(ListaComprasData.Productos);
            TempData["Mensaje"] = "Historial guardado en JSON.";
            return RedirectToAction("Lista");
        }

        public IActionResult CargarHistorial()
        {
            ListaComprasData.Productos = _jsonService.LeerArchivoJson();
            TempData["Mensaje"] = "Historial cargado.";
            return RedirectToAction("Lista");
        }

        public IActionResult GenerarReporte()
        {
            _archivoService.GenerarReporteTxt(ListaComprasData.Productos);
            TempData["Mensaje"] = "Reporte generado en formato .txt.";
            return RedirectToAction("Lista");
        }

        public IActionResult VerReporte()
        {
            ViewBag.Contenido = _archivoService.LeerReporteTxt();
            return View();
        }

        public IActionResult ExportarExcel()
        {
            var archivoExcel = _archivoService.ExportarExcel(ListaComprasData.Productos);
            return File(archivoExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte-compras.xlsx");
        }

        public IActionResult ImportarExcel(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                TempData["Mensaje"] = "Seleccione un archivo.";
                return RedirectToAction("Lista");
            }
            using (var stream = archivo.OpenReadStream())
            {
                var productos = _archivoService.LeerExcel(stream);
                ListaComprasData.Productos.AddRange(productos);
                TempData["Mensaje"] = "Productos importados correctamente desde Excel.";
                return RedirectToAction("Lista");
            }
        }
    }
}
