namespace POOI_cibertec_demo.Models
{
    public partial class Producto
    {
        public decimal CalcularDescuento()
        {
            if (CalcularSubtotal() >= 100)
            {
                return Math.Round(CalcularSubtotal() * 0.1m, 2);
            } else
            {
                return 0;
            }
        }
    }
}
