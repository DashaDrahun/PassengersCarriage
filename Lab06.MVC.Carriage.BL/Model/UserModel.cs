namespace Lab06.MVC.Carriage.BL.Model
{
    public class UserModel
    {
        // todo: не реализует IModel - скажется ли это на работе проги?
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
    }
}
