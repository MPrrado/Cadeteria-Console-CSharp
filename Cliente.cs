namespace EspacioCliente
{
    public class Cliente
    {
        private string nombre;
        private string direccion;
        private long telefono;
        private string referencias;

        public string Nombre { get => nombre;}
        public string Direccion { get => direccion;}
        public long Telefono { get => telefono;}
        public string Referencias { get => referencias;}
        public Cliente()
        {
            
        }
        public Cliente(string nombre, string direccion, long telefono, string referencias)
        {
            this.nombre = nombre;
            this.direccion = direccion;
            this.telefono = telefono;
            this.referencias = referencias;
        }
        public Cliente(Cliente cliente)
        {
            this.nombre = cliente.Nombre;
            this.direccion = cliente.Direccion;
            this.telefono = cliente.Telefono;
            this.referencias = cliente.Referencias;
        }
    }
}