namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "PatientId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Payments", "PatientId");
            AddForeignKey("dbo.Payments", "PatientId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "PatientId", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "PatientId" });
            DropColumn("dbo.Payments", "PatientId");
        }
    }
}
