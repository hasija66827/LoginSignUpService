namespace LoginSignUpService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatedatabase : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Latitude", c => c.String());
            AlterColumn("dbo.Users", "Longitude", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Longitude", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Latitude", c => c.String(nullable: false));
        }
    }
}
