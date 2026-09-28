namespace BigBang.App.Sales.Migrations._20260825143519
{
    [FluentMigrator.Migration(2026_08_25_14_35_19, "alter_tblSOPDocument")]
    public class Alter_tblSOPDocument : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260825143519.Alter_tblSOPDocument_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260825143519.Alter_tblSOPDocument_Down.sql");
        }
    }
}