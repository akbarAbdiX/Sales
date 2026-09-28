namespace BigBang.App.Sales.Migrations._20260715163947
{
    [FluentMigrator.Migration(2026_07_15_16_39_47, "Alter_tblSOPDocumentItem")]
    public class Alter_tblSOPDocumentItem : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260715163947.Alter_tblSOPDocumentItem_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260715163947.Alter_tblSOPDocumentItem_Down.sql");
        }
    }
}