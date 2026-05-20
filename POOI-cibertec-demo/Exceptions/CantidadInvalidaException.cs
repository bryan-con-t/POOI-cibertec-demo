namespace POOI_cibertec_demo.Exceptions
{
    public class CantidadInvalidaException : Exception
    {
        public CantidadInvalidaException() : base("La cantidad debe ser mayor a 0.")
        {
        }
    }
}
