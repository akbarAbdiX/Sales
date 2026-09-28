namespace BigBang.App.Sales.Migrations._20260713105149
{
    [FluentMigrator.Migration(2026_07_13_10_51_49, "Alter_tblSOPDocument")]
    public class Alter_tblSOPDocument : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260713105149.Alter_tblSOPDocument_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260713105149.Alter_tblSOPDocument_Down.sql");
        }
    }
}