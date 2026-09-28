namespace BigBang.App.Sales.Migrations._20260718172527
{
    [FluentMigrator.Migration(2026_07_18_17_25_27, "Insert_tblSOPDocumentType")]
    public class Insert_tblSOPDocumentType : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260718172527.Insert_tblSOPDocumentType_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260718172527.Insert_tblSOPDocumentType_Down.sql");
        }
    }
}