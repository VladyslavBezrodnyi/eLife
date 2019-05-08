namespace eLifeApi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    public partial class eLifeDB /*: IdentityDbContext<User>*/ : DbContext
    {
        public eLifeDB()
            : base("name=eLifeDB")
        {
        }

        public virtual DbSet<ClinicAdmin> ClinicAdmins { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<ConversationReply> ConversationReplies { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<DoctorInform> DoctorInforms { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<PatientInform> PatientInforms { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public new DbSet<Role> Roles { get; set; }
        public virtual DbSet<TypeOfService> TypeOfServices { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public new DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicAdmin>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.ClinicAdmin)
                .HasForeignKey(e => e.ClinicAdminId);

            modelBuilder.Entity<Clinic>()
                .HasMany(e => e.ClinicAdmins)
                .WithRequired(e => e.Clinic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Clinic>()
                .HasMany(e => e.DoctorInforms)
                .WithRequired(e => e.Clinic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DoctorInform>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.DoctorInform)
                .HasForeignKey(e => e.DoctorId);

            modelBuilder.Entity<PatientInform>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.PatientInform)
                .HasForeignKey(e => e.PatientId);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amf)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Comis)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Record>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Record)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.Role_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeOfService>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TypeOfService>()
                .HasMany(e => e.Records)
                .WithRequired(e => e.TypeOfService)
                .HasForeignKey(e => new { e.Id_type, e.Id_doctor })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Type>()
                .HasMany(e => e.TypeOfServices)
                .WithRequired(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ConversationReplies)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Id_sender);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Conversations)
                .WithRequired(e => e.Doctor)
                .HasForeignKey(e => e.Id_doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Conversations1)
                .WithRequired(e => e.Patient)
                .HasForeignKey(e => e.Id_patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Doctor)
                .HasForeignKey(e => e.Id_doctor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Feedbacks1)
                .WithRequired(e => e.Patient)
                .HasForeignKey(e => e.Id_patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Records)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Id_patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TypeOfServices)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Id_doctor)
                .WillCascadeOnDelete(false);
        }
    }
}
