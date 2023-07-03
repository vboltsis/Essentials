using Types;

int number = 1;
int? nullable = null;
DateTime date = new DateTime(1111, 1, 1);
string text = "1";
List<int> list = new List<int> { 1, 1 };
Person person = new Person { Name = "1", Age = 1 };
byte[] bytes = new byte[4];
PersonStruct personStruct = new PersonStruct { Name = "1", Age = 1 };

ByValueOrByReference.ByValue(number, nullable, date, text, list, person, bytes, personStruct);
ByValueOrByReference.ByReference(ref number, ref nullable, ref date, ref text, ref list, ref person, ref bytes, ref personStruct);

Console.WriteLine();
