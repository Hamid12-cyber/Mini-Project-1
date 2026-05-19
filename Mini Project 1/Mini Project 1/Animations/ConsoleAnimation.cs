using System;
using System.Threading;

namespace Mini_Project_1.Animations
{
    internal static class ConsoleAnimation
    {
        private static readonly ConsoleColor Primary = ConsoleColor.Red;
        private static readonly ConsoleColor Second = ConsoleColor.Yellow;
        private static readonly ConsoleColor Bright = ConsoleColor.White;
        private static readonly ConsoleColor Dim = ConsoleColor.DarkRed;

        // ── 1. SPLASH SCREEN ──────────────────────────────────────────────
        public static void SplashScreen(string title = "MINI SHOP")
        {
            Console.Clear();
            Console.CursorVisible = false;

            int w = Console.WindowWidth;
            int cx = w / 2;
            int cy = Console.WindowHeight / 2;

            // Sürətli skan
            Console.ForegroundColor = Dim;
            for (int x = 0; x < w; x += 3)
            {
                SafeSetCursor(x, cy);
                Console.Write('·');
                Thread.Sleep(2);
            }
            Thread.Sleep(60);
            Console.Clear();

            // Başlıq çərçivəsi
            string border = new string('═', title.Length + 6);
            int bx = cx - (border.Length + 2) / 2;
            int by = cy - 1;

            for (int f = 0; f < 4; f++)
            {
                Console.ForegroundColor = f % 2 == 0 ? Primary : Dim;
                SafeSetCursor(bx, by); Console.Write($"╔{border}╗");
                SafeSetCursor(bx, by + 1); Console.Write($"║   {title}   ║");
                SafeSetCursor(bx, by + 2); Console.Write($"╚{border}╝");
                Thread.Sleep(60);
            }

            // Final
            Console.ForegroundColor = Primary;
            SafeSetCursor(bx, by); Console.Write($"╔{border}╗");
            SafeSetCursor(bx, by + 1);
            Console.Write("║   ");
            Console.ForegroundColor = Bright;
            Console.Write(title);
            Console.ForegroundColor = Primary;
            Console.Write("   ║");
            SafeSetCursor(bx, by + 2); Console.Write($"╚{border}╝");

            // Alt xətt
            for (int len = 0; len <= title.Length + 8; len++)
            {
                SafeSetCursor(cx - len / 2, by + 4);
                Console.ForegroundColor = Second;
                Console.Write(new string('─', len));
                Thread.Sleep(12);
            }

            Thread.Sleep(400);
            Console.Clear();
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        // ── 2. LOADING — sürətli, 400ms ──────────────────────────────────
        public static void Loading(string message = "Yüklənir", int durationMs = 400)
        {
            Console.CursorVisible = false;
            int barLen = Math.Min(Console.WindowWidth - 6, 44);
            long end = Environment.TickCount64 + durationMs;
            int frame = 0;

            while (Environment.TickCount64 < end)
            {
                double pct = 1.0 - (double)(end - Environment.TickCount64) / durationMs;
                int filled = (int)(pct * barLen);

                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = Second;
                Console.Write(frame % 2 == 0 ? "  ▰  " : "  ▱  ");
                Console.ForegroundColor = Bright;
                Console.Write(message + "          ");

                Console.SetCursorPosition(0, 1);
                Console.ForegroundColor = Dim;
                Console.Write("  [");
                Console.ForegroundColor = Primary;
                Console.Write(new string('█', Math.Max(0, filled)));
                Console.ForegroundColor = Dim;
                Console.Write(new string('░', Math.Max(0, barLen - filled)));
                Console.Write("]  ");

                frame++;
                Thread.Sleep(30);
            }

            Console.SetCursorPosition(0, 0); Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, 1); Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
            Console.ResetColor();
        }

        // ── 3. TYPEWRITE — yalnız lazım olduqda ──────────────────────────
        public static void TypeWrite(string text, ConsoleColor color = default, int delayMs = 0)
        {
            if (color == default) color = Bright;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void TypeWriteLine(string text, ConsoleColor color = default, int delayMs = 0)
        {
            TypeWrite(text, color, delayMs);
            Console.WriteLine();
        }

        // ── 4. MENÜ — hamısı birbaşa, delay yox ─────────────────────────
        public static void PrintMenu(string title, string[] items)
        {
            Console.Clear();

            string border = new string('─', title.Length + 4);
            Console.ForegroundColor = Dim;
            Console.WriteLine($"\n  ┌{border}┐");
            Console.Write("  │  ");
            Console.ForegroundColor = Bright;
            Console.Write(title);
            Console.ForegroundColor = Dim;
            Console.WriteLine("  │");
            Console.WriteLine($"  └{border}┘\n");

            foreach (string item in items)
            {
                Console.ForegroundColor = Second;   // › — sarı
                Console.Write("  › ");
                Console.ForegroundColor = Bright;   // mətn — ağ
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.ForegroundColor = Primary;      // » — qırmızı
            Console.Write("  » ");
            Console.ForegroundColor = Bright;
            Console.Write("Seçim: ");
            Console.ResetColor();
        }

        // ── 5. RECEIPT ────────────────────────────────────────────────────
        public static void PrintReceiptAnimated(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("=") || line.StartsWith("-"))
                {
                    Console.ForegroundColor = Primary;   // xətt — qırmızı
                }
                else if (line.Contains("TOTAL"))
                {
                    Console.ForegroundColor = Bright;    // total — ağ
                }
                else if (line.Contains("Discount"))
                {
                    Console.ForegroundColor = Second;    // endirim — sarı
                }
                else if (line.Contains("Order No") || line.Contains("STATUS"))
                {
                    Console.ForegroundColor = Second;    // başlıq sahələri — sarı
                }
                else
                {
                    Console.ForegroundColor = Bright;    // normal — ağ
                }
                Console.WriteLine(line);
            }
            Console.ResetColor();
        }

        // ── 6. MESAJLAR ───────────────────────────────────────────────────
        public static void Success(string message)
        {
            Console.ForegroundColor = Second;
            Console.WriteLine($"\n  ✓ {message}");
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = Primary;
            Console.WriteLine($"\n  ✗ {message}");
            Console.ResetColor();
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = Second;
            Console.WriteLine($"\n  ⚠ {message}");
            Console.ResetColor();
        }

        // ── KÖMƏKÇI ───────────────────────────────────────────────────────
        private static void SafeSetCursor(int x, int y)
        {
            try
            {
                x = Math.Max(0, Math.Min(x, Console.WindowWidth - 1));
                y = Math.Max(0, Math.Min(y, Console.WindowHeight - 1));
                Console.SetCursorPosition(x, y);
            }
            catch { }
        }
    }
}
