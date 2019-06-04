namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "payment_id", c => c.Int());
            AlterColumn("dbo.Payments", "amount", c => c.Decimal(storeType: "money"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "amount", c => c.Decimal(nullable: false, storeType: "money"));
            AlterColumn("dbo.Payments", "payment_id", c => c.Int(nullable: false));
        }
    }
}
