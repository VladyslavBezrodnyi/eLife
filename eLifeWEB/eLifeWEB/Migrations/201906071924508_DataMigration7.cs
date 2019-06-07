namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PatientInforms", "BankCard");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PatientInforms", "BankCard", c => c.String(nullable: false));
        }
    }
}
