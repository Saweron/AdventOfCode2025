#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public static bool[,] ParseInput(string[] lines) {
	// const char BLANK_CELL = '.';
	const char FULL_CELL = '@';

	bool[,] grid = new bool[lines[0].Length, lines.Length];
	Console.WriteLine($"WIDTH: {lines[0].Length} HEIGHT: {lines.Length}");
	int indexX = 0;
	int indexY = 0;
	foreach (string line in lines) {
		indexX = 0;
		foreach (char character in line) {
			grid[indexX, indexY] = (character == FULL_CELL);
			indexX++;
		}
		indexY++;
	}
	return grid;
}

public static bool GetCell(bool[,] grid, int x, int y) {
	if (x < 0) {return false;}
	if (y < 0) {return false;}
	if (x >= grid.GetLength(0)) {return false;}
	if (y >= grid.GetLength(1)) {return false;}
	return grid[x, y];
}

public static bool CheckCell(bool[,] grid, int x, int y) {
	if (!GetCell(grid, x, y)) {return false;} // if cell is empty, don't bother counting adjascent
	const int THRESHOLD = 4;

	const int TOP = 1;
	const int BOTTOM = -1;
	const int LEFT = -1;
	const int RIGHT = 1;
	const int NONE = 0;

	int total = 0;
	total += GetCell(grid, x + RIGHT, y + NONE)   ? 1 : 0; // right
	total += GetCell(grid, x + RIGHT, y + TOP)    ? 1 : 0; // top right
	total += GetCell(grid, x + NONE,  y + TOP)    ? 1 : 0; // top
	total += GetCell(grid, x + LEFT,  y + TOP)    ? 1 : 0; // top left
	total += GetCell(grid, x + LEFT,  y + NONE)   ? 1 : 0; // left
	total += GetCell(grid, x + LEFT,  y + BOTTOM) ? 1 : 0; // bottom left
	total += GetCell(grid, x + NONE,  y + BOTTOM) ? 1 : 0; // bottom
	total += GetCell(grid, x + RIGHT, y + BOTTOM) ? 1 : 0; // bottom right
	return total < THRESHOLD;
}

public static int CheckAllCells(bool[,] grid) {
	int sizeX = grid.GetLength(0);
	int sizeY = grid.GetLength(1);
	int total = 0;

	for (int x = 0; x < sizeX; x++) {
		for (int y = 0; y < sizeY; y++) {
			total += CheckCell(grid, x, y) ? 1 : 0;
		}
	}

	return total;
}

bool[,] myGrid = ParseInput(File.ReadAllLines(USING_PATH));
Console.WriteLine($"TOTAL: {CheckAllCells(myGrid)}")
