﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameMaker;

namespace DrawingTests
{
	static class Program
	{
		static void Main(string[] args)
		{
			Game.Run(args, new Room(1024, 768), 60, gameStart);
		}

		static void gameStart()
		{
			GlobalEvent.ExitOnEscape = true;

			new Drawer();
			new Block(0, 0);

			Window.Title = "Hello, world!";
		}
	}
}
