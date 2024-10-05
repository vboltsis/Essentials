using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace RandomLibraries;

public class QuestpdfExample
{
    public void ExampleWithViewer()
    {
        Document.Create(containter =>
        {
            containter.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("I am the head")
                    .SemiBold().FontSize(30).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);
                        x.Item().Text(Placeholders.LoremIpsum());
                        x.Item().Image(Placeholders.Image(200, 100));
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }).ShowInPreviewer();
    }

    public void ExampleWithPdf()
    {
        Document.Create(containter =>
        {
            containter.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("I am the head")
                    .SemiBold().FontSize(30).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);
                        x.Item().Text(Placeholders.LoremIpsum());
                        x.Item().Image(Placeholders.Image(200, 100));
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }).GeneratePdf("test.pdf");
    }

    public void ExampleWithByteArray()
    {
        byte[] pdfBytes;

        using (var stream = new MemoryStream())
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content()
                        .Padding(50)
                        .Text(text =>
                        {
                            text.Span("Hello, World!").FontColor(Colors.Red.Accent4);
                        });
                });
            })
            .GeneratePdf(stream);

            pdfBytes = stream.ToArray();
        }
    }
}


//Document.Create(container =>
//{
//    container.Page(page =>
//    {
//        page.Content()
//            .Padding(50)
//            .Text(text =>
//            {
//                text.Span("Hello, World!").FontColor(Colors.Red.Accent4);
//            });
//    });
//})
//.GeneratePdf("C:\\temp\\example.pdf");


//Document.Create(container =>
//{
//    container.Page(page =>
//    {
//        page.Size(PageSizes.Letter);
//        page.Margin(1, Unit.Inch);
//        page.PageColor(Colors.Grey.Lighten3);

//        page.Content()
//            .Column(column =>
//            {
//    column.Spacing(15);
//    var randomNumber = Random.Shared.Next(0, 2);

//    bool includeSection = randomNumber is 0;

//    if (includeSection)
//    {
//        column.Item().Text("Are you talking to me?!")
//            .Bold()
//            .FontSize(69)
//            .FontColor(Colors.Green.Darken1);
//    }

//    column.Item().Row(row =>
//    {
//        row.RelativeItem()
//           .Text("I am the one who knocks.")
//           .FontSize(18);

//        row.ConstantItem(100)
//           .Image(Placeholders.Image(420, 690));
//    });
//});
//    });
//})
//    .ShowInPreviewer();
//.GeneratePdf("C:\\temp\\dynamic-layout.pdf");