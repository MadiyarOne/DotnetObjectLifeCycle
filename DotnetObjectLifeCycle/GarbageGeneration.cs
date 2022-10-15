using System.Text;
using static System.Console;

class GarbageGeneration
{
    static void Main(string[] args)
    {
        OutputEncoding = Encoding.Unicode;
        WriteLine("GC demonstration🚮");
        WriteLine($"Max generation: {GC.MaxGeneration}");
        GarbageHelper hlp = new GarbageHelper();
        WriteLine($"Object generation: {GC.GetGeneration(hlp)}");
        WriteLine($"Memory usage byte: {GC.GetTotalMemory(false)}");
        hlp.MakeGarbage(); 
        WriteLine($"Memory usage byte: {GC.GetTotalMemory(false)}");
        WriteLine($"Memory usage byte: {GC.GetTotalMemory(false)}");
        WriteLine($"Object generation: {GC.GetGeneration(hlp)}");
    
        GC.Collect();
        WriteLine($"Memory usage byte: {GC.GetTotalMemory(false)}");
        WriteLine($"Object generation: {GC.GetGeneration(hlp)}");
        ReadKey();
    }
}






class GarbageHelper
{
    public void MakeGarbage()
    {
        for (int i = 0; i < 100; i++)
        {
            Person p = new Person();
        }
    }

    class Person
    {
        private string _name;
        private string surname;
        private byte age;
    }
}