using Lab06.MVC.Carriage.BL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Model
{
    public class RouteModel: IModel
    {
        public int Id { get; set; }

        public double Kilometres { get; set; }

        public string CityDepart { get; set; }

        public string CityArr { get; set; }
    }
}
