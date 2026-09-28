namespace BigBang.App.Sales.Migrations._20260815082949
{
    [FluentMigrator.Migration(2026_08_15_08_29_49, "Alter_tblSOPDocument_Change_Size_Of_UniqueTaxIdentifier_Column")]
    public class Alter_tblSOPDocument_Change_Size_Of_UniqueTaxIdentifier_Column : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260815082949.Alter_tblSOPDocument_Change_Size_Of_UniqueTaxIdentifier_Column_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260815082949.Alter_tblSOPDocument_Change_Size_Of_UniqueTaxIdentifier_Column_Down.sql");
        }
    }
}