namespace app.IOAbstractions
{
    public class InputReader : IInputReader
    {
        public string? ReadLine() => Console.ReadLine();
    }
}