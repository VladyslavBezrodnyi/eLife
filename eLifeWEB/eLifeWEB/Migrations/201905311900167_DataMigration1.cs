namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Conversations", new[] { "Doctor_Id" });
            DropIndex("dbo.Conversations", new[] { "Patient_Id" });
            DropColumn("dbo.Conversations", "DoctorId");
            DropColumn("dbo.Conversations", "PatientId");
            RenameColumn(table: "dbo.Conversations", name: "Doctor_Id", newName: "DoctorId");
            RenameColumn(table: "dbo.Conversations", name: "Patient_Id", newName: "PatientId");
            AlterColumn("dbo.Conversations", "PatientId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Conversations", "DoctorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Conversations", "PatientId");
            CreateIndex("dbo.Conversations", "DoctorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Conversations", new[] { "DoctorId" });
            DropIndex("dbo.Conversations", new[] { "PatientId" });
            AlterColumn("dbo.Conversations", "DoctorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Conversations", "PatientId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Conversations", name: "PatientId", newName: "Patient_Id");
            RenameColumn(table: "dbo.Conversations", name: "DoctorId", newName: "Doctor_Id");
            AddColumn("dbo.Conversations", "PatientId", c => c.Int(nullable: false));
            AddColumn("dbo.Conversations", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Conversations", "Patient_Id");
            CreateIndex("dbo.Conversations", "Doctor_Id");
        }
    }
}
