using POOI_cibertec_demo.Interfaces;

namespace POOI_cibertec_demo.Models
{
    public class ProductoOferta : Producto, IPrecioActualizable
    {
        public decimal Descuento { get; set; }
        public bool PrecioCambio { get; set; }
        public decimal PrecioAnterior { get; set; }

        public override decimal CalcularTotal()
        {
            decimal total = CalcularSubtotal() - Descuento;
            return Math.Round(total, 2);
        }

        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            PrecioAnterior = Precio;
            Precio = nuevoPrecio;
            PrecioCambio = true;
        }
    }
}
