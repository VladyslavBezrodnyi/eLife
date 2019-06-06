using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eLifeWEB.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("ПІБ")]
        public string Name { get; set; }

        [DisplayName("Стать")]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Дата народження")]
        public DateTime? Bithday { get; set; }
        [DisplayName("Пацієнт")]
        public int? PatientInformId { get; set; }

        [DisplayName("Лікар")] 
        public int? DoctorInformId { get; set; }

        [DisplayName("Адміністратор клініки")]
        public int? ClinicAdminId { get; set; }

        public virtual ClinicAdmin ClinicAdmin { get; set; }

        public virtual ICollection<ConversationReply> ConversationReplies { get; set; }

        public virtual ICollection<Conversation> Conversations { get; set; }

        public virtual ICollection<Conversation> Conversations1 { get; set; }

        public virtual DoctorInform DoctorInform { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual ICollection<Feedback> Feedbacks1 { get; set; }

        public virtual PatientInform PatientInform { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        public virtual ICollection<Record> Records1 { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<TypeOfService> TypeOfServices { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ClinicAdmin> ClinicAdmins { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<ConversationReply> ConversationReplies { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<DoctorInform> DoctorInforms { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<PatientInform> PatientInforms { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<TypeOfService> TypeOfServices { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<eLifeWEB.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}