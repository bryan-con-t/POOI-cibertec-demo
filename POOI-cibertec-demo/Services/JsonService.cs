using Newtonsoft.Json;
using POOI_cibertec_demo.Models;

namespace POOI_cibertec_demo.Services
{
    public class JsonService
    {
        public string ConvertirAJson(object datos)
        {
            return JsonConvert.SerializeObject(datos, Formatting.Indented);
        }

        public void GuardarJson(List<ProductoOferta> productos)
        {
            string json = JsonConvert.SerializeObject(productos, Formatting.Indented);
            File.WriteAllText("historial-compras.json", json);
        }

        public List<ProductoOferta> ConvertirDesdeJson(string json)
        {
            return JsonConvert.DeserializeObject<List<ProductoOferta>>(json) ?? new List<ProductoOferta>();
        }

        public List<ProductoOferta> LeerArchivoJson()
        {
            if (!File.Exists("historial-compras.json"))
            {
                return new List<ProductoOferta>();
            }
            string json = File.ReadAllText("historial-compras.json");
            return ConvertirDesdeJson(json);
        }
    }
}
