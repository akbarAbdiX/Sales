namespace BigBang.App.Sales.Migrations._20260809163342
{
    [FluentMigrator.Migration(2026_08_09_16_33_42, "Alter_tblSOPDocument_Add_ContractNumber_Column")]
    public class Alter_tblSOPDocument_Add_ContractNumber_Column : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260809163342.Alter_tblSOPDocument_Add_ContractNumber_Column_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260809163342.Alter_tblSOPDocument_Add_ContractNumber_Column_Down.sql");
        }
    }
}