using System;
using System.IO;
using System.Threading.Tasks;

public interface IChatBrain { Task<string> GetReplyAsync(string input); }

class Program
{
    // Async because LLM calls are async; rules mode still works fine.
    static async Task Main(string[] args)
    {

        DotNetEnv.Env.Load(); // Load .env file first thing

        Directory.CreateDirectory("./transcripts");                                  // ensure folder exists, if not, creates one
        var path = $"./transcripts/chat-{DateTime.Now:yyyyMMdd-HHmmss}.txt";         // safe, unique filename
        using var writer = new StreamWriter(path, append: false);                    // open transcript once
        writer.WriteLine($"[{DateTime.Now:HH:mm}] system > session started");        // start marker

        var mode = (Environment.GetEnvironmentVariable("CHAT_MODE") ?? "OFFLINE");   // OFFLINE or LLM
        Console.WriteLine($"[system] Chatbot starting in {mode} mode.");
        Console.WriteLine("Welcome to Chatbot! Type 'exit' to quit.");

        // Choose the brain: LLM if configured, otherwise rules.
        IChatBrain brain = mode.Equals("LLM", StringComparison.OrdinalIgnoreCase)
            ? new LlmBrain()                                                         // requires your LlmBrain class
            : new RulesBrain();                                                      // offline rules fallback

        while (true)
        {
            Console.Write($"[{DateTime.Now:HH:mm}] you > ");                         // console prompt
            var input = Console.ReadLine();

            if (input?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true)   // uniform exit
            {
                writer.WriteLine($"[{DateTime.Now:HH:mm}] system > session ended");
                break;
            }

            writer.WriteLine($"[{DateTime.Now:HH:mm}] you > {input}");               // log user line
            var reply = await brain.GetReplyAsync(input ?? string.Empty);            // ask the brain
            writer.WriteLine($"[{DateTime.Now:HH:mm}] bot > {reply}");               // log bot line
            Console.WriteLine($"[{DateTime.Now:HH:mm}] bot > {reply}");              // show bot line
        }
    }
}
