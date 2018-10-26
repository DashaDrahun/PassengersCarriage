using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Interfaces;
using Lab06.MVC.Carriage.Models;
using Lab06.MVC.Carriage.BL.Model;

namespace Lab06.MVC.Carriage.ModelBuilders
{
    public class ModelBuilder : IModelBuilder
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public ModelBuilder(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public HomeIndexViewModel Build()
        {
            string pathToPictures = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images"));
            var directory = new DirectoryInfo(pathToPictures);
            var files = directory.GetFiles();
            List<string> picturesPathes = new List<string>();
            foreach (var file in files)
            {
                picturesPathes.Add(Path.Combine(file.Directory.Name, file.Name));
            }

            IEnumerable<RouteModel> routeModels = userService.GetAllRoutes();
            List<RouteViewModel> allRoutesVm = mapper.Map<IEnumerable<RouteModel>, List<RouteViewModel>>(routeModels);

            return new HomeIndexViewModel
            {
                AllRoutes = allRoutesVm,
                Pictures = picturesPathes
            };
        }
    }
}