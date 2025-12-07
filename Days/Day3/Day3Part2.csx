#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public static long LargestNumber(string cells, int joiningAmount = 12) {
	Debug.Assert(cells.Length > joiningAmount, "Cells length is too short");

	string result = "";
	int lastIndex = -1;

	for (int i = joiningAmount - 1; i >= 0; i--) {
		int largest = 0;
		for (int j = lastIndex + 1; j < cells.Length - i; j++) {
			int item = int.Parse(cells[j].ToString());
			if (item > largest) {
				largest = item;
				lastIndex = j;
			}

			if (item == 9) {
				// lastIndex += 1;
				break;
			}
		}
		result += largest.ToString();
	}

	return long.Parse(result);
}

public static long SumLines(string[] lines) {
	long sum = 0;
	foreach (string line in lines) {
		sum += LargestNumber(line);
	}
	return sum;
}

// Console.WriteLine(LargestNumber("811111111111119"));
// Console.WriteLine(LargestNumber("234234234234278"));
Console.WriteLine(SumLines(File.ReadAllLines(USING_PATH)));
