using Microsoft.AspNetCore.Mvc;
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
            ViewBag.Subtotal = producto.CalcularSubtotal();
            ViewBag.Descuento = producto.CalcularDescuento();
            ViewBag.Total = producto.CalcularTotal(producto.CalcularDescuento());
            return View(producto);
        }
    }
}
