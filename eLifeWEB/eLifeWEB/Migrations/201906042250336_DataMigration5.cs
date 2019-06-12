namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "EndDate");
        }
    }
}
