using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lab06.MVC.Carriage.BL.Infrastructure;
using Lab06.MVC.Carriage.BL.Model;
using Lab06.MVC.Carriage.DAL.Entities;

namespace Lab06.MVC.Carriage.BL.Mappers
{
    public class TripMapper: ITripMapper
    {
        private readonly IMapper mapper;

        public TripMapper()
        {
            mapper =
                new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Trip, TripModel>()
                            .ForMember(v => v.NumbersOfFreeSeats,
                                opts => opts.MapFrom(src =>
                                    !String.IsNullOrWhiteSpace(src.FreeSeetsNumbers)
                                        ? src.FreeSeetsNumbers.Split(' ').Select(x => Int32.Parse(x))
                                        : new List<int>()))
                            .ForSourceMember(x => x.Orders, y => y.Ignore());
                        cfg.CreateMap<TripModel, Trip>().IgnoreAllVirtual();
                    })
                    .CreateMapper();
        }

        public TripModel MapModel(Trip sourceTrip)
        {
            return mapper.Map<Trip, TripModel>(sourceTrip);
        }

        public IEnumerable<TripModel> MapCollectionModels(IEnumerable<Trip> sourceTrips)
        {
            return mapper.Map<IEnumerable<Trip>, IEnumerable<TripModel>>(sourceTrips);
        }

        public Trip MapEntity(TripModel sourceModel)
        {
            var trip = mapper.Map<Trip>(sourceModel);
            trip.FreeSeetsNumbers = string.Join(" ", sourceModel.NumbersOfFreeSeats.Select(x => x.ToString()));
            return trip;
        }
    }
}
