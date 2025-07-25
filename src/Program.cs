using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using Raylib_cs;

namespace Program
{


    class ConsoleHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;


        public static void HideConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        public static void ShowConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOW);
        }
    }

    class Program
    {


        static Dictionary<int, int> BuildLoopMap(List<string> instructions)
        {
            var loopMap = new Dictionary<int, int>();
            var stack = new Stack<int>();

            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i] == "[")
                {
                    stack.Push(i);
                }
                else if (instructions[i] == "]")
                {
                    if (stack.Count == 0)
                        throw new Exception($"Unmatched ] at instruction {i}");

                    int start = stack.Pop();
                    loopMap[start] = i;
                    loopMap[i] = start;
                }
            }

            if (stack.Count > 0)
                throw new Exception($"Unmatched [ at instruction {stack.Peek()}");

            return loopMap;
        }

        static List<string> Split(string code)
        {
            List<string> instructions = new List<string>();

            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];

                if (".,<>[]+-".Contains(c))
                {
                    instructions.Add(c.ToString());
                }
                else if (c == '^' && i + 1 < code.Length)
                {
                    string chunk = "^";
                    i++; // move to the next character
                    while (i < code.Length && code[i] != '^')
                    {
                        chunk += code[i];
                        i++;
                    }
                    chunk += "^"; // include closing ^
                    instructions.Add(chunk);
                }
                else if (c == '(' && i + 1 < code.Length)
                {
                    string chunk = "(";
                    i++;
                    while (i < code.Length && code[i] != ')')
                    {
                        chunk += code[i];
                        i++;
                    }
                    chunk += ")";
                    instructions.Add(chunk);
                }
                else if (c == '{' && i + 1 < code.Length)
                {
                    string chunk = "{";
                    i++;
                    while (i < code.Length && code[i] != '}')
                    {
                        chunk += code[i];
                        i++;
                    }
                    chunk += "}";
                    instructions.Add(chunk);
                }
            }

            return instructions;
        }

        static void StartWindowThread(byte[] tape)
        {
            Dictionary<int, Color> colorMap = new Dictionary<int, Color>()
            {
                [0] = Color.Black,
                [1] = Color.DarkGray,
                [2] = Color.Gray,
                [3] = Color.LightGray,
                [4] = Color.White,
                [5] = Color.Red,
                [6] = Color.Green,
                [7] = Color.Blue,
                [8] = Color.Yellow,
                [9] = Color.Orange,
                [10] = Color.Purple,
                [11] = Color.Maroon,
                [12] = Color.Lime,
                [13] = Color.SkyBlue,
                [14] = Color.Brown,
                [15] = Color.Pink
            };

            new Thread(() =>
            {
                int width = tape[251];
                int height = tape[252];

                Raylib.InitWindow(width, height, "BF++ Window");
                Raylib.SetTargetFPS(60);

                while (!Raylib.WindowShouldClose())
                {
                    Raylib.BeginDrawing();

                    if (tape[250] == 0 && Raylib.WindowShouldClose() == false && tape[79998] == 1)
                    {
                        Raylib.CloseWindow();
                        break;
                    }
                    else
                    {
                        Raylib.ClearBackground(Color.Black); // Optional: clear each frame
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int index = 253 + y * width + x;
                                byte val = tape[index];
                                if (colorMap.TryGetValue(val, out Color color))
                                    Raylib.DrawPixel(x, y, color);
                            }
                        }
                    }

                    Raylib.EndDrawing();
                }

            }).Start();
        }


        static void StartBigWindowThread(int[] tape)
        {
            Dictionary<int, Color> colorMap = new Dictionary<int, Color>()
            {
                [0] = Color.Black,
                [1] = Color.DarkGray,
                [2] = Color.Gray,
                [3] = Color.LightGray,
                [4] = Color.White,
                [5] = Color.Red,
                [6] = Color.Green,
                [7] = Color.Blue,
                [8] = Color.Yellow,
                [9] = Color.Orange,
                [10] = Color.Purple,
                [11] = Color.Maroon,
                [12] = Color.Lime,
                [13] = Color.SkyBlue,
                [14] = Color.Brown,
                [15] = Color.Pink
            };

            new Thread(() =>
            {
                int width = tape[251];
                int height = tape[252];

                Raylib.InitWindow(width, height, "BF++ Window");
                Raylib.SetTargetFPS(60);

                while (!Raylib.WindowShouldClose())
                {
                    Raylib.BeginDrawing();

                    if (tape[250] == 0 && Raylib.WindowShouldClose() == false && tape[79998] == 1)
                    {
                        Raylib.CloseWindow();
                        break;
                    }
                    else
                    {
                        Raylib.ClearBackground(Color.Black); // Optional: clear each frame
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int index = 253 + y * width + x;
                                int val = tape[index];
                                if (colorMap.TryGetValue(val, out Color color))
                                    Raylib.DrawPixel(x, y, color);
                            }
                        }
                    }

                    Raylib.EndDrawing();
                }

            }).Start();
        }

        public static class Global
        {
            public static int[] tape = new int[80000];
        }
        static void Main(string[] args)
        {
            var code = "{250}^1^>^16^>^1^>{253}^0^>^1^>^2^>^3^>^4^>^5^>^6^>^7^>^8^>^9^>^10^>^11^>^12^>^13^>^14^>^15^>{29999}^1^{40000}[{253}<++++++++[->+<]>[->+<]<++++++++>-]";

            if (File.Exists(args[0]) && args.Length > 0)
                code = File.ReadAllText(args[0]);

            var instructions = Split(code);
            Dictionary<int, int> loopMap = BuildLoopMap(instructions);
            byte[] tape = new byte[80000];
            int pointer = 0;
            int instructionPointer = 0;
            code = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            while (instructionPointer < instructions.Count)
            {
                if (instructions[instructionPointer] == ">")
                {
                    pointer++;
                    if (args.Length > 2 && args[2] == "debug")
                        Console.WriteLine("Going right");
                }
                else if (instructions[instructionPointer] == "<")
                {
                    pointer--;
                    if (args.Length > 2 && args[2] == "debug")
                        Console.WriteLine("Going left");

                }
                else if (instructions[instructionPointer] == "+")
                {
                    if (args.Length > 3 && args[3] == "big")
                    {
                        Global.tape[pointer]++;
                    }
                    else
                    {
                        tape[pointer]++;
                    }
                    if (args.Length > 2 && args[2] == "debug")
                        Console.WriteLine("Adding one");
                }
                else if (instructions[instructionPointer] == "-")
                {
                    if (args.Length > 3 && args[3] == "big")
                    {
                        Global.tape[pointer]--;
                    }
                    else
                    {
                        tape[pointer]--;
                    }
                    if (args.Length > 2 && args[2] == "debug")
                        Console.WriteLine("Subtracting one");
                }

                else if (instructions[instructionPointer] == ".")
                {
                    Console.Write((args[3] == "big") ? (char)Global.tape[pointer] : (char)tape[pointer]);
                }
                else if (instructions[instructionPointer] == ",")
                {
                    if (args.Length > 3 && args[3] == "big")
                    {
                        Global.tape[pointer] = (int)Console.Read();
                    }
                    else
                    {
                        tape[pointer] = (byte)Console.Read();
                    }
                }

                else if (instructions[instructionPointer][0] == '^')
                {
                    string val = "";
                    int pp = 1;
                    while (instructions[instructionPointer][pp] != '^')
                    {
                        val += instructions[instructionPointer][pp];
                        pp++;
                    }

                    if (args.Length > 3 && args[3] == "big")
                    {
                        if (int.TryParse(val, out int res))
                        {
                            Global.tape[pointer] = res;
                            if (args.Length > 2 && args[2] == "debug")
                                Console.WriteLine($"[big] Set value at {pointer} to {res}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid value in ^ ^: {val}");
                        }
                    }
                    else
                    {
                        if (byte.TryParse(val, out byte result))
                        {
                            tape[pointer] = result;
                            if (args.Length > 2 && args[2] == "debug")
                                Console.WriteLine($"[byte] Set value at {pointer} to {result}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid value in ^ ^: {val}");
                        }
                    }

                }
                else if (instructions[instructionPointer][0] == '{')
                {
                    string val = "";
                    int pp = 1;


                    while (instructions[instructionPointer][pp] != '}')
                    {
                        val += instructions[instructionPointer][pp];
                        pp++;
                    }

                    if (int.TryParse(val, out int result))
                    {
                        if (result < tape.Length) pointer = result;
                        if (args.Length > 2 && args[2] == "debug")
                            Console.WriteLine($"Jumping to {result}");
                    }
                }

                else if (instructions[instructionPointer][0] == '(')
                {
                    string val = "";
                    int pp = 1;


                    while (instructions[instructionPointer][pp] != ')')
                    {
                        val += instructions[instructionPointer][pp];
                        pp++;
                    }

                    if (int.TryParse(val, out int result))
                    {
                        if (result < instructions.Count) instructionPointer = result;
                    }
                }
                else if (instructions[instructionPointer] == "[")
                {
                    if (args[3] == "big" && Global.tape[pointer] == 0)
                    {
                        instructionPointer = loopMap[instructionPointer];
                    }
                    else if (tape[pointer] == 0)
                        instructionPointer = loopMap[instructionPointer];
                }
                else if (instructions[instructionPointer] == "]")
                {

                    if (args[3] == "big" && Global.tape[pointer] != 0)
                    {
                        instructionPointer = loopMap[instructionPointer];
                    }
                    else if (tape[pointer] != 0)
                        instructionPointer = loopMap[instructionPointer];
                }

                if (args.Length > 3 && args[3] == "big")
                {
                    if (Global.tape[79998] == 1)
                    {
                        if (Global.tape[250] == 1 && Global.tape[251] * Global.tape[252] == 0)
                        {
                            ConsoleHelper.HideConsole();
                            if (args.Length > 2 && args[2] == "debug")
                                Console.WriteLine("Hiding Console");
                        }


                        else if (Global.tape[250] == 1 && Global.tape[79999] == 0)
                        {
                            ConsoleHelper.HideConsole();
                            Global.tape[79999] = 1;
                            StartBigWindowThread(Global.tape);
                            if (args.Length > 2 && args[2] == "debug")
                                Console.WriteLine("Started window thread");

                        }

                        else if (Global.tape[250] == 0 && Global.tape[79998] == 1)
                        {
                            Global.tape[79999] = 0;
                            ConsoleHelper.ShowConsole();
                            if (args.Length > 2 && args[2] == "debug")
                                Console.WriteLine("Showed console");
                        }



                    }

                    if (Global.tape[79997] > 0)
                    {
                        Thread.Sleep(Global.tape[79997]);
                        if (args.Length > 2 && args[2] == "debug")
                            Console.WriteLine($"Slept for {Global.tape[79997]} ms");
                        Global.tape[79997] = 0;
                    }



                    if (Global.tape[249] == 1 && args[1] == "AC" && args.Length > 1)
                    {
                        StringBuilder command = new StringBuilder();

                        for (int i = 192; i <= 248; i++)
                        {
                            int b = Global.tape[i];
                            if (b >= 32 && b <= 126)
                            {
                                command.Append((char)b);
                            }


                        }
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();

                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = $"/C {command.ToString()}";
                        startInfo.UseShellExecute = false;
                        startInfo.CreateNoWindow = true;
                        startInfo.RedirectStandardError = true;
                        startInfo.RedirectStandardOutput = true;

                        process.StartInfo = startInfo;
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        string err = process.StandardError.ReadToEnd();
                        process.WaitForExit();
                        string combined = output + err;
                        int outputStart = 65279;
                        int outputLength = 64;
                        for (int i = 0; i < outputLength; i++)
                            Global.tape[outputStart + i] = 0;

                        byte[] outputBuf = System.Text.Encoding.ASCII.GetBytes(combined);

                        int lenToWrite = Math.Min(outputBuf.Length, outputLength);

                        for (int i = 0; i < lenToWrite; i++)
                        {
                            Global.tape[outputStart + i] = outputBuf[i];
                        }

                        Global.tape[249] = 0;

                    }


                    if (Global.tape[79996] == 1 && Global.tape[79998] == 1)
                    {
                        for (int i = 0; i < 79999; i++)
                            tape[i] = 0;
                    }
                    if (Global.tape[79995] == 1 && Global.tape[79998] == 1)
                    {
                        Environment.Exit(0);
                    }
                }


                if (tape[79998] == 1)
                {
                    if (tape[250] == 1 && tape[251] * tape[252] == 0)
                    {
                        ConsoleHelper.HideConsole();
                        if (args.Length > 2 && args[2] == "debug")
                            Console.WriteLine("Hiding Console");
                    }


                    else if (tape[250] == 1 && tape[79999] == 0)
                    {
                        ConsoleHelper.HideConsole();
                        tape[79999] = 1;
                        StartWindowThread(tape);
                        if (args.Length > 2 && args[2] == "debug")
                            Console.WriteLine("Started window thread");

                    }

                    else if (tape[250] == 0 && tape[79998] == 1)
                    {
                        tape[79999] = 0;
                        ConsoleHelper.ShowConsole();
                        if (args.Length > 2 && args[2] == "debug")
                            Console.WriteLine("Showed console");
                    }



                }

                if (tape[79997] > 0)
                {
                    Thread.Sleep(tape[79997]);
                    if (args.Length > 2 && args[2] == "debug")
                        Console.WriteLine($"Slept for {tape[79997]} ms");
                    tape[79997] = 0;
                }

                instructionPointer++;


                if (tape[249] == 1 && args[1] == "AC" && args.Length > 1)
                {
                    StringBuilder command = new StringBuilder();

                    for (int i = 192; i <= 248; i++)
                    {
                        byte b = tape[i];
                        if (b >= 32 && b <= 126)
                        {
                            command.Append((char)b);
                        }


                    }
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();

                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = $"/C {command.ToString()}";
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.RedirectStandardOutput = true;

                    process.StartInfo = startInfo;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string err = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    string combined = output + err;
                    int outputStart = 65279;
                    int outputLength = 64;
                    for (int i = 0; i < outputLength; i++)
                        tape[outputStart + i] = 0;

                    byte[] outputBuf = System.Text.Encoding.ASCII.GetBytes(combined);

                    int lenToWrite = Math.Min(outputBuf.Length, outputLength);

                    for (int i = 0; i < lenToWrite; i++)
                    {
                        tape[outputStart + i] = outputBuf[i];
                    }

                    tape[249] = 0;

                }


                if (tape[79996] == 1 && tape[79998] == 1)
                {
                    for (int i = 0; i < 79999; i++)
                        tape[i] = 0;
                }
                if (tape[79995] == 1 && tape[79998] == 1)
                {
                    Environment.Exit(0);
                }
            }

        }
    }
}
