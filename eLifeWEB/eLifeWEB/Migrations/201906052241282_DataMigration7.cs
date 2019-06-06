namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "RecordId", "dbo.Records");
            DropIndex("dbo.Records", new[] { "Id", "DoctorId" });
            DropPrimaryKey("dbo.Records");
            AlterColumn("dbo.Records", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Records", "Id");
            CreateIndex("dbo.Records", new[] { "Id", "DoctorId" });
            AddForeignKey("dbo.Payments", "RecordId", "dbo.Records", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "RecordId", "dbo.Records");
            DropIndex("dbo.Records", new[] { "Id", "DoctorId" });
            DropPrimaryKey("dbo.Records");
            AlterColumn("dbo.Records", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Records", "Id");
            CreateIndex("dbo.Records", new[] { "Id", "DoctorId" });
            AddForeignKey("dbo.Payments", "RecordId", "dbo.Records", "Id");
        }
    }
}
