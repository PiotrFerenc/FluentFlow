namespace FluentFlow.Core;

public enum ColumnType
{
    Serial = 0,
    BigSerial,
    SmallSerial,
    BigInt,
    SmallInt,
    Decimal,
    CharacterVarying,
    Varchar,
    Boolean,
    Date,
    TimestampWithTimeZone,
    TimeWithoutTimeZone,
    TimeWithTimeZone,
    Jsonb,
    Uuid,
    Bytea,
    Array,
    Interval,
    Daterange,
    Real,
    Numeric,
    DoublePrecision,
    Money,
    Tsrange,
    Tstzrange,
    Integer,
    Point,
    Line,
    Lseg,
    Box,
    Path,
    Polygon,
    Circle,
    Inet,
    Cidr,
    Macaddr,
    Macaddr8,
    Json,
    Xml,
    Int4Range,
    Character,
    Text,
    Char,
    TimestampWithoutTimeZone
}

public enum CSharpType
{
    Int,
    Long,
    Short,
    Byte,
    Float,
    Double,
    Decimal,
    Bool,
    Char,
    String,
    Object,
    DateTime,
    TimeSpan,
    Guid,
    ByteArray,
    Nullable,
    List,
    Dictionary,
    Int32,
    Int64,
    Int16,
    Single,
    DateTimeOffset,
    ObjectArray
}

interface ICSharpType
{
}

readonly record struct Int32 : ICSharpType;
readonly record struct Int64 : ICSharpType;
readonly record struct Int16 : ICSharpType;
readonly record struct Single : ICSharpType;
readonly record struct DateTimeOffset : ICSharpType;
readonly record struct ObjectArray : ICSharpType;
readonly record struct Int : ICSharpType;
readonly record struct Long : ICSharpType;
readonly record struct Short : ICSharpType;
readonly record struct Byte : ICSharpType;
readonly record struct Float : ICSharpType;
readonly record struct Double : ICSharpType;
readonly record struct Decimal : ICSharpType;
readonly record struct Bool : ICSharpType;
readonly record struct Char : ICSharpType;
readonly record struct String : ICSharpType;
readonly record struct Object : ICSharpType;
readonly record struct DateTime : ICSharpType;
readonly record struct TimeSpan : ICSharpType;
readonly record struct Guid : ICSharpType;
readonly record struct ByteArray : ICSharpType;
readonly record struct Nullable : ICSharpType;
readonly record struct List : ICSharpType;
readonly record struct Dictionary : ICSharpType;


public static class CSharpTypeMapper
{
    public static readonly Dictionary<CSharpType, string> Mappings = new()
    {
        { CSharpType.Int, "AsInt32()" },
        { CSharpType.Long, "AsInt64()" },
        { CSharpType.Short, "AsInt16()" },
        { CSharpType.Byte, "AsByte()" },
        { CSharpType.Float, "AsFloat()" },
        { CSharpType.Double, "AsDouble()" },
        { CSharpType.Decimal, "AsDecimal()" },
        { CSharpType.Bool, "AsBoolean()" },
        { CSharpType.Char, "AsFixedLengthString(1)" },
        { CSharpType.String, "AsString()" },
        { CSharpType.Object, "AsCustom(\"object\")" },
        { CSharpType.DateTime, "AsDateTime()" },
        { CSharpType.TimeSpan, "AsCustom(\"interval\")" },
        { CSharpType.Guid, "AsGuid()" },
        { CSharpType.ByteArray, "AsBinary()" },
        { CSharpType.Nullable, "Nullable()" },
        { CSharpType.List, "AsCustom(\"array\")" },
        { CSharpType.Dictionary, "AsCustom(\"dictionary\")" },
        { CSharpType.Int32, "AsInt32()" },
        { CSharpType.Int64, "AsInt64()" },
        { CSharpType.Int16, "AsInt16()" },
        { CSharpType.Single, "AsFloat()" },
        { CSharpType.DateTimeOffset, "AsDateTimeOffset()" },
        { CSharpType.ObjectArray, "AsCustom(\"object[]\")" }
    };
}