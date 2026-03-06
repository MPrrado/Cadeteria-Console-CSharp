using System.Diagnostics.Contracts;
using EspacioPedido;

namespace EspacioCadete
{
    public class Cadete
    {
        private int id;
        private string nombre;
        private string direccion;
        private long telefono;
        private List<Pedido> listadoPedidos;

        public int Id { get => id;}

        public Cadete()
        {

        }

        public Cadete(int id, string nombre, string direccion, long telefono)
        {
            this.id =id;
            this.nombre = nombre;
            this.direccion = direccion;
            this.telefono = telefono;
            this.listadoPedidos = new List<Pedido>();
        }

        public double ObtenerJornal()
        {
            double totalRecaudado = 0;

            foreach (Pedido p in listadoPedidos)
            {
                if (p.ObtenerEstadoPedido() == Estados.Entregado.ToString())
                {
                    totalRecaudado += 500;
                }
            }
            return totalRecaudado;
        }

        public bool AsignarPedido(Pedido pedido)
        {
            try
            {
                listadoPedidos.Add(pedido);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR NO SE PUDO ASIGNAR EL PEDIDO NUMERO: {pedido.NumPedido}");
                return false;
            }
        }
        
        public int ObtenerPendientes()
        {
            return listadoPedidos.Count(p => p.ObtenerEstadoPedido() == Estados.Pendiente.ToString());
        }

        public string ObtenerNombreCadete()
        {
            return nombre;
        }

        public List<Pedido> ObtenerListadoPedidos()
        {
            return listadoPedidos;
        }

        public bool EliminarPedido(int numeroPedido)
        {
            if (listadoPedidos.Exists(p => p.NumPedido == numeroPedido && p.ObtenerEstadoPedido() != Estados.Entregado.ToString()))
            {
                listadoPedidos.Remove(listadoPedidos.Find(p => p.NumPedido == numeroPedido)); //utilizo find porque si llegó hasta acá entonces es porque existe
                return true;
            }else
            {
                return false;
            }
        }

        public void MostrarCadete()
        {
            Console.WriteLine("------------------------Cadete------------------------");
            Console.WriteLine($"N°: {id} | nombre: {nombre} | direccion: {direccion} | telefono: {telefono}");
        }
    }
}