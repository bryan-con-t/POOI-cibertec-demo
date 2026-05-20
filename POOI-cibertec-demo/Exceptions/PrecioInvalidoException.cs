namespace POOI_cibertec_demo.Exceptions
{
    public class PrecioInvalidoException : Exception
    {
        public PrecioInvalidoException() : base("El precio debe ser mayor a 0.")
        {
        }
    }
}
