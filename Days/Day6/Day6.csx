#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public const char SYMBOL_MULTIPLY = '*';
public const char SYMBOL_ADD = '+';

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

public class Parsed {
	public readonly InstructionSet[] InstructionSets;

	public Parsed(string[] lines) {
		const char ENTRY_SEPARATOR = ' ';

		int maxSizeX = lines[0].Split(ENTRY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Length;
		int maxSizeY = lines.Length;
		string[,] parsedGrid = new string[maxSizeX, maxSizeY];
		for (int y = 0; y < maxSizeY; y++) {
			string[] splitLine = lines[y].Split(ENTRY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
			for (int x = 0; x < maxSizeX; x++) {
				parsedGrid[x, y] = splitLine[x];
			}
		}

		List<InstructionSet> makingSetsList = [];

		for (int x = 0; x < maxSizeX; x++) {
			char instruction = parsedGrid[x, maxSizeY - 1][0];
			Debug.Assert(instruction == SYMBOL_MULTIPLY || instruction == SYMBOL_ADD, "Last entry must be a math symbol");
			long[] values = new long[maxSizeY - 1];
			for (int y = 0; y < maxSizeY - 1; y++) {
				values[y] = long.Parse(parsedGrid[x,y]);
			}

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
Console.WriteLine($"SUM: {SumAllInstructions(myParsed.InstructionSets)}");
