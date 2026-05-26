using POOI_cibertec_demo.Data;
using POOI_cibertec_demo.Models;

namespace POOI_cibertec_demo.Services
{
    public class CompraService
    {
        public async Task<string> GuardarCompraAsync()
        {
            await Task.Delay(3000);
            return "Compra guardada correctamente";
        }

        public async Task<List<ProductoOferta>> ObtenerHistorialAsync()
        {
            await Task.Delay(2000);
            return ListaComprasData.Productos;
        }

        public async Task<string> ProcesarCompraAsync()
        {
            await Task.Delay(5000);
            return "Compra procesada";
        }
    }
}
