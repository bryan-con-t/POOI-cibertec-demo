using Microsoft.AspNetCore.Mvc;
using POOI_cibertec_demo.Data;
using POOI_cibertec_demo.Exceptions;
using POOI_cibertec_demo.Models;

namespace POOI_cibertec_demo.Controllers
{
    public class ProductoController : Controller
    {
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
                } else
                {
                    producto.MarcarComoPendiente();
                }
                var productoExistente = ListaComprasData.Productos.FirstOrDefault(p => p.Nombre == producto.Nombre);
                if (productoExistente == null)
                {
                    ListaComprasData.Productos.Add(producto);
                } else
                {
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Cantidad = producto.Cantidad;
                    productoExistente.Categoria = producto.Categoria;
                    productoExistente.Estado = producto.Estado;
                    productoExistente.Descuento = producto.Descuento;
                    productoExistente.PrecioCambio = producto.PrecioCambio;
                }
                return RedirectToAction("Lista");
            } catch (PrecioInvalidoException ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            } catch (CantidadInvalidaException ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            } catch (DescuentoInvalidoException ex)
            {
                ViewBag.Error = ex.Message;
                return View(producto);
            } catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado.";
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
    }
}
