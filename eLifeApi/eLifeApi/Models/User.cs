namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Microsoft.AspNet.Identity.EntityFramework;

    [Serializable]
    public partial class User // : IdentityUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            ConversationReplies = new HashSet<ConversationReply>();
            Conversations = new HashSet<Conversation>();
            Conversations1 = new HashSet<Conversation>();
            Feedbacks = new HashSet<Feedback>();
            Feedbacks1 = new HashSet<Feedback>();
            Records = new HashSet<Record>();
            TypeOfServices = new HashSet<TypeOfService>();
        }

       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        public int ?Role_id { get; set; }

        [DisplayName("Номер телефону")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfimed { get; set; }

        [Required]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        [DisplayName("Електронна пошта")]
        public string Email { get; set; }

        public bool EmailConfimed { get; set; }

        [DisplayName("ПІБ")]
        public string Name { get; set; }

        [DisplayName("Стать")]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Дата народження")]
        public DateTime? Bithday { get; set; }

        [DisplayName("Пацієнт")]
        public int? PatientId { get; set; }

        [DisplayName("Лікар")]
        public int? DoctorId { get; set; }

        [DisplayName("Адміністратор клініки")]
        public int? ClinicAdminId { get; set; }

        public virtual ClinicAdmin ClinicAdmin { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConversationReply> ConversationReplies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Conversation> Conversations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Conversation> Conversations1 { get; set; }

        public virtual DoctorInform DoctorInform { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks1 { get; set; }

        public virtual PatientInform PatientInform { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Record> Records { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TypeOfService> TypeOfServices { get; set; }
    }
}
