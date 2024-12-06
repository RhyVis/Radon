using Radon.Core.Util;
using Xunit.Abstractions;

namespace Radon.AppTest;

public class PdxLangParserTest(ITestOutputHelper output)
{
    private const string TestContent1 =
        """
        l_test:

        # Comment

        Test.0062.name:0 "Name"
        Test.0062.desc:0 "Desc"
        Test.0062.a:0 "A"
        Test.0062.a.response:0 "A.resp"
        Test.0062.b:0 "B"
        Test.0062.b.response:0 "B.resp"
        Test.0062.c:0 "C"
        Test.0062.c.response:0 "C.resp"
        Test.0062.d:0 "D"
        Test.0062.d.response:0 "D.resp"
        Test.0062.e:0 "E"
        Test.0062.e.response:0 "E.resp"
        Test.0062.f:0 "F"
        """;

    private const string TestContent2 =
        """
        l_test:

        # Comment

        Test.0062.name:0 "Name"
        Test.0062.desc.a:0 "DescA"
        Test.0062.desc.b:0 "DescB"
        Test.0062.desc.c:0 "DescC"
        Test.0062.a:0 "A"
        """;

    private const string TestContent3 =
        """
        l_test:

        # Comment

        Test.TRe.name:0 "Name"
        Test.TRe.desc.a:0 "DescA"
        Test.TRe.desc.b:0 "DescB"
        Test.TRe.desc.c:0 "DescC"
        Test.TRe.a:0 "A"
        Test.TRe.a.response:0 "A Resp"
        Test.TRe.a.tooltip:0 "A TT"
        Test.TRe.b:0 "B"
        Test.TRe.b.response:0 "B Resp"
        Test.TRe.c:0 "C"
        Test.TRe.c.tooltip:0 "C TT"
        """;

    private const string TestContent4 =
        """
        l_test:

        # Comment

        Test.TRe.name:0 "Name 1"
        Test.TRe.desc.a:0 "Desc 1 A"
        Test.TRe.desc.b:0 "Desc 1 B"

        Test.TRf.desc:0 "Desc 2"
        Test.TRf.desc.a:0 "Desc 2 A"
        Test.TRf.desc.b:0 "Desc 2 B"

        Test.TRz.name:0 "Name 3"
        Test.TRz.desc:0 "Desc 3"
        Test.TRz.b:0 "B"

        """;

    private const string TestContentMixed =
        """
        l_test:

        # Comment

        Test.0062.name:0 "Name"
        Test.0062.desc:0 "Desc"
        Test.0062.a:0 "A"
        Test.0062.a.response:0 "A.resp"
        Test.0062.b:0 "B"
        Test.0062.b.response:0 "B.resp"
        Test.0062.c:0 "C"
        Test.0062.c.response:0 "C.resp"
        Test.0062.d:0 "D"
        Test.0062.d.response:0 "D.resp"
        Test.0062.e:0 "E"
        Test.0062.e.response:0 "E.resp"
        Test.0062.f:0 "F"

        Test.0063.name:0 "Name 2"
        Test.0063.desc.a:0 "Desc 2 A"
        Test.0063.desc.b:0 "Desc 2 B"
        Test.0063.desc.c:0 "Desc 2 C"
        Test.0063.a:0 "A 2"

        Test.0064.name:0 "Name 3"
        Test.0064.desc.a:0 "Desc 3 A"
        Test.0064.desc.b:0 "Desc 3 B"
        Test.0064.desc.c:0 "Desc 3 C"
        Test.0064.a:0 "A 3"
        Test.0064.a.response:0 "A 3 Resp"
        Test.0064.a.tooltip:0 "A 3 TT"
        Test.0064.b:0 "B 3"
        Test.0064.b.response:0 "B 3 Resp"
        Test.0064.c:0 "C 3"
        Test.0064.c.tooltip:0 "C 3 TT"

        Test.0065.name:0 "Name 4"
        Test.0065.desc.a:0 "Desc 4 A"
        Test.0065.desc.b:0 "Desc 4 B"

        Test.0066.desc:0 "Desc 5"
        Test.0066.desc.a:0 "Desc 5 A"
        Test.0066.desc.b:0 "Desc 5 B"

        Test.0067.name:0 "Name 6"
        Test.0067.desc:0 "Desc 6"
        Test.0067.b:0 "B 6"
        """;

    private void WriteResult(List<PdxLangParsedItem> parsedItems, List<PdxLangEventItem> eventItems)
    {
        output.WriteLine("Items");
        foreach (var item in parsedItems) output.WriteLine($"{item.Name} {item.Namespace} {item.Value}");

        output.WriteLine("Events:");
        foreach (var eventItem in eventItems)
        {
            output.WriteLine("!Name");
            output.WriteLine(eventItem.Name);

            output.WriteLine("!Desc");
            output.WriteLine(eventItem.Desc);

            output.WriteLine("!Options");
            foreach (var option in eventItem.Options)
            {
                output.WriteLine("!Option");
                output.WriteLine(option.Name);
                output.WriteLine(option.Key);
                output.WriteLine(option.Resp);
            }
        }
    }

    [Fact]
    public void TestEventNestedResponse()
    {
        var parser = PdxLangParser.Create(TestContent1);
        var parsedItems = parser.GetParsedItems();
        var eventItems = parser.GetEventItems();

        WriteResult(parsedItems, eventItems);

        Assert.Equal(13, parsedItems.Count);
        Assert.Single(eventItems);
    }

    [Fact]
    public void TestEventNestedDesc()
    {
        var parser = PdxLangParser.Create(TestContent2);
        var parsedItems = parser.GetParsedItems();
        var eventItems = parser.GetEventItems();

        WriteResult(parsedItems, eventItems);

        Assert.Single(eventItems);
    }

    [Fact]
    public void TestEventNestedRespAndTooltip()
    {
        var parser = PdxLangParser.Create(TestContent3);
        var parsedItems = parser.GetParsedItems();
        var eventItems = parser.GetEventItems();

        WriteResult(parsedItems, eventItems);

        Assert.Single(eventItems);
    }

    [Fact]
    public void TestEventNotCompleteEvent()
    {
        var parser = PdxLangParser.Create(TestContent4);
        var parsedItems = parser.GetParsedItems();
        var eventItems = parser.GetEventItems();

        WriteResult(parsedItems, eventItems);

        Assert.Equal(3, eventItems.Count);
    }

    [Fact]
    public void TestEventLongMixed()
    {
        var parser = PdxLangParser.Create(TestContentMixed);
        var parsedItems = parser.GetParsedItems();
        var eventItems = parser.GetEventItems();

        WriteResult(parsedItems, eventItems);
    }
}
