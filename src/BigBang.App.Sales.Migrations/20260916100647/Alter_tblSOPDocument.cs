namespace BigBang.App.Sales.Migrations._20260916100647
{
    [FluentMigrator.Migration(2026_09_16_10_06_47, "alter_tblSOPDocument")]
    public class Alter_tblSOPDocument : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260916100647.Alter_tblSOPDocument_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260916100647.Alter_tblSOPDocument_Down.sql");
        }
    }
}