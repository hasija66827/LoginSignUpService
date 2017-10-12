namespace LoginSignUpService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsRegisterdUserFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsRegisteredUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsRegisteredUser");
        }
    }
}
