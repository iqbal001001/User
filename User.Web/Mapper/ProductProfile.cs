//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using User.Domain;
//using User.Web.Dto;

//namespace User.Web.Mapper
//{
//    public class UserProfile : Profile
//    {
//        public UserProfile()
//        {
//            //CreateMap<UserInfo, UserDto>().ReverseMap()
//            //         .ForMember(dest => dest.Type,
//            //        opt => opt.MapFrom(s => (int)s.Type)); ;

//            CreateMap<UserInfo, UserDto>()
//                .ForMember(destination => destination.Type,
//                 opt => opt.MapFrom(source => Enum.GetName(typeof(UserType), source.Type)));


//            CreateMap<UserInfo, UserInsertDto>()
//                .ForMember(destination => destination.Type,
//                 opt => opt.MapFrom(source => Enum.GetName(typeof(UserType), source.Type)));

//            CreateMap<UserDto, UserInsertDto>()
//              .ForMember(destination => destination.Type,
//               opt => opt.MapFrom(source => Enum.Parse(typeof(UserType), source.Type, true)));

//            //CreateMap<UserInfo, UserDto>()
//            //.ForMember(destination => destination.Types,
//            //                      opt => opt.MapFrom(s => Enum.GetValues(typeof(UserType)).Cast<string>().ToList()));
//        }
//    }
  
//}
