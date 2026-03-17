using System.Data;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using EspacioAccesoADatos;
using EspacioCadete;
using EspacioCliente;
using EspacioPedido;

namespace EspacioCadeteria
{
    public class Cadeteria
    {
        private string nombre;
        private long telefono;
        private List<Cadete> listadoCadete;
        private List<Pedido> listadoPedidos;

        public Cadeteria()
        {

        }
        public Cadeteria(string nombre, long telefono)
        {
            this.nombre = nombre;
            this.telefono = telefono;
            this.listadoCadete = new List<Cadete>();
            this.listadoPedidos = new List<Pedido>();
        }

        public string Nombre { get => nombre;}
        public long Telefono { get => telefono;}

        public double ObtenerRecaudacion()
        {
            double totalRecaudado = 0;
            foreach (Cadete c in listadoCadete)
            {
                totalRecaudado += c.ObtenerJornal();
            }
            return totalRecaudado;
        }

        public bool AltaPedido(string? observacion, string rutaArchivoClientes)
        {
            try
            {
                string[] datosClienteAleatorio = AccesoADatos.ObtenerCliente(rutaArchivoClientes);
                Pedido pedido = new(datosClienteAleatorio[0],datosClienteAleatorio[1],long.Parse(datosClienteAleatorio[2]), datosClienteAleatorio[3], observacion);
                pedido.AsignarNumeroPedido(listadoPedidos.Count == 0 ? 1:listadoPedidos.Count+1);
                pedido.CambiarEstado(Estados.SinAsignar);
                listadoPedidos.Add(pedido);
                return true;
                
            }catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR NO SE PUDO DAR DE ALTA EL PEDIDO");
                Console.ResetColor();
                return  false;
            }
        }

        public void AgregarCadete(Cadete cadete)
        {
            try
            {
                listadoCadete.Add(cadete);
            }catch(Exception e)
            {   
                Console.WriteLine("ERROR NO SE PUDO AGREGAR EL CADETE");
            }

        }

        public void AsignarPedidoACadete(int numeroPedido)
        {
            Cadete cadeteConMenosPedidos = listadoCadete.OrderBy(c => c.ObtenerPendientes()).First();
            Pedido pedidoParaAsignar = listadoPedidos.First(p => p.NumPedido == numeroPedido);
            pedidoParaAsignar.CambiarEstado(Estados.Pendiente);
            pedidoParaAsignar.AsignarIdCadete(cadeteConMenosPedidos.Id);
            cadeteConMenosPedidos.AsignarPedido(pedidoParaAsignar);
        }

        public void MostrarPedidos()
        {
            foreach(Pedido p in listadoPedidos)
            {
                p.MostrarPedido();
            }
        }
        public void MostrarPedidosSinAsignar()
        {
            foreach(Pedido p in listadoPedidos)
            {
                if(p.ObtenerEstadoPedido() == Estados.SinAsignar.ToString())
                {
                    p.MostrarPedido();
                }
            }
        }
        public void MostrarPedidosAsignados()
        {
            foreach(Pedido p in listadoPedidos)
            {
                if(p.ObtenerEstadoPedido() != Estados.SinAsignar.ToString() && p.ObtenerEstadoPedido() != Estados.Entregado.ToString())
                {
                    p.MostrarPedido();
                }
            }
        }

        public void MostrarCadetes()
        {
            foreach(Cadete c in listadoCadete)
            {
                c.MostrarCadete();
                Console.WriteLine("-------------------listado pedidos-------------------");
                foreach(Pedido p in c.ObtenerListadoPedidos())
                {
                    p.MostrarPedido();
                }
                Console.WriteLine("-----------------------------------------");
            }
        }
        
        public Pedido ObtenerPedido(int numeroPedido)
        {
            return listadoPedidos.First(p => p.NumPedido == numeroPedido);
        }
        public void MostrarEstados()
        {
            Console.WriteLine($"[0]{Estados.SinAsignar}");
            Console.WriteLine($"[1]{Estados.Pendiente}");
            Console.WriteLine($"[2]{Estados.Preparacion}");
            Console.WriteLine($"[3]{Estados.EnCamino}");
            Console.WriteLine($"[4]{Estados.Entregado}");
        }
        
        public List<Pedido> ObtenerListadoPedido()
        {
            return listadoPedidos;
        }
        // agregar metodo que me permita reasignar pedido
        public bool ReasignarPedido(int numeroPedido, int numeroCadete)
        {
            Pedido pedidoReasignar = listadoPedidos.First(p => p.NumPedido == numeroPedido); //obtengo el pedido para luego usar el idCadete que tiene para eliminar el pedido de la lista del mismo
            Cadete cadeteELiminar = listadoCadete.First(c => c.Id == pedidoReasignar.IdCadete);//Obtengo el cadete del que hay que eliminarle el pedido de su lista
            //elimino el pedido de la lista del cadete que lo tenia
            bool seBorro = cadeteELiminar.EliminarPedido(pedidoReasignar.NumPedido);

            if(seBorro)
            {
                Cadete cadeteNuevo = ObtenerCadete(numeroCadete);
                bool seAgrego= cadeteNuevo.AsignarPedido(pedidoReasignar);
                if(seAgrego)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
        public Cadete? ObtenerCadete(int numeroCadete)
        {
            return listadoCadete.Exists(c => c.Id ==numeroCadete) ? listadoCadete.First(c => c.Id == numeroCadete): null;
        }
        /*
        esto implica que en la clase cadete tengo que tener un metodo que me permita eliminar pedido solamente si no esta entregado
        */
    }
}