using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ResidualKnowledgeTestApp.Shared
{
    public class Project : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }
       
        public DateTime LastEditionTime { get; set; }

        public int? CurriculumId { get; set; }

        [ForeignKey(nameof(CurriculumId))]
        public Curriculum Curriculum { get; set; }

        public Stage Stage { get; set; }

        public List<CheckingDiscipline> CheckingDisciplines { get; set; } = new List<CheckingDiscipline>();

        public string GoogleSheetLink { get; set; }

        // public User User { get; set; }
    }
}
