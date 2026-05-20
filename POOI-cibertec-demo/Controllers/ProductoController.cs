using Microsoft.AspNetCore.Mvc;
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
            } catch (PrecioInvalidoException ex)
            {
                ViewBag.Error = ex.Message;
            } catch (CantidadInvalidaException ex)
            {
                ViewBag.Error = ex.Message;
            } catch (DescuentoInvalidoException ex)
            {
                ViewBag.Error = ex.Message;
            } catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado.";
            }
            return View(producto);
        }
    }
}
