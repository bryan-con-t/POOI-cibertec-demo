using POOI_cibertec_demo.Exceptions;
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
            decimal subtotal = CalcularSubtotal();
            if (Descuento < 0)
            {
                throw new DescuentoInvalidoException("El descuento no puede ser negativo.");
            }
            if (Descuento > subtotal)
            {
                throw new DescuentoInvalidoException("El descuento no puede ser mayor al subtotal.");
            }
            decimal total = subtotal - Descuento;
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
