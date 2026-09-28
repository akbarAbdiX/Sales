namespace BigBang.App.Sales.Migrations._20260822113208
{
    [FluentMigrator.Migration(2026_08_22_11_32_08, "alter_tblSOPDocument")]
    public class Alter_tblSOPDocument : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260822113208.Alter_tblSOPDocument_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260822113208.Alter_tblSOPDocument_Down.sql");
        }
    }
}