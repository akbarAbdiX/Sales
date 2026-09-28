namespace BigBang.App.Sales.Migrations._20260718210541
{
    [FluentMigrator.Migration(2026_07_18_21_05_41, "Insert_tblSOPDocumentType")]
    public class Insert_tblSOPDocumentType : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260718210541.Insert_tblSOPDocumentType_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260718210541.Insert_tblSOPDocumentType_Down.sql");
        }
    }
}