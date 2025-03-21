namespace CT_Login.Models
{
    public class User
    {
        public int UsuarioID { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Password { get; set; }
        // Otros campos que puedas necesitar...
        public List<Role> Roles { get; set; }
        public List<Subrole> Subroles { get; set; }
    }
}
