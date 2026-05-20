using POOI_cibertec_demo.Exceptions;

namespace POOI_cibertec_demo.Models
{
    public partial class Producto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Estado { get; set; }

        public Producto()
        {
            Estado = "Pendiente";
        }

        public decimal CalcularSubtotal()
        {
            if (Precio <= 0)
            {
                throw new PrecioInvalidoException();
            }
            if (Cantidad <= 0)
            {
                throw new CantidadInvalidaException();
            }
            return Math.Round(Precio * Cantidad, 2);
        }

        public virtual decimal CalcularTotal() {
            return CalcularSubtotal();
        }

        public void MarcarComoComprado()
        {
            Estado = "Comprado";
        }

        public void MarcarComoPendiente()
        {
            Estado = "Pendiente";
        }
    }
}
