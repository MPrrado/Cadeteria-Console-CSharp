using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading.Channels;
using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;

bool siguePrograma = true;

Cadeteria miCadeteria = new Cadeteria("MiCadeteria",3815825154);
List<Cliente> listaClientes = [
    new ("Cliente 1", "direccion cliente 1", 3811111111, "ref 1"),
    new ("Cliente 2", "direccion cliente 2", 3812222222, "ref 2"),
    new ("Cliente 3", "direccion cliente 3", 3813333333, "ref 3"),
    new ("Cliente 4", "direccion cliente 4", 3814444444, "ref 4"),
    new ("Cliente 5", "direccion cliente 5", 3815555555, "ref 5")];
List<Cadete> listaCadetes = [
    new (1,"cadete 1", "direccion cadete 1", 3811111111),
    new (2,"cadete 2", "direccion cadete 2", 3812222222),
    new (3,"cadete 3", "direccion cadete 3", 3813333333),
    new (4,"cadete 4", "direccion cadete 4", 3814444444),
    new (5,"cadete 5", "direccion cadete 5", 3815555555)];

foreach (Cadete c in listaCadetes)
{
    miCadeteria.AgregarCadete(c);
}

Random random = new Random();

while (siguePrograma)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("=====================================================");
    Console.WriteLine($"------------------------ Cadeteria: {miCadeteria.Nombre} --------------------");
    Console.WriteLine("[1] Alta pedido");
    Console.WriteLine("[2] Asignar pedido a cadete");
    Console.WriteLine("[3] Cambiar de estado del pedido");
    Console.WriteLine("[4] Rasignar pedido");
    Console.WriteLine("[5] Mostrar Pedidos");
    Console.WriteLine("[6] Mostrar Cadetes");
    Console.WriteLine("[7] Recaudado");
    Console.WriteLine("[8] Salir");
    Console.WriteLine("=====================================================");
    Console.ResetColor();
    Console.WriteLine("Seleccione una opcion: ");
    if(int.TryParse(Console.ReadLine(), out int opc))
    {
        switch (opc)
        {
            case 1:
                Console.WriteLine("----------------ALTA PEDIDOS----------------");
                bool banderaAltaPedido = true;
                while (banderaAltaPedido) // bucle para dar de alta mutliples pedidos
                {
                    Console.WriteLine("Ingrese la observacion del pedido: ");
                    string observacion = Console.ReadLine();
                    miCadeteria.AltaPedido(listaClientes[random.Next(4)],observacion);
                    bool letracorrecta = true;
                    while (letracorrecta)
                    {
                        Console.WriteLine("DESEA DAR DE ALTA MAS PEDIDOS? SI: S / NO: N");
                        string sigue = Console.ReadLine();
                        if(sigue == "N" || sigue == "n")
                        {
                            letracorrecta = false;
                            banderaAltaPedido = false;
                        }else if (sigue != "s" && sigue != "S")
                        {
                            Console.WriteLine("ERROR DEBE INGRESAR UNA OPCION VALIDA S/N");
                        }else
                        {
                            letracorrecta = false;
                        }
                    }
                }
                break;
            case 2:
                Console.WriteLine("-------Seleccione un pedido con el numero de pedido para asignar -------");
                bool banderaAsignacion = true;
                while (banderaAsignacion)
                {
                    miCadeteria.MostrarPedidosSinAsignar();
                    if(int.TryParse(Console.ReadLine(), out int numero))
                    {
                        if (miCadeteria.ObtenerListadoPedido().Exists(p => p.NumPedido == numero))
                        {
                            miCadeteria.AsignarPedidoACadete(numero);
                            banderaAsignacion = false;
                        }
                        else
                        {
                            Console.WriteLine($"ERROR EL PEDIDO NUMERO: {numero} NO EXISTE, INGRESE NUEVAMENTE");
                        }
                    }else //caso en el que no ingrese un numero de pedido e ingrese cualquier otro tipo de datos
                    {
                        Console.WriteLine("ERROR DEBE INGRESAR UN NUMERO DE PEDIDO, REVISAR ENTRADA");
                    }
                }
                // miCadeteria.MostrarCadetes();
                break;
            case 3:
                Console.WriteLine("-------Seleccione un pedido con el numero de pedido para cambiar su estado -------");
                Console.WriteLine();
                bool banderaSeleccion = true;
                while (banderaSeleccion) 
                {
                    miCadeteria.MostrarPedidosAsignados();
                    if(int.TryParse(Console.ReadLine(), out int numero1))
                    {
                        if(miCadeteria.ObtenerListadoPedido().Exists(p => p.NumPedido == numero1 && p.ObtenerEstadoPedido() != Estados.SinAsignar.ToString()))
                        {
                            Pedido pedido = miCadeteria.ObtenerPedido(numero1);
                            Console.WriteLine();
                            Console.WriteLine("Pedido seleccionado: ");
                            Console.WriteLine();
                            pedido.MostrarPedido();
                            Console.WriteLine();
                            Console.WriteLine("Seleccione el numero de estado que para cambiar");
                            Console.WriteLine();
                            bool banderaEstados = true;
                            while(banderaEstados)
                            {
                                miCadeteria.MostrarEstados();
                                if(int.TryParse(Console.ReadLine(), out int opcionEstado))
                                {
                                    if (opcionEstado <=4 && opcionEstado >= 0)
                                    {
                                        pedido.CambiarEstado((Estados)opcionEstado);
                                        banderaSeleccion = false;
                                        banderaEstados = false;
                                    }else
                                    {
                                        Console.WriteLine("ERROR SELECCIONE UN ESTADO VALIDO");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR SELECCIONE UN NUMERO DE ESTADO VALIDO");
                                }
                            }
                        }else
                        {
                            Console.WriteLine("ERROR SELECCIONE UN PEDIDO VALIDO");
                        }
                    }else
                    {
                        Console.WriteLine("ERROR DEBE INGRESAR UN NUMERO DE PEDIDO");
                    }
                }

                break;
            case 4:
                bool bandera = true;
                while(bandera)
                {
                    Console.WriteLine("-------------------------- PEDIDOS DISPONIBLES PARA REASIGNACION--------------------------");
                    bool banderaPedido = true;
                    while (banderaPedido)
                    {
                        miCadeteria.MostrarPedidosAsignados();
                        Console.WriteLine();
                        Console.WriteLine("SELECCIONE UN NUMERO DE PEDIDO PARA REASIGNAR");
                        Console.WriteLine();
                        if(int.TryParse(Console.ReadLine(), out int numPedido))
                        {
                            if(miCadeteria.ObtenerListadoPedido().Exists(p => p.NumPedido == numPedido && p.ObtenerEstadoPedido() != Estados.SinAsignar.ToString()))
                            {
                                Pedido pedido = miCadeteria.ObtenerPedido(numPedido);
                                Console.WriteLine("EL PEDIDO SELECCIONADO ES: ");
                                pedido.MostrarPedido();
                                bool acepta = true;
                                while(acepta)
                                {
                                    Console.WriteLine("DESEA CONTINUAR CON LA REASIGNACION: SI: S / NO: N");
                                    string continuar = Console.ReadLine();
                                    if (continuar == "s" || continuar == "S")
                                    {
                                        bool acepta_cadete = true;
                                        while (acepta_cadete)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("==========================CADETES==========================");
                                            miCadeteria.MostrarCadetes();
                                            Console.WriteLine("");
                                            Console.WriteLine("SELECCIONE UN CADETE PARA REASIGNARLE EL PEDIDO");
                                            if(int.TryParse(Console.ReadLine(), out int numeroCadete))
                                            {
                                                Cadete? cadete = miCadeteria.ObtenerCadete(numeroCadete);
                                                if(cadete != null)
                                                {
                                                    bool bandera_continua_cadete=true;
                                                    while(bandera_continua_cadete)
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("==================CADETE SELECCIONADO==================");
                                                        cadete.MostrarCadete();
                                                        Console.WriteLine();
                                                        Console.WriteLine("DESEA CONTINUAR CON LA REASIGNACION: SI: S / NO: N");
                                                        continuar = Console.ReadLine();
                                                        if(continuar == "S" || continuar == "s")
                                                        {
                                                            bool reasignacion = miCadeteria.ReasignarPedido(numPedido, numeroCadete);
                                                            Console.Clear();
                                                            miCadeteria.MostrarCadetes();
                                                            acepta = false;
                                                            banderaPedido = false;
                                                            bandera=false;
                                                            acepta_cadete=false;
                                                            bandera_continua_cadete=false;

                                                        }else if (continuar == "n" || continuar == "N")
                                                        {
                                                            acepta = false;
                                                            banderaPedido = false;
                                                            bandera=false;
                                                            acepta_cadete=false;
                                                            bandera_continua_cadete=false;
                                                            Console.WriteLine("Cancelando la reasignacion....");
                                                        }else
                                                        {
                                                            Console.WriteLine("ERROR, DEBE INGRESAR SI DESEA CONTINUAR O NO");
                                                        }
                                                    }
                                                }else
                                                {
                                                    Console.WriteLine("ERROR NO SE PUDO ENCONTRAR EL CADETE CON EL NUMERO INGRESADO");
                                                }
                                            }else
                                            {
                                                Console.WriteLine("ERROR DEBE INGRESAR UN NUMERO CORRECTO DE CADETE");
                                            }
                                        }
                                    }else if(continuar == "n" || continuar =="N")
                                    {
                                        acepta = false;
                                        banderaPedido = false;
                                        bandera = false;
                                        Console.WriteLine("Cancelando la reasignacion....");
                                    }else
                                    {
                                        Console.WriteLine("ERROR INGRESE SI DESEA CONTINUAR O NO");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("ERROR EL PRODUCTO SELECCIONADO ES INCORRECTO");
                            }
                        }else
                        {
                            Console.WriteLine("ERROR DEBE INGRESAR UN NUMERO DE PEDIDO");
                        }
                    }

                }
                break;
            case 5:
                Console.WriteLine("=========================== PEDIDOS ===========================");
                miCadeteria.MostrarPedidos();
                break;
            case 6:
                Console.WriteLine("=========================== CADETES ===========================");
                miCadeteria.MostrarCadetes();
                break;
            case 7:
                Console.WriteLine("=========================== RECAUDADO ===========================");
                Console.WriteLine($"TOTAL: ${miCadeteria.ObtenerRecaudacion()}");
                break;
            case 8:
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("SALIENDO.....");
                siguePrograma = false;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR DEBE INGRESAR UNA OPCION VALIDA");
                Console.ResetColor();
                break;
        }
    }else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("!!!!!!!!!!!!!!!!ERROR: LA OPCION INGRESADA DEBE SER UN NUMERO CORRESPONDIENTE A LOS QUE SE ENCUENTRAN ENTRE '[]'");
        Console.ResetColor();
    }
}
Console.ResetColor();