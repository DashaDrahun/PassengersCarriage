using Lab06.MVC.Carriage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06.MVC.Carriage.ModelBuilders
{
    public interface IModelBuilder
    {
        HomeIndexViewModel Build();
    }
}
