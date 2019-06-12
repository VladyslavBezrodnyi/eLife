namespace eLifeWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Payments");
            AddColumn("dbo.Payments", "order_id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Payments", "payment_id", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "liqpay_order_id", c => c.String());
            AddColumn("dbo.Payments", "currency", c => c.String());
            AddColumn("dbo.Payments", "amount", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Payments", "status", c => c.String());
            AddPrimaryKey("dbo.Payments", "order_id");
            DropColumn("dbo.Payments", "PaymentId");
            DropColumn("dbo.Payments", "Id");
            DropColumn("dbo.Payments", "Signature");
            DropColumn("dbo.Payments", "State");
            DropColumn("dbo.Payments", "Message");
            DropColumn("dbo.Payments", "Ref");
            DropColumn("dbo.Payments", "Amf");
            DropColumn("dbo.Payments", "Ccy");
            DropColumn("dbo.Payments", "Comis");
            DropColumn("dbo.Payments", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "Code", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "Comis", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Payments", "Ccy", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "Amf", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Payments", "Ref", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "Message", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "State", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "Signature", c => c.String(nullable: false));
            AddColumn("dbo.Payments", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "PaymentId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Payments");
            DropColumn("dbo.Payments", "status");
            DropColumn("dbo.Payments", "amount");
            DropColumn("dbo.Payments", "currency");
            DropColumn("dbo.Payments", "liqpay_order_id");
            DropColumn("dbo.Payments", "payment_id");
            DropColumn("dbo.Payments", "order_id");
            AddPrimaryKey("dbo.Payments", "PaymentId");
        }
    }
}
