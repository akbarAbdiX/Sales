namespace BigBang.App.Sales.Migrations._20260923101959
{
    [FluentMigrator.Migration(2026_09_23_10_19_59, "alter_SOPAdjustment")]
    public class Alter_SOPAdjustment : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260923101959.Alter_SOPAdjustment_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260923101959.Alter_SOPAdjustment_Down.sql");
        }
    }
}