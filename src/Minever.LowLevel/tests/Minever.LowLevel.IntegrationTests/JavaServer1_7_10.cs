namespace Minever.LowLevel.IntegrationTests;

[CollectionDefinition(nameof(JavaServer1_7_10Collection))]
public abstract class JavaServer1_7_10Collection : ICollectionFixture<JavaServer1_7_10>
{
}

public class JavaServer1_7_10 : JavaServer
{
    public override string Version => "1.7.10";
}
