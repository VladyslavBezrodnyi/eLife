namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Records", "TypeId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Records", "TypeId", c => c.Int(nullable: false));
        }
    }
}
