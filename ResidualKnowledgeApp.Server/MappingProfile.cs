using AutoMapper;
using ResidualKnowledgeApp.Shared;
using ResidualKnowledgeApp.Shared.DTO;
using ResidualKnowledgeApp.Shared.ViewModels;
using System.Linq;

namespace ResidualKnowledgeApp.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Competence, CompetenceDTO>();

            CreateMap<DisciplineCompetence, CompetenceWithDisciplineDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Competence.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Competence.Code))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Competence.Description))
                .ForMember(dest => dest.Discipline, opt => opt.MapFrom(src => src.Discipline)).ReverseMap();

            CreateMap<CompetenceWithDisciplineDTO, Competence>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Disciplines, opt => opt.Ignore())
                .ForMember(dest => dest.CheckingDisciplines, opt => opt.Ignore())
                .ForMember(dest => dest.CurriculumId, opt => opt.Ignore())
                .ForMember(dest => dest.DisciplineCompetences, opt => opt.Ignore())
                .ForMember(dest => dest.UserSelection, opt => opt.Ignore());

            CreateMap<Discipline, DisciplineDTO>()
                .ForMember(dest => dest.Competences, opt => opt.MapFrom(src => src.Competences))
                .ForMember(dest => dest.DisciplineName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DisciplineCode, opt => opt.MapFrom(src => src.Code))
                .ReverseMap();

            CreateMap<Curriculum, CurriculumDTO>();

            CreateMap<CreateProjectVM, Project>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Project, ProjectDetailsDTO>()
                .ForMember(dto => dto.Curriculum, opt => opt.MapFrom(p => p.Curriculum))
                .ForMember(dto => dto.CheckingDisciplines, opt => opt.MapFrom(p => p.CheckingDisciplines));

            CreateMap<Project, ProjectOverviewDTO>()
                .ForMember(dest => dest.ProgrammeName, opt => opt.MapFrom(src => src.Curriculum.ProgrammeName))
                .ForMember(dest => dest.ProgrammeCode, opt => opt.MapFrom(src => src.Curriculum.ProgrammeCode))
                .ForMember(dest => dest.LevelOfEducation, opt => opt.MapFrom(src => src.Curriculum.LevelOfEducation));


            //CreateMap<Project, ProjectOverviewDTO>().IncludeMembers(s => s.Curriculum);
            //CreateMap<Curriculum, ProjectOverviewDTO>(MemberList.None)
            //    .ForMember(dest => dest.ProgrammeName, opt => opt.MapFrom(src => src.ProgrammeName))
            //    .ForMember(dest => dest.ProgrammeCode, opt => opt.MapFrom(src => src.ProgrammeCode))
            //    .ForMember(dest => dest.LevelOfEducation, opt => opt.MapFrom(src => src.LevelOfEducation));

            CreateMap<CreateCheckingDisciplineVM, CheckingDiscipline>();

            CreateMap<UserSelection, CompetenceWithDisciplineDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CheckingCompetence.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CheckingCompetence.Code))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.CheckingCompetence.Description))
                .ForMember(dest => dest.Discipline, opt => opt.MapFrom(src => src.CheckingDiscipline.Discipline));

            CreateMap<CheckingDiscipline, CheckingDisciplineDetailsDTO>()
                 .ForMember(dest => dest.CheckingCompetences, opt => opt.MapFrom(src => src.UserSelection));

            CreateMap<CheckingDiscipline, CheckingDisciplineOverviewDTO>();

            CreateMap<Discipline, CheckingDiscipline>()
                .ForMember(dest => dest.DisciplineId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Curriculum.ProjectId));
        }
    }
}
