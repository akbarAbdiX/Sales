namespace BigBang.App.Sales.Migrations._20260809123801
{
    [FluentMigrator.Migration(2026_08_09_12_38_01, "alter_tblSOPDocument")]
    public class Alter_tblSOPDocument : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260809123801.Alter_tblSOPDocument_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260809123801.Alter_tblSOPDocument_Down.sql");
        }
    }
}