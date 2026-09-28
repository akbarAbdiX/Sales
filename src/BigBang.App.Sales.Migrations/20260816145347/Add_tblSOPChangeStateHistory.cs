namespace BigBang.App.Sales.Migrations._20260816145347
{
    [FluentMigrator.Migration(2026_08_16_14_53_47, "Add_tblSOPChangeStateHistory")]
    public class Add_tblSOPChangeStateHistory : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260816145347.Add_tblSOPChangeStateHistory_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260816145347.Add_tblSOPChangeStateHistory_Down.sql");
        }
    }
}