using FluentMigrator;

namespace FluentFlow.Example;

[Migration(202504070759)]
public class TestTableMigration : Migration
{
    public override void Up()
    {
        Create.Table("test_table")
            .WithColumn("daterange_column").AsDateTime()
            .WithColumn("small_int_column").AsInt16()
            .WithColumn("int_column").AsInt32()
            .WithColumn("big_int_column").AsInt64()
            .WithColumn("decimal_column").AsDecimal()
            .WithColumn("numeric_column").AsDecimal()
            .WithColumn("real_column").AsFloat()
            .WithColumn("double_precision_column").AsDouble()
            .WithColumn("money_column").AsDecimal()
            .WithColumn("tsrange_column").AsString()
            .WithColumn("tstzrange_column").AsString()
            .WithColumn("id").AsInt32()
            .WithColumn("id").AsInt32()
            .WithColumn("bytea_column").AsBinary()
            .WithColumn("timestamp_column").AsDateTime()
            .WithColumn("timestamptz_column").AsDateTimeOffset()
            .WithColumn("date_column").AsDateTime()
            .WithColumn("time_column").AsCustom("interval")
            .WithColumn("timetz_column").AsString()
            .WithColumn("interval_column").AsCustom("interval")
            .WithColumn("boolean_column").AsBoolean()
            .WithColumn("status_column").AsString()
            .WithColumn("point_column").AsString()
            .WithColumn("line_column").AsString()
            .WithColumn("lseg_column").AsString()
            .WithColumn("box_column").AsString()
            .WithColumn("path_column").AsString()
            .WithColumn("polygon_column").AsString()
            .WithColumn("circle_column").AsString()
            .WithColumn("inet_column").AsString()
            .WithColumn("cidr_column").AsString()
            .WithColumn("macaddr_column").AsString()
            .WithColumn("macaddr8_column").AsString()
            .WithColumn("json_column").AsString()
            .WithColumn("jsonb_column").AsString()
            .WithColumn("uuid_column").AsGuid()
            .WithColumn("xml_column").AsString()
            .WithColumn("int_array_column").AsCustom("object[]")
            .WithColumn("int4range_column").AsString()
            .WithColumn("char_column").AsString()
            .WithColumn("varchar_column").AsString()
            .WithColumn("text_column").AsString();
    }

    public override void Down()
    {
        Delete.Table("test_table");
    }
}