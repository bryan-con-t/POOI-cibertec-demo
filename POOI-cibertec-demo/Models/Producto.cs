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
            return Math.Round(Precio * Cantidad, 2);
        }

        public virtual decimal CalcularTotal() {
            return CalcularSubtotal();
        }
    }
}
