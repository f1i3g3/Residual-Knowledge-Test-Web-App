using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Shared;

namespace ResidualKnowledgeTestApp.Server
{
    /// <summary>
    /// База данных
    /// </summary>
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Список пользователей
        /// </summary>
        // public DbSet<User> Users { get; set; }

        /// <summary>
        /// Созданные проекты
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Учебные планы
        /// </summary>
        public DbSet<Curriculum> Curriculums { get; set; }

        /// <summary>
        /// Дисциплины
        /// </summary>
        public DbSet<Discipline> Disciplines { get; set; }

        /// <summary>
        /// Компетенции
        /// </summary>
        public DbSet<Competence> Competences { get; set; }

        /// <summary>
        /// Выбранные дисциплины
        /// </summary>
        public DbSet<CheckingDiscipline> CheckingDisciplines { get; set; } // В базе данных??

        /// <summary>
        /// Критерии оценивания
        /// </summary>
        public DbSet<MarkCriterion> MarkCriteria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CheckingDiscipline>()
                .HasMany(cd => cd.CheckingCompetences)
                .WithMany(cc => cc.CheckingDisciplines)
                .UsingEntity<UserSelection>(
                   us => us
                    .HasOne(pt => pt.CheckingCompetence)
                    .WithMany(t => t.UserSelection)
                    .HasForeignKey(pt => pt.CheckingCompetenceId),
                   us => us
                    .HasOne(pt => pt.CheckingDiscipline)
                    .WithMany(p => p.UserSelection)
                    .HasForeignKey(pt => pt.CheckingDisciplineId),
                us =>
                {
                    us.HasKey(t => new { t.CheckingCompetenceId, t.CheckingDisciplineId });
                    us.ToTable("UserSelections");
                });

            modelBuilder
                .Entity<Discipline>()
                .HasMany(d => d.Competences)
                .WithMany(c => c.Disciplines)
                .UsingEntity<DisciplineCompetence>(
                   us => us
                    .HasOne(pt => pt.Competence)
                    .WithMany(t => t.DisciplineCompetences)
                    .HasForeignKey(pt => pt.CompetenceId),
                   us => us
                    .HasOne(pt => pt.Discipline)
                    .WithMany(p => p.DisciplineCompetences)
                    .HasForeignKey(pt => pt.DisciplineId),
                us =>
                {
                    us.HasKey(t => new { t.CompetenceId, t.DisciplineId });
                    us.ToTable("DisciplineCompetence");
                });
        }
    }
}
