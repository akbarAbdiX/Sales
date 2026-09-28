namespace BigBang.App.Sales.Migrations._20260816171001
{
    [FluentMigrator.Migration(2026_08_16_17_10_01, "Add_tblSOPChangeStateHistory_CommentColumn")]
    public class Add_tblSOPChangeStateHistory_CommentColumn : FluentMigrator.Migration //Migration|ForwardOnlyMigration|AutoReversingMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("_20260816171001.Add_tblSOPChangeStateHistory_CommentColumn_Up.sql");
        }

        public override void Down()
        {
            Execute.EmbeddedScript("_20260816171001.Add_tblSOPChangeStateHistory_CommentColumn_Down.sql");
        }
    }
}