namespace LoginSignUpService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingpasscodeattribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Passcode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Passcode");
        }
    }
}
