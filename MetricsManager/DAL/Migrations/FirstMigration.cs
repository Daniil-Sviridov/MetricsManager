using FluentMigrator;

namespace MetricsManager.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("agents").WithColumn("id").AsInt64().PrimaryKey().Identity().WithColumn("agentaddress").AsString(2048).WithColumn("isenabled").AsBoolean().NotNullable();
            Create.Table("cpumetrics").WithColumn("id").AsInt32().PrimaryKey().Identity().WithColumn("agentid").AsInt64().WithColumn("value").AsInt32().WithColumn("time").AsInt64();
            Create.Table("dotnetmetrics").WithColumn("id").AsInt32().PrimaryKey().Identity().WithColumn("agentid").AsInt64().WithColumn("value").AsInt32().WithColumn("time").AsInt64();
            Create.Table("networkmetrics").WithColumn("id").AsInt32().PrimaryKey().Identity().WithColumn("agentid").AsInt64().WithColumn("value").AsInt32().WithColumn("time").AsInt64();
            Create.Table("hddmetrics").WithColumn("id").AsInt32().PrimaryKey().Identity().WithColumn("agentid").AsInt64().WithColumn("value").AsInt32().WithColumn("time").AsInt64();
            Create.Table("rammetrics").WithColumn("id").AsInt32().PrimaryKey().Identity().WithColumn("agentid").AsInt64().WithColumn("value").AsInt32().WithColumn("time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("cpumetrics");
            Delete.Table("dotnetmetrics");
            Delete.Table("hddmetrics");
            Delete.Table("hddmetrics");
            Delete.Table("rammetrics");
        }
    }
}

