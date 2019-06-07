namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Records", "PatientId", "dbo.AspNetUsers");
            AddColumn("dbo.Records", "AttendingDoctorId", c => c.String(maxLength: 128));
            AddColumn("dbo.Records", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Records", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            CreateIndex("dbo.Records", "AttendingDoctorId");
            CreateIndex("dbo.Records", "ApplicationUser_Id");
            CreateIndex("dbo.Records", "ApplicationUser_Id1");
            AddForeignKey("dbo.Records", "AttendingDoctorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Records", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Records", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Records", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Records", "AttendingDoctorId", "dbo.AspNetUsers");
            DropIndex("dbo.Records", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Records", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Records", new[] { "AttendingDoctorId" });
            DropColumn("dbo.Records", "ApplicationUser_Id1");
            DropColumn("dbo.Records", "ApplicationUser_Id");
            DropColumn("dbo.Records", "AttendingDoctorId");
            AddForeignKey("dbo.Records", "PatientId", "dbo.AspNetUsers", "Id");
        }
    }
}
