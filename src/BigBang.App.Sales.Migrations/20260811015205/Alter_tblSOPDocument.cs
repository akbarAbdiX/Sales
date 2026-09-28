namespace BigBang.App.Sales.Migrations._20260811015205
{
    [FluentMigrator.Migration(2026_08_11_01_52_05, "alter_tblSOPDocument")]
    public class Alter_tblSOPDocument : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260811015205.Alter_tblSOPDocument_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260811015205.Alter_tblSOPDocument_Down.sql");
        }
    }
}