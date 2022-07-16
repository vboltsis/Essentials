namespace CSharpHistory;

class Version1_0
{
	//EVENTS
    //public event EventHandler<FileListArgs> Progress;
    //Progress?.Invoke(this, new FileListArgs(file));
    //EventHandler<FileListArgs> onProgress = (sender, eventArgs) =>
    //Console.WriteLine(eventArgs.FoundFile);

    //fileLister.Progress += onProgress;
    //fileLister.Progress -= onProgress;
}

struct Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }
}

interface IShape
{
	void Draw();
}
/*
The major features of C# 1.0 included:

Classes
Structs
Interfaces
Events
Properties
Delegates
Operators and expressions
Statements
Attributes
*/