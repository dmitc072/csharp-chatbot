public class RulesBrain : IChatBrain
{
    private readonly RuleEngine _rules = new RuleEngine();
    public Task<string> GetReplyAsync(string input) => Task.FromResult(_rules.GetReply(input));
}
public class RuleEngine
{
    public RuleEngine() { }

    public string GetReply(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "Say something or type /help.";
        var t = input.ToLowerInvariant();

        string? TryCalc(string s, char op)
        {
            var parts = s.Split(op);
            if (parts.Length != 2) return null;
            if (!int.TryParse(parts[0].Trim(), out var left)) return null;
            if (!int.TryParse(parts[1].Trim(), out var right)) return null;
            var result = op == '+' ? left + right : op == '*' ? left * right : left - right;
            return $"That equals {result}.";
        }

        return t switch
        {
            _ when t.Contains("hello") || t.Contains("hi") || t.Contains("hey")
                => "Hi! I'm your C# chatbot.",
            _ when t.Contains("how are you")
                => "I'm just code, but I'm doing great!",
            _ when t.Contains("time")
                => $"It's {DateTime.Now:t}.",
            _ when t.Contains("date") || t.Contains("day")
                => $"It's {DateTime.Now:dddd, MMMM d, yyyy}.",
            _ when t.Contains("bye")
                => "Goodbye! Talk to you later.",
            _ when t.Contains("thank")
                => "You're welcome!",
            _ when t.Contains("your name")
                => "I'm Chatbot, written in C#.",
            _ when t.Contains("joke")
                => "Why do programmers prefer dark mode? Because light attracts bugs!",
            _ when t.Contains("weather")
                => "I can't check the weather yet, but I hope it's nice where you are!",
            _ when t.Contains("+") => TryCalc(t, '+') ?? "I can only do simple math like 2+2.",
            _ when t.Contains("*") => TryCalc(t, '*') ?? "I can only do simple math like 5*3.",
            _ when t.Contains("-") => TryCalc(t, '-') ?? "I can only do simple math like 7-4.",
            "/help" or "help" => "Commands: /help, /reset, exit",
            "/reset" or "reset" => "(nothing to reset yet)",
            _ => "I don't know that yet. Try /help."
        };
    }

}
