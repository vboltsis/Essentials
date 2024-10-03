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