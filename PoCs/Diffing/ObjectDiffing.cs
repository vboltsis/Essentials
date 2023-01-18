using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

namespace PoCs.Diffing;

public class MyClass
{
    public static HashSet<string> PropertiesChanged = new();

    private string myProperty;
    public string MyProperty
    {
        get { return myProperty; }
        set
        {
            myProperty = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertiesChanged.Add(propertyName);
    }
}

public class SetterLoggerGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        // Get all the types in the compilation
        var types = context.Compilation.SyntaxTrees
            .SelectMany(tree => tree.GetRoot().DescendantNodes())
            .OfType<ClassDeclarationSyntax>()
            .ToList();

        // Filter the types to only include those that implement the IGen interface
        //var genTypes = types.Where(type => type..Implements("IGen"));
        var genTypes = types;

        // For each type, add a Console.WriteLine() to all its setters
        foreach (var type in genTypes)
        {
            // Get all the setters in the type
            var setters = type.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(property => property.AccessorList.Accessors
                    .Any(accessor => accessor.Kind() == SyntaxKind.SetAccessorDeclaration));

            // For each setter, add a Console.WriteLine() to the body of the setter
            foreach (var setter in setters)
            {
                var statement = SyntaxFactory.ParseStatement(
                    "Console.WriteLine($\"Setter called: {type.Identifier.Text}.{setter.Identifier.Text}\");");
                var newAccessor = setter.AccessorList.Accessors
                    .Single(accessor => accessor.Kind() == SyntaxKind.SetAccessorDeclaration)
                    .WithBody(SyntaxFactory.Block(statement));
                var newProperty = setter.WithAccessorList(
                    setter.AccessorList.WithAccessors(
                        SyntaxFactory.List(new[] { newAccessor })));

                //context..AddCompilationUnit(type.SyntaxTree.FilePath, newProperty);
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        
    }
}

//public class MyClass : INotifyPropertyChanged
//{
//    private string _imageFullPath;

//    protected void OnPropertyChanged(PropertyChangedEventArgs e)
//    {
//        PropertyChangedEventHandler handler = PropertyChanged;
//        if (handler != null)
//            handler(this, e);
//    }

//    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
//    {
//        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
//    }

//    public string ImageFullPath
//    {
//        get { return _imageFullPath; }
//        set
//        {
//            if (value != _imageFullPath)
//            {
//                _imageFullPath = value;
//                OnPropertyChanged();
//            }
//        }
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//}

//public class ObjectDiffing
//{
//    public IEnumerable<string> FindDifferences<T>(T object1, T object2)
//    {
//        // Create a parameter for the Expression trees
//        var parameter = Expression.Parameter(typeof(T), "obj");

//        // Create a list to store the differences
//        var differences = new List<string>();

//        // Get the properties of the class
//        var properties = typeof(T).GetProperties();

//        // Iterate over each property and compare the values of the objects
//        foreach (var property in properties)
//        {
//            // Check if the property is a nested class
//            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
//            {
//                // If the property is a nested class, recursively call the method to find the differences
//                var nestedObject1 = property.GetValue(object1);
//                var nestedObject2 = property.GetValue(object2);

//                var nestedDifferences = FindDifferences(nestedObject1, nestedObject2);

//                // Add the property name as a prefix to the differences from the nested class
//                differences.AddRange(nestedDifferences.Select(x => $"{property.Name}.{x}"));
//            }
//            else
//            {
//                // Create an expression to get the value of the property for the first object
//                var propertyValue1 = Expression.Property(parameter, property.Name);

//                // Create an expression to get the value of the property for the second object
//                var propertyValue2 = Expression.Property(parameter, property.Name);

//                // Check if the property is a list
//                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
//                {
//                    // If the property is a list, compare the items in the list using the SequenceEqual method
//                    var list1 = propertyValue1 as IList<>;
//                    var list2 = propertyValue2 as IList<>;

//                    var listValuesAreEqual = list1.Cast<object>().SequenceEqual(list2.Cast<object>());

//                    // If the lists are not equal, add the property name to the list of differences
//                    if (!listValuesAreEqual)
//                    {
//                        differences.Add(property.Name);
//                    }
//                }
//                else
//                {
//                    // Create an expression to compare the two property values
//                    var equalsExpression = Expression.Equal(propertyValue1, propertyValue2);

//                    // Compile the expression to a lambda function and invoke it to compare the property values
//                    //var lambda = Expression.Lambda < F
//                }
//            }
//        }
//    }
//}

/*
using System;
using System.Reflection;

namespace SourceGenerator
{
    public class SetterLoggerGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            // Get all the types in the compilation
            var types = context.Compilation.SyntaxTrees
                .SelectMany(tree => tree.GetRoot().DescendantNodes())
                .OfType<ClassDeclarationSyntax>()
                .ToList();

            // Filter the types to only include those that implement the IGen interface
            var genTypes = types.Where(type => type.Implements("IGen"));

            // For each type, add a Console.WriteLine() to all its setters
            foreach (var type in genTypes)
            {
                // Get all the setters in the type
                var setters = type.DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .Where(property => property.AccessorList.Accessors
                        .Any(accessor => accessor.Kind() == SyntaxKind.SetAccessorDeclaration));

                // For each setter, add a Console.WriteLine() to the body of the setter
                foreach (var setter in setters)
                {
                    var statement = SyntaxFactory.ParseStatement(
                        "Console.WriteLine($\"Setter called: {type.Identifier.Text}.{setter.Identifier.Text}\");");
                    var newAccessor = setter.AccessorList.Accessors
                        .Single(accessor => accessor.Kind() == SyntaxKind.SetAccessorDeclaration)
                        .WithBody(SyntaxFactory.Block(statement));
                    var newProperty = setter.WithAccessorList(
                        setter.AccessorList.WithAccessors(
                            SyntaxFactory.List(new[] { newAccessor })));

                    context.AddCompilationUnit(type.SyntaxTree.FilePath, newProperty);
                }
            }
        }

        public void Initialize(InitializationContext context)
        {
            // No initialization required for this source generator
        }
    }
}

 */

/*
 To use this source generator, you would need to reference the Microsoft.CodeAnalysis.CSharp.dll and Microsoft.CodeAnalysis.CSharp.Syntax assemblies,
and add the SetterLoggerGenerator to the list of source generators in your project's csproj file:
<ItemGroup>
  <SourceGenerator Include="SetterLoggerGenerator" />
</ItemGroup>

This source generator uses the ISourceGenerator interface from the Microsoft.CodeAnalysis.CSharp.Syntax namespace,
which defines the methods that need to be implemented by a C# source generator.
It uses the SyntaxFactory class to create new syntax nodes for the added Console.WriteLine() statements,
and the Compilation and SyntaxTree classes to access and modify the syntax trees of the source code in the compilation.
 */