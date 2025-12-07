#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public struct ExpirationRange {
	public readonly long Highest;
	public readonly long Lowest;
	public readonly bool Obliterated;

	public ExpirationRange(long min, long max, bool obliterated = false) {
		this.Lowest = min;
		this.Highest = max;
		this.Obliterated = obliterated;
	}

	public readonly bool InRange(long comparing) {
		return comparing >= this.Lowest && comparing <= this.Highest;
	}

	public readonly long ItemsInRange() {
		return this.Obliterated ? 0 : Highest - Lowest + 1;
	}

	public readonly ExpirationRange TrimRange(ExpirationRange otherRange) {
		if ((otherRange.Lowest <= this.Lowest) && (otherRange.Highest >= this.Highest)) {return new ExpirationRange(0, 0, true);} // obliterated by other range
		if ((otherRange.Lowest <= this.Highest) && (otherRange.Highest >= this.Highest)) {return new ExpirationRange(this.Lowest, otherRange.Lowest - 1);} // trim down highest end
		if ((otherRange.Highest >= this.Lowest) && (otherRange.Lowest <= this.Lowest)) {return new ExpirationRange(otherRange.Highest + 1, this.Highest);} // trim down lowest end
		return new ExpirationRange(this.Lowest, this.Highest); // no intersection
	}
}

public class Parsed {
	public readonly ExpirationRange[] Ranges;
	public readonly long[] Items;

	private enum ParsingMode {
		Ranges,
		Items,
	}

	public Parsed(string[] lines) {
		List<ExpirationRange> addedRanges = [];
		List<long> addedItems = [];

		const string BLANK_LINE = "";
		const char RANGE_SPLITTER = '-';

		ParsingMode currentMode = ParsingMode.Ranges;

		foreach (string line in lines) {
			if (line == BLANK_LINE) {
				currentMode = ParsingMode.Items;
			} else if (currentMode == ParsingMode.Ranges) {
				string[] split = line.Split(RANGE_SPLITTER);
				addedRanges.Add(new ExpirationRange(long.Parse(split[0]), long.Parse(split[1])));
			} else {
				addedItems.Add(long.Parse(line));
			}
		}

		this.Ranges = addedRanges.ToArray<ExpirationRange>();
		this.Items = addedItems.ToArray<long>();
	}
}

public static bool IsItemExpired(long itemId, ExpirationRange[] ranges) {
	foreach (ExpirationRange checkingRange in ranges) {
		if (checkingRange.InRange(itemId)) {return true;}
	}
	return false;
}

public static int CountFresh(long[] items, ExpirationRange[] ranges) {
	int count = 0;
	foreach (long item in items) {
		count += IsItemExpired(item, ranges) ? 1 : 0;
	}
	return count;
}

public static long CountTotalPotential(ExpirationRange[] ranges) {
	long count = 0;
	ExpirationRange[] trimmedRanges = (ExpirationRange[])ranges.Clone();

	for (int myRangeIndex = 0; myRangeIndex < trimmedRanges.Length; myRangeIndex++) {
		ExpirationRange trimmedRange = trimmedRanges[myRangeIndex];
		for (int otherRangeIndex = 0; otherRangeIndex < trimmedRanges.Length; otherRangeIndex ++) {
			if (trimmedRange.Obliterated) {break;} // my range was obliterated last step

			ExpirationRange otherRange = trimmedRanges[otherRangeIndex];
			if (otherRange.Obliterated) {continue;} // other range was obliterated

			if (myRangeIndex == otherRangeIndex) {continue;} // don't compare against yourself

			trimmedRange = trimmedRange.TrimRange(otherRange);
		}
		trimmedRanges[myRangeIndex] = trimmedRange;
		if (trimmedRange.Obliterated) {continue;}
		count += trimmedRange.ItemsInRange();
	}
	return count;
}

Parsed myParsed = new(File.ReadAllLines(USING_PATH));
Console.WriteLine($"TOTAL POTENTIAL: {CountTotalPotential(myParsed.Ranges)}");
