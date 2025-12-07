#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public struct ExpirationRange {
	private long _lowest;
	private long _highest;

	public ExpirationRange(long min, long max) {
		this._lowest = min;
		this._highest = max;
	}

	public readonly bool InRange(long comparing) {
		return comparing >= this._lowest && comparing <= this._highest;
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

Parsed myParsed = new(File.ReadAllLines(USING_PATH));
Console.WriteLine($"TOTAL FRESH: {CountFresh(myParsed.Items, myParsed.Ranges)}")
