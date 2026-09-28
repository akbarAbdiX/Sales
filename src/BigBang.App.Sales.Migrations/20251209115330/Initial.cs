namespace BigBang.App.Sales.Migrations._0
{
    [FluentMigrator.Migration(2025_12_09_11_53_30, "Initial")]
    public class Initial : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20251209115330.Initial_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20251209115330.Initial_Down.sql");
        }
    }
}