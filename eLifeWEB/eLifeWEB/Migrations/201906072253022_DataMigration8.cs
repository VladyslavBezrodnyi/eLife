namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Records", "PatientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "RecordId", "dbo.Records");
            DropIndex("dbo.Records", new[] { "Id", "DoctorId" });
            DropPrimaryKey("dbo.Records");
            AddColumn("dbo.Records", "AttendingDoctorId", c => c.String(maxLength: 128));
            AddColumn("dbo.Records", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Records", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AlterColumn("dbo.Records", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Records", "TypeId", c => c.Int());
            AddPrimaryKey("dbo.Records", "Id");
            CreateIndex("dbo.Records", new[] { "Id", "DoctorId" });
            CreateIndex("dbo.Records", "AttendingDoctorId");
            CreateIndex("dbo.Records", "ApplicationUser_Id");
            CreateIndex("dbo.Records", "ApplicationUser_Id1");
            AddForeignKey("dbo.Records", "AttendingDoctorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Records", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Records", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Payments", "RecordId", "dbo.Records", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Payments", "RecordId", "dbo.Records");
            DropForeignKey("dbo.Records", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "AttendingDoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.Records", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Records", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Records", new[] { "AttendingDoctorId" });
            DropIndex("dbo.Records", new[] { "Id", "DoctorId" });
            DropPrimaryKey("dbo.Records");
            AlterColumn("dbo.Records", "TypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Records", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Records", "ApplicationUser_Id1");
            DropColumn("dbo.Records", "ApplicationUser_Id");
            DropColumn("dbo.Records", "AttendingDoctorId");
            AddPrimaryKey("dbo.Records", "Id");
            CreateIndex("dbo.Records", new[] { "Id", "DoctorId" });
            AddForeignKey("dbo.Payments", "RecordId", "dbo.Records", "Id");
            AddForeignKey("dbo.Records", "PatientId", "dbo.AspNetUsers", "Id");
        }
    }
}
