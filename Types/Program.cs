using FeatureExamples;

string filePath = @"C:\Temp\example.txt";
Console.WriteLine("Waiting for the file to be created...");

FileInfo createdFile = await new FileCreatedAwaitable(filePath);

Console.WriteLine("File has been created!");
Console.WriteLine($"File Name: {createdFile.Name}");
Console.WriteLine($"File Size: {createdFile.Length} bytes");
Console.WriteLine($"Created On: {createdFile.CreationTime}");
