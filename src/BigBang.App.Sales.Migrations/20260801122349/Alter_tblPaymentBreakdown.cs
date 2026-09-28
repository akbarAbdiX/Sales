namespace BigBang.App.Sales.Migrations._20260801122349
{
    [FluentMigrator.Migration(2026_08_01_12_23_49, "alter_tblPaymentBreakdown")]
    public class Alter_tblPaymentBreakdown : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260801122349.Alter_tblPaymentBreakdown_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260801122349.Alter_tblPaymentBreakdown_Down.sql");
        }
    }
}