namespace ClassesAndStudents.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class EFModel1 : DbContext
    {
        // Контекст настроен для использования строки подключения "EFModel1" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "ClassesAndStudents.Models.EFModel1" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "EFModel1" 
        // в файле конфигурации приложения.
        public EFModel1()
            : base("name=EFModel1")
        {
            Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using modelBuilder here

            modelBuilder.Entity<ClassEntity>().HasMany<StudentEntity>(m => m.StudentEntities).WithMany(m=>m.ClassEntities)
                .Map(cs =>
                {
                    cs.MapLeftKey("ClassEntityId");
                    cs.MapRightKey("StudentEntityId");
                    cs.ToTable("ClassStudents");
                }); ;

            base.OnModelCreating(modelBuilder);
        }
        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<ClassEntity> ClassEntities { get; set; }
        public virtual DbSet<StudentEntity> StudentEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}