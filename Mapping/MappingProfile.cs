using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
            .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
             .ForMember(vr => vr.Make, opt => opt.MapFrom(v => v.Model.Make))
             .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf =>
             new KeyValuePairResource { Id = vf.FeatureId, Name = vf.Feature.Name })));

            // API Resource to Domain
            CreateMap<VehicleQueryResource, VehicleQuery>();

            CreateMap<SaveVehicleResource, Vehicle>()
            .ForMember(v => v.Id, opt => opt.Ignore())
            .ForMember(v => v.Features, opt => opt.Ignore())
            .AfterMap((vr, v) =>
            {
                // Remove unselected features
                var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId)).ToList();
                foreach (var feature in removedFeatures)
                    v.Features.Remove(feature);

                // Add new features
                var addedFeatures = vr.Features.Where(id => v.Features.All(f => f.FeatureId != id))
                    .Select(id => new VehicleFeature { FeatureId = id }).ToList();
                foreach (var feature in addedFeatures)
                    v.Features.Add(feature);

            });
            CreateMap<ContactResource, Contact>();
        }
    }
}