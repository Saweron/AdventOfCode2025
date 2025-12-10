#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public const char SYMBOL_MULTIPLY = '*';
public const char SYMBOL_ADD = '+';
public const char SYMBOL_BLANK = ' ';

public class InstructionSet {
	public readonly char Instruction;
	public readonly long[] Values;

	public InstructionSet(char instruction, long[] values) {
		this.Instruction = instruction;
		this.Values = values;
	}

	public long Evaluate() {
		const int FIRST_INDEX = 0;
		const int SECOND_INDEX = 1;

		long result = this.Values[FIRST_INDEX];
		if (this.Instruction == SYMBOL_MULTIPLY) {
			for (int i = SECOND_INDEX; i < this.Values.Length; i++) {result *= this.Values[i];}
		} else if (this.Instruction == SYMBOL_ADD) {
			for (int i = SECOND_INDEX; i < this.Values.Length; i++) {result += this.Values[i];}
		}
		return result;
	}
}

public static string[,] LinesToGrid(string[] lines) {
	const char ENTRY_SEPARATOR = SYMBOL_BLANK;

	// create grid array of values from raw text input
	int maxSizeX = lines[0].Split(ENTRY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Length;
	int maxSizeY = lines.Length;
	string[,] parsedGrid = new string[maxSizeX, maxSizeY];
	for (int y = 0; y < maxSizeY; y++) {
		string[] splitLine = lines[y].Split(ENTRY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
		for (int x = 0; x < maxSizeX; x++) {
			parsedGrid[x, y] = splitLine[x];
		}
	}
	return parsedGrid;
}

public static char SafeStringRead(string input, int index) {
	if (index > input.Length - 1) {return SYMBOL_BLANK;}
	return input[index];
}

public static string[,] LinesToGridAligned(string[] lines) {
	int maxCharsX = 0;
	foreach (string line in lines) {
		if (line.Length > maxCharsX) {maxCharsX = line.Length;}
	}
	// Console.WriteLine($"MAX LENGTH X: {maxCharsX}");
	// int maxCharsX = lines[0].Length;
	int maxSizeX = lines[0].Split(SYMBOL_BLANK, StringSplitOptions.RemoveEmptyEntries).Length;
	int maxSizeY = lines.Length;
	string[,] parsedGrid = new string[maxSizeX, maxSizeY];

	string[] columnBuffer = new string[maxSizeY];
	int columnIndex = 0;
	for (int i = 0; i < maxCharsX; i++) {
		bool allBlanks = true;
		for (int line = 0; line < maxSizeY; line++) {
			// Console.WriteLine($"{line} {i}");
			// char reading = lines[line][i];
			char reading = SafeStringRead(lines[line], i);
			// Console.WriteLine(reading);
			if (reading != SYMBOL_BLANK) {allBlanks = false;}
			// columnBuffer[line] += reading;

			// final (operation) line doesn't need whitespace alignment padding
			if (line != maxSizeY) {columnBuffer[line] = columnBuffer[line] + reading;}
		}
		// Console.WriteLine(allBlanks);

		if (allBlanks) {
			// Console.WriteLine("ALL BLANKS");
			for (int line = 0; line < maxSizeY; line++) {
				parsedGrid[columnIndex, line] = columnBuffer[line][..^1]; //remove the final blank appended on
				// Console.WriteLine(columnBuffer[line][..^1]);
			}
			columnBuffer = new string[maxSizeY];
			columnIndex += 1;
		}
	}

	for (int line = 0; line < maxSizeY; line++) {
		parsedGrid[columnIndex, line] = columnBuffer[line]; //remove the final blank appended on
		// Console.WriteLine("FINAL");
		// Console.WriteLine(columnBuffer[line]);
	}

	return parsedGrid;
}

// public static char? GetCharPadded(string input, int index, int longest) {
// 	Debug.Assert(input.Length < longest, "Input cannot be longer than longest parameter");
// 	return input[index];
// }

public static string[] TransposeEntries(string[] before) {
	int longestLength = 0;
	foreach (string line in before) {
		int length = line.Length;
		if (length > longestLength) {longestLength = length;}
	}

	string[] after = new string[longestLength];
	for (int characterIndex = 0; characterIndex < longestLength; characterIndex++) {
		string combinedWord = "";
		foreach (string line in before) {
			if (characterIndex > line.Length - 1) {continue;} // out of bounds
			combinedWord += line[characterIndex];
		}
		// Console.WriteLine($"TRANSPOSED {combinedWord}");
		after[characterIndex] = combinedWord;
	}

	return after;
}

public static string[] GetColumn(string[,] grid, int x, int endOffset = 1) {
	int maxSizeX = grid.GetLength(0);
	int maxSizeY = grid.GetLength(1);
	string[] values = new string[maxSizeY - 1];
	for (int y = 0; y < maxSizeY - endOffset; y++) {
		values[y] = grid[x, y];
	}
	return values;
}

public static long[] ToNumbers(string[] stringValues) {
	long[] converted = new long[stringValues.Length];
	for (int i = 0; i < stringValues.Length; i++) {
		converted[i] = long.Parse(stringValues[i]);
	}
	return converted;
}

public class Parsed {
	public readonly InstructionSet[] InstructionSets;

	public Parsed(string[] lines) {

		string[,] parsedGrid = LinesToGridAligned(lines);
		int maxSizeX = parsedGrid.GetLength(0);
		int maxSizeY = parsedGrid.GetLength(1);

		List<InstructionSet> makingSetsList = [];

		for (int x = 0; x < maxSizeX; x++) {
			// Console.WriteLine($"LINE: {parsedGrid[x, maxSizeY - 1]}");
			char instruction = parsedGrid[x, maxSizeY - 1][0];
			Debug.Assert(instruction == SYMBOL_MULTIPLY || instruction == SYMBOL_ADD, "Last entry must be a math symbol");
			long[] values = ToNumbers(TransposeEntries(GetColumn(parsedGrid, x)));

			makingSetsList.Add(new InstructionSet(instruction, values));
		}

		this.InstructionSets = makingSetsList.ToArray<InstructionSet>();
	}
}

public static long SumAllInstructions(InstructionSet[] sets) {
	long sum = 0;
	foreach (InstructionSet instructionSet in sets) {
		sum += instructionSet.Evaluate();
	}
	return sum;
}

Parsed myParsed = new(File.ReadAllLines(USING_PATH));
// string[,] myGrid = LinesToGridAligned(File.ReadAllLines(PATH_TO_EXAMPLE));

Console.WriteLine($"SUM: {SumAllInstructions(myParsed.InstructionSets)}");
