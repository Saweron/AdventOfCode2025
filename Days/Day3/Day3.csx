#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public static int LargestNumber(string cells) {
	Debug.Assert(cells.Length > 1, "Cells length is too short");

	int largestFirst = 0;
	int largestFirstIndex = 0;
	int largestSecond = 0;

	for (int i = 0; i < cells.Length - 1; i++) {
		int item = int.Parse(cells[i].ToString());
		if (item > largestFirst) {
			largestFirst = item;
			largestFirstIndex = i;
		}

		if (item == 9) {break;}
	}

	for (int i = largestFirstIndex + 1; i < cells.Length; i++) {
		int item = int.Parse(cells[i].ToString());
		if (item > largestSecond) {
			largestSecond = item;
		}

		if (item == 9) {break;}
	}

	return int.Parse(largestFirst.ToString() + largestSecond.ToString());
}

public static int SumLines(string[] lines) {
	int sum = 0;
	foreach (string line in lines) {
		sum += LargestNumber(line);
	}
	return sum;
}

Console.WriteLine(SumLines(File.ReadAllLines(USING_PATH)));
