using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

class ByDenisRafi
{
	#region Type Declarations
	class D
	{
		public int Left;
		public int Top;
	}
	class E
	{
		public int Left;
		public int Top;
		public int Frame;
	}
	class N
	{
		public int Left;
		public int Top;
		public int Frame;
	}
	class I
	{
		public int Frame;
		public int Left;
		public int Top;
		public int Health;
	}
	static class S
	{
		#region 
		public static string[] bulletRenders = new string[]
		{
			" ", 
			"-", 
			"~", 
			"█", 
		};
		public static readonly string[] helicopterRenders = new string[]
		{
			@"             " + '\n' +
			@"             " + '\n' +
			@"             ",			
			@"  ~~~~~+~~~~~" + '\n' +
			@"'\---<[D]R)  " + '\n' +
			@"     -'-`-   ",
			@"  -----+-----" + '\n' +
			@"*\---<[D]R)  " + '\n' +
			@"     -'-`-   ",
		};
		public static string[] ufoRenders = new string[]
		{
			@"   __*__   " + '\n' +
			@"-=<_‗_‗_>=-",
			@"     _!_     " + '\n' +
			@"    /_*_\    " + '\n' +
			@"-==<_‗_‗_>==-",
			@"  _/\_  " + '\n' +
			@" /_**_\ " + '\n' +
			@"() () ()",
			@" _!_!_ " + '\n' +
			@"|_o-o_|" + '\n' +
			@" ^^^^^ ",
			@" _!_ " + '\n' +
			@"(_o_)" + '\n' +
			@" ^^^ ",
		};
		public static readonly string[] explosionRenders = new string[]
		{
			@"           " + '\n' +
			@"   █████   " + '\n' +
			@"   █████   " + '\n' +
			@"   █████   " + '\n' +
			@"           ",
			@"           " + '\n' +
			@"           " + '\n' +
			@"     *     " + '\n' +
			@"           " + '\n' +
			@"           ",
			@"           " + '\n' +
			@"     *     " + '\n' +
			@"    *#*    " + '\n' +
			@"     *     " + '\n' +
			@"           ",
			@"           " + '\n' +
			@"    *#*    " + '\n' +
			@"   *#*#*   " + '\n' +
			@"    *#*    " + '\n' +
			@"           ",
			@"     *     " + '\n' +
			@"   *#*#*   " + '\n' +
			@"  *#* *#*  " + '\n' +
			@"   *#*#*   " + '\n' +
			@"     *     ",
			@"    *#*    " + '\n' +
			@"  *#* *#*  " + '\n' +
			@" *#*   *#* " + '\n' +
			@"  *#* *#*  " + '\n' +
			@"    *#*    ",
			@"   *   *   " + '\n' +
			@" **     ** " + '\n' +
			@"**       **" + '\n' +
			@" **     ** " + '\n' +
			@"   *   *   ",
			@"   *   *   " + '\n' +
			@" *       * " + '\n' +
			@"*         *" + '\n' +
			@" *       * " + '\n' +
			@"   *   *   ",
		};
		#endregion
	}
	#endregion
	static readonly int height = Console.WindowHeight;
	static readonly int width = Console.WindowWidth;
	static readonly TimeSpan threadSleepTimeSpan = TimeSpan.FromMilliseconds(10);
	static readonly TimeSpan helicopterTimeSpan = TimeSpan.FromMilliseconds(70);
	static readonly TimeSpan ufoMovementTimeSpan = TimeSpan.FromMilliseconds(100);
	static readonly TimeSpan enemySpawnTimeSpan = TimeSpan.FromSeconds(1.75);
	static readonly D player = new D { Left = 2, Top = height / 2, };
	static readonly List<I> ufos = new List<I>();
	static readonly List<E> bullets = new List<E>();
	static readonly List<N> explosions = new List<N>();
	static readonly Stopwatch stopwatchGame = new Stopwatch();
	static readonly Stopwatch stopwatchUFOSpawn = new Stopwatch();
	static readonly Stopwatch stopwatchHelicopter = new Stopwatch();
	static readonly Stopwatch stopwatchUFO = new Stopwatch();
	static readonly Random random = new Random();
	static int score = 0;
	static bool bulletFrame;
	static bool helicopterRender;

	static void Main()
	{
		Console.Title = "Get To The Chopper ";
		Console.Clear();
		Console.WindowWidth = 100;
		Console.WindowHeight = 30;
		Console.CursorVisible = false;
		stopwatchGame.Restart();
		stopwatchUFOSpawn.Restart();
		stopwatchHelicopter.Restart();
		stopwatchUFO.Restart();
		while (true)
		{
			#region Window Resize

			if (height != Console.WindowHeight || width != Console.WindowWidth)
			{
				Console.Clear();;
				return;
			}
			#endregion

			#region Update UFOs

			if (stopwatchUFOSpawn.Elapsed > enemySpawnTimeSpan)
			{
				ufos.Add(new I
				{
					Health = 4,
					Frame = random.Next(5),
					Top = random.Next(height - 3),
					Left = width,
				});
				stopwatchUFOSpawn.Restart();
			}

			if (stopwatchUFO.Elapsed > ufoMovementTimeSpan)
			{
				foreach (I ufo in ufos)
				{
					if (ufo.Left < width)
					{
						Console.SetCursorPosition(ufo.Left, ufo.Top);
						Erase(S.ufoRenders[ufo.Frame]);
					}
					ufo.Left--;
					if (ufo.Left <= 0)
					{
						Console.Clear();
						Console.Write("Score- " + score + ".");
						return;
					}
				}
				stopwatchUFO.Restart();
			}
			#endregion

			#region Update Player

			bool playerRenderRequired = false;
			if (Console.KeyAvailable)
			{
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.UpArrow:
						Console.SetCursorPosition(player.Left, player.Top);
						Render(S.helicopterRenders[default], true);
						player.Top = Math.Max(player.Top - 1, 0);
						playerRenderRequired = true;
						break;
					case ConsoleKey.DownArrow:
						Console.SetCursorPosition(player.Left, player.Top);
						Render(S.helicopterRenders[default], true);
						player.Top = Math.Min(player.Top + 1, height - 3);
						playerRenderRequired = true;
						break;
					case ConsoleKey.RightArrow:
						bullets.Add(new E
						{
							Left = player.Left + 11,
							Top = player.Top + 1,
							Frame = (bulletFrame = !bulletFrame) ? 1 : 2,
						});
						break;
					case ConsoleKey.Escape:
						Console.Clear();
						return;
				}
			}
			while (Console.KeyAvailable)
			{
				Console.ReadKey(true);
			}
			#endregion		
			#region 
			HashSet<E> bulletRemovals = new HashSet<E>();
			foreach (E bullet in bullets)
			{
				Console.SetCursorPosition(bullet.Left, bullet.Top);
				Console.Write(S.bulletRenders[default]);
				bullet.Left++;
				if (bullet.Left >= width || bullet.Frame is 3)
				{
					bulletRemovals.Add(bullet);
				}
				HashSet<I> ufoRemovals = new HashSet<I>();
				foreach (I ufo in ufos)
				{
					if (ufo.Left <= bullet.Left &&
						ufo.Top <= bullet.Top &&
						CollisionCheck(
						(S.bulletRenders[bullet.Frame], bullet.Left, bullet.Top),
						(S.ufoRenders[ufo.Frame], ufo.Left, ufo.Top)))
					{
						bullet.Frame = 3;
						ufo.Health--;
						if (ufo.Health <= 0)
						{
							score += 100;
							Console.SetCursorPosition(ufo.Left, ufo.Top);
							Erase(S.ufoRenders[ufo.Frame]);
							ufoRemovals.Add(ufo);
							explosions.Add(new N
							{
								Left = bullet.Left - 5,
								Top = Math.Max(bullet.Top - 2, 0),
							});
						}
					}
				}
				ufos.RemoveAll(ufoRemovals.Contains);
			}
			bullets.RemoveAll(bulletRemovals.Contains);
			#endregion
			#region Update & Render Explosions
			HashSet<N> explosionRemovals = new HashSet<N>();
			foreach (N explosion in explosions)
			{
				if (explosion.Frame > 0)
				{
					Console.SetCursorPosition(explosion.Left, explosion.Top);
					Erase(S.explosionRenders[explosion.Frame - 1]);
				}
				if (explosion.Frame <S.explosionRenders.Length)
				{
					Console.SetCursorPosition(explosion.Left, explosion.Top);
					Render(S.explosionRenders[explosion.Frame]);
				}
				explosion.Frame++;
				if (explosion.Frame > S.explosionRenders.Length)
				{
					explosionRemovals.Add(explosion);
				}
			}
			explosions.RemoveAll(explosionRemovals.Contains);
			#endregion
			#region Render Player
			if (stopwatchHelicopter.Elapsed > helicopterTimeSpan)
			{
				helicopterRender = !helicopterRender;
				stopwatchHelicopter.Restart();
				playerRenderRequired = true;
			}
			if (playerRenderRequired)
			{
				Console.SetCursorPosition(player.Left, player.Top);
				Render(S.helicopterRenders[helicopterRender ? 1 : 2]);
			}
			#endregion
			#region Render UFOs

			foreach (I ufo in ufos)
			{
				if (ufo.Left < width)
				{
					Console.SetCursorPosition(ufo.Left, ufo.Top);
					Render(S.ufoRenders[ufo.Frame]);
				}
			}

			#endregion

			#region Render Bullets

			foreach (E bullet in bullets)
			{
				Console.SetCursorPosition(bullet.Left, bullet.Top);
				Render(S.bulletRenders[bullet.Frame]);
			}

			#endregion

			Thread.Sleep(threadSleepTimeSpan);
		}
	}
	static void Render(string @string, bool renderSpace = false)
	{
		int x = Console.CursorLeft;
		int y = Console.CursorTop;
		foreach (char c in @string)
			if (c is '\n')
				Console.SetCursorPosition(x, ++y);
			else if (Console.CursorLeft < width - 1 && (!(c is ' ') || renderSpace))
				Console.Write(c);
			else if (Console.CursorLeft < width - 1 && Console.CursorTop < height - 1)
				Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
	}
	static void Erase(string @string)
	{
		int x = Console.CursorLeft;
		int y = Console.CursorTop;
		foreach (char c in @string)
			if (c is '\n')
				Console.SetCursorPosition(x, ++y);
			else if (Console.CursorLeft < width - 1 && !(c is ' '))
				Console.Write(' ');
			else if (Console.CursorLeft < width - 1 && Console.CursorTop < height - 1)
				Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
	}
	static bool CollisionCheck((string String, int Left, int Top) A, (string String, int Left, int Top) B)
	{
		char[,] buffer = new char[width, height];
		int left = A.Left;
		int top = A.Top;
		foreach (char c in A.String)
		{
			if (c is '\n')
			{
				left = A.Left;
				top++;
			}
			else if (left < width && top < height && c != ' ')
			{
				buffer[left++, top] = c;
			}
		}
		left = B.Left;
		top = B.Top;
		foreach (char c in B.String)
		{
			if (c is '\n')
			{
				left = A.Left;
				top++;
			}
			else if (left < width && top < height && c != ' ')
			{
				if (buffer[left, top] != default)
				{
					return true;
				}
				buffer[left++, top] = c;
			}
		}
		return false;
	}
}