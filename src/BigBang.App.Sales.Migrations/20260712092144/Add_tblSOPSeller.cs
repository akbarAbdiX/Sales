namespace BigBang.App.Sales.Migrations._20260712092144
{
    [FluentMigrator.Migration(2026_07_12_09_21_44, "Add_tblSOPSeller")]
    public class Add_tblSOPSeller : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260712092144.Add_tblSOPSeller_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260712092144.Add_tblSOPSeller_Down.sql");
        }
    }
}