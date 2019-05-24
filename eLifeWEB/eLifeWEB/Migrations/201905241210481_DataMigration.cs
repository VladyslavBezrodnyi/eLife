namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            CreateTable(
                "dbo.ClinicAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClinicId = c.Int(nullable: false),
                        ClinicConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinics", t => t.ClinicId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.ConversationReplies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversationId = c.Int(nullable: false),
                        ReplyText = c.String(nullable: false),
                        Time = c.DateTime(nullable: false),
                        SenderId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversations", t => t.ConversationId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.ConversationId)
                .Index(t => t.SenderId);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Doctor_Id = c.String(maxLength: 128),
                        Patient_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Doctor_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Patient_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.DoctorInforms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Specialization = c.String(nullable: false),
                        Category = c.String(name: "Category ", nullable: false),
                        Guardian = c.String(nullable: false),
                        ClinicId = c.Int(),
                        Education = c.String(nullable: false),
                        Skills = c.String(nullable: false),
                        Practiced = c.Boolean(nullable: false),
                        Image = c.Binary(nullable: false, storeType: "image"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinics", t => t.ClinicId)
                .Index(t => t.ClinicId);
            
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Adress = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        BankCard = c.String(nullable: false),
                        Image = c.Binary(storeType: "image"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        PatientId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Stars = c.Int(nullable: false),
                        Comment = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.DoctorId, t.PatientId })
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorId)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.PatientInforms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adress = c.String(nullable: false),
                        Activity = c.String(nullable: false),
                        BloodGroup = c.String(),
                        Allergy = c.String(),
                        Operations = c.String(),
                        Infectiousdiseases = c.String(name: "Infectious diseases"),
                        Diabetes = c.String(),
                        BankCard = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PatientId = c.String(maxLength: 128),
                        DoctorId = c.String(maxLength: 128),
                        TypeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Complaints = c.String(),
                        ObjectiveData = c.String(),
                        Diagnoses = c.String(),
                        CourseOfTheDisease = c.String(),
                        Appointment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientId)
                .ForeignKey("dbo.TypeOfServices", t => new { t.Id, t.DoctorId })
                .Index(t => new { t.Id, t.DoctorId })
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        RecordId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Signature = c.String(nullable: false),
                        State = c.Boolean(nullable: false),
                        Message = c.String(nullable: false),
                        Ref = c.String(nullable: false),
                        Amf = c.Decimal(nullable: false, storeType: "money"),
                        Ccy = c.String(nullable: false),
                        Comis = c.Decimal(nullable: false, storeType: "money"),
                        Code = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Records", t => t.RecordId)
                .Index(t => t.RecordId);
            
            CreateTable(
                "dbo.TypeOfServices",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DoctorId = c.String(nullable: false, maxLength: 128),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => new { t.Id, t.DoctorId })
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorId)
                .ForeignKey("dbo.Types", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String());
            AddColumn("dbo.AspNetUsers", "Bithday", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.AspNetUsers", "PatientInformId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DoctorInformId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "ClinicAdminId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "PatientInformId");
            CreateIndex("dbo.AspNetUsers", "DoctorInformId");
            CreateIndex("dbo.AspNetUsers", "ClinicAdminId");
            AddForeignKey("dbo.AspNetUsers", "ClinicAdminId", "dbo.ClinicAdmins", "Id");
            AddForeignKey("dbo.AspNetUsers", "DoctorInformId", "dbo.DoctorInforms", "Id");
            AddForeignKey("dbo.AspNetUsers", "PatientInformId", "dbo.PatientInforms", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TypeOfServices", "Id", "dbo.Types");
            DropForeignKey("dbo.Records", new[] { "Id", "DoctorId" }, "dbo.TypeOfServices");
            DropForeignKey("dbo.TypeOfServices", "DoctorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "RecordId", "dbo.Records");
            DropForeignKey("dbo.Records", "PatientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "PatientInformId", "dbo.PatientInforms");
            DropForeignKey("dbo.Feedbacks", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "PatientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Feedbacks", "DoctorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DoctorInforms", "ClinicId", "dbo.Clinics");
            DropForeignKey("dbo.ClinicAdmins", "ClinicId", "dbo.Clinics");
            DropForeignKey("dbo.AspNetUsers", "DoctorInformId", "dbo.DoctorInforms");
            DropForeignKey("dbo.Conversations", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConversationReplies", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConversationReplies", "ConversationId", "dbo.Conversations");
            DropForeignKey("dbo.Conversations", "Patient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "Doctor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ClinicAdminId", "dbo.ClinicAdmins");
            DropIndex("dbo.TypeOfServices", new[] { "DoctorId" });
            DropIndex("dbo.TypeOfServices", new[] { "Id" });
            DropIndex("dbo.Payments", new[] { "RecordId" });
            DropIndex("dbo.Records", new[] { "PatientId" });
            DropIndex("dbo.Records", new[] { "Id", "DoctorId" });
            DropIndex("dbo.Feedbacks", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Feedbacks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Feedbacks", new[] { "PatientId" });
            DropIndex("dbo.Feedbacks", new[] { "DoctorId" });
            DropIndex("dbo.DoctorInforms", new[] { "ClinicId" });
            DropIndex("dbo.Conversations", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Conversations", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Conversations", new[] { "Patient_Id" });
            DropIndex("dbo.Conversations", new[] { "Doctor_Id" });
            DropIndex("dbo.ConversationReplies", new[] { "SenderId" });
            DropIndex("dbo.ConversationReplies", new[] { "ConversationId" });
            DropIndex("dbo.AspNetUsers", new[] { "ClinicAdminId" });
            DropIndex("dbo.AspNetUsers", new[] { "DoctorInformId" });
            DropIndex("dbo.AspNetUsers", new[] { "PatientInformId" });
            DropIndex("dbo.ClinicAdmins", new[] { "ClinicId" });
            DropColumn("dbo.AspNetUsers", "ClinicAdminId");
            DropColumn("dbo.AspNetUsers", "DoctorInformId");
            DropColumn("dbo.AspNetUsers", "PatientInformId");
            DropColumn("dbo.AspNetUsers", "Bithday");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.Types");
            DropTable("dbo.TypeOfServices");
            DropTable("dbo.Payments");
            DropTable("dbo.Records");
            DropTable("dbo.PatientInforms");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Clinics");
            DropTable("dbo.DoctorInforms");
            DropTable("dbo.Conversations");
            DropTable("dbo.ConversationReplies");
            DropTable("dbo.ClinicAdmins");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
    }
}
