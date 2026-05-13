using Microsoft.OpenApi;

namespace Pure.Primitives.Abstractions.OpenAPI.Schema.Tests;

public sealed record PrimitivesDocumentTransformerTests
{
    private static OpenApiDocument BuildDocumentWithSchema(
        string name,
        JsonSchemaType type
    )
    {
        return new OpenApiDocument
        {
            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, IOpenApiSchema>
                {
                    [name] = new OpenApiSchema { Type = type },
                },
            },
        };
    }

    private static IOpenApiSchema Schema(OpenApiDocument document, string name)
    {
        return document.Components!.Schemas![name];
    }

    [Fact]
    public async Task IStringIsReplacedWithNativeStringSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IString",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.String, Schema(document, "IString").Type);
        Assert.Null(Schema(document, "IString").Format);
    }

    [Fact]
    public async Task IBoolIsReplacedWithNativeBooleanSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IBool",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.Boolean, Schema(document, "IBool").Type);
    }

    [Fact]
    public async Task IGuidIsReplacedWithStringUuidSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IGuid",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.String, Schema(document, "IGuid").Type);
        Assert.Equal("uuid", Schema(document, "IGuid").Format);
    }

    [Fact]
    public async Task IDateIsReplacedWithStringDateSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IDate",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.String, Schema(document, "IDate").Type);
        Assert.Equal("date", Schema(document, "IDate").Format);
    }

    [Fact]
    public async Task ITimeIsReplacedWithStringTimeSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "ITime",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.String, Schema(document, "ITime").Type);
        Assert.Equal("time", Schema(document, "ITime").Format);
    }

    [Fact]
    public async Task IDateTimeIsReplacedWithStringDateTimeSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IDateTime",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.String, Schema(document, "IDateTime").Type);
        Assert.Equal("date-time", Schema(document, "IDateTime").Format);
    }

    [Fact]
    public async Task IDayOfWeekIsReplacedWithIntegerSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IDayOfWeek",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.Integer, Schema(document, "IDayOfWeek").Type);
    }

    [Fact]
    public async Task ICharIsReplacedWithStringSingleCharSchema()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "IChar",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.String, Schema(document, "IChar").Type);
        Assert.Equal(1, Schema(document, "IChar").MaxLength);
    }

    public static TheoryData<string, JsonSchemaType, string> NumberVariants()
    {
        return new()
        {
            { "INumberByte", JsonSchemaType.Integer, "int32" },
            { "INumberSByte", JsonSchemaType.Integer, "int32" },
            { "INumberInt16", JsonSchemaType.Integer, "int32" },
            { "INumberUInt16", JsonSchemaType.Integer, "int32" },
            { "INumberInt32", JsonSchemaType.Integer, "int32" },
            { "INumberUInt32", JsonSchemaType.Integer, "int64" },
            { "INumberInt64", JsonSchemaType.Integer, "int64" },
            { "INumberUInt64", JsonSchemaType.Integer, "int64" },
            { "INumberSingle", JsonSchemaType.Number, "float" },
            { "INumberDouble", JsonSchemaType.Number, "double" },
            { "INumberDecimal", JsonSchemaType.Number, "double" },
            { "INumberIntPtr", JsonSchemaType.Integer, "int64" },
            { "INumberUIntPtr", JsonSchemaType.Integer, "int64" },
        };
    }

    [Theory]
    [MemberData(nameof(NumberVariants))]
    public async Task INumberVariantsAreReplacedWithNativeNumericSchemas(
        string schemaName,
        JsonSchemaType expectedType,
        string expectedFormat
    )
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            schemaName,
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(expectedType, Schema(document, schemaName).Type);
        Assert.Equal(expectedFormat, Schema(document, schemaName).Format);
    }

    [Fact]
    public async Task UnrelatedSchemaIsNotModified()
    {
        OpenApiDocument document = BuildDocumentWithSchema(
            "MyCustomType",
            JsonSchemaType.Object
        );

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );

        Assert.Equal(JsonSchemaType.Object, Schema(document, "MyCustomType").Type);
    }

    [Fact]
    public async Task NullComponentsDoesNotThrow()
    {
        OpenApiDocument document = new();

        await new PrimitivesDocumentTransformer().TransformAsync(
            document,
            null!,
            CancellationToken.None
        );
    }
}
