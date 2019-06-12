namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TypeOfServices", "Id", "dbo.Types");
            DropForeignKey("dbo.Records", new[] { "Id", "DoctorId" }, "dbo.TypeOfServices");
            DropIndex("dbo.Records", new[] { "Id", "DoctorId" });
            DropIndex("dbo.TypeOfServices", new[] { "Id" });
            DropIndex("dbo.TypeOfServices", new[] { "DoctorId" });
            DropPrimaryKey("dbo.TypeOfServices");
            AddColumn("dbo.Records", "TypeOfServiceId", c => c.Int());
            AddColumn("dbo.TypeOfServices", "Name", c => c.String());
            AlterColumn("dbo.TypeOfServices", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.TypeOfServices", "DoctorId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.TypeOfServices", "Id");
            CreateIndex("dbo.Records", "TypeOfServiceId");
            CreateIndex("dbo.TypeOfServices", "DoctorId");
            AddForeignKey("dbo.Records", "TypeOfServiceId", "dbo.TypeOfServices", "Id");
            DropColumn("dbo.Records", "DoctorId");
            DropColumn("dbo.Records", "TypeId");
            DropTable("dbo.Types");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Records", "TypeId", c => c.Int());
            AddColumn("dbo.Records", "DoctorId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Records", "TypeOfServiceId", "dbo.TypeOfServices");
            DropIndex("dbo.TypeOfServices", new[] { "DoctorId" });
            DropIndex("dbo.Records", new[] { "TypeOfServiceId" });
            DropPrimaryKey("dbo.TypeOfServices");
            AlterColumn("dbo.TypeOfServices", "DoctorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TypeOfServices", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.TypeOfServices", "Name");
            DropColumn("dbo.Records", "TypeOfServiceId");
            AddPrimaryKey("dbo.TypeOfServices", new[] { "Id", "DoctorId" });
            CreateIndex("dbo.TypeOfServices", "DoctorId");
            CreateIndex("dbo.TypeOfServices", "Id");
            CreateIndex("dbo.Records", new[] { "Id", "DoctorId" });
            AddForeignKey("dbo.Records", new[] { "Id", "DoctorId" }, "dbo.TypeOfServices", new[] { "Id", "DoctorId" });
            AddForeignKey("dbo.TypeOfServices", "Id", "dbo.Types", "Id");
        }
    }
}
