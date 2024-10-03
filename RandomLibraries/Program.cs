using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using RandomLibraries;

QuestPDF.Settings.License = LicenseType.Community;

//var pdf = new QuestpdfExample();

//pdf.ExampleWithViewer();

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.Letter);
        page.Margin(1, Unit.Inch);
        page.PageColor(Colors.Grey.Lighten3);

        page.Content()
            .Column(column =>
            {
                column.Spacing(15);
                var randomNumber = Random.Shared.Next(0, 2);

                bool includeSection = randomNumber is 0;

                if (includeSection)
                {
                    column.Item().Text("Are you talking to me?!")
                        .Bold()
                        .FontSize(69)
                        .FontColor(Colors.Green.Darken1);
                }

                column.Item().Row(row =>
                {
                    row.RelativeItem()
                       .Text("I am the one who knocks.")
                       .FontSize(18);

                    row.ConstantItem(100)
                       .Image(Placeholders.Image(420, 690));
                });
            });
    });
})
    .ShowInPreviewer();
//.GeneratePdf("C:\\temp\\dynamic-layout.pdf");


Console.ReadLine();