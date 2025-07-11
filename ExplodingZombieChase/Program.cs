// See https://aka.ms/new-console-template for more information
using ExplodingZombieChase;

Console.ResetColor();
Console.WriteLine("Hello, World!");

Grid game = new Grid(14, 18, .1, .1);
game.DisplayGrid();