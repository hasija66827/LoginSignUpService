namespace LoginSignUpService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingemailAttributeNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "EmailId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "EmailId", c => c.String(nullable: false));
        }
    }
}
