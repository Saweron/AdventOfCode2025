#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public struct InputRange {
	public readonly long Min;
	public readonly long Max;

	public InputRange(long min, long max) {
		this.Min = min;
		this.Max = max;
	}
}

public static InputRange[] ParseInput(string[] lines) {
	const char ENTRY_SPLITTER = ',';
	const char RANGE_SPLITTER = '-';

	List<InputRange> ranges = [];
	foreach (string line in lines) {
		string[] lineEntries = line.Split(ENTRY_SPLITTER, StringSplitOptions.RemoveEmptyEntries);
		foreach (string entry in lineEntries) {
			string[] splitRange = entry.Split(RANGE_SPLITTER, StringSplitOptions.RemoveEmptyEntries);
			InputRange createdRange = new InputRange(long.Parse(splitRange[0]), long.Parse(splitRange[1]));
			ranges.Add(createdRange);
		}
	}
	return ranges.ToArray<InputRange>();
}

public static bool IsIdValid(string id) {
	int length = id.Length;

	for (int dividingBy = 2; dividingBy <= length; dividingBy ++) {
		if (length % dividingBy != 0) {continue;}

		int divided = length / dividingBy;
		string firstChunk = id[..divided];
		bool passedCheck = true;

		for (int chunk = divided; chunk < length; chunk += divided) {
			string comparingChunk = id[chunk..(chunk + divided)];
			if (comparingChunk != firstChunk) {passedCheck = false; break;}
		}

		if (passedCheck) {return false;}
	}

	return true;
}

public static long AddRange(InputRange inputRange) {
	long total = 0;
	for (long id = inputRange.Min; id <= inputRange.Max; id++) {
		total += IsIdValid(Convert.ToString(id)) ? 0 : id;
	}
	return total;
}

InputRange[] myRanges = ParseInput(File.ReadAllLines(USING_PATH));
long total = 0;
foreach (InputRange range in myRanges) {
	total += AddRange(range);
}
Console.WriteLine(total);
