#r "System.Console"
using System;

const string PATH_TO_EXAMPLE = "./Example.txt";
const string PATH_TO_INPUT = "./Input.txt";
const string USING_PATH = PATH_TO_INPUT;

public class State {
	private int _currentPosition;
	private int _clicksCounted;
	private readonly int _wrapMax;
	private readonly int _wrapMin;
	private readonly int _wrapRange;

	public State(
		int WrapMinValue = 0,
		int WrapMaxValue = 99,
		int InitialPosition = 50
	) {
		Debug.Assert(WrapMinValue < WrapMaxValue, "Min value cannot be more than max value");

		_currentPosition = InitialPosition;
		_clicksCounted = 0;
		_wrapMin = WrapMinValue;
		_wrapMax = WrapMaxValue;
		_wrapRange = WrapMaxValue - WrapMinValue + 1;
	}

	public void MoveBy(int Amount) {
		int AmountWrapped = Amount % _wrapRange;
		int ExtraTimesWrapped = Math.Abs((Amount - AmountWrapped) / _wrapRange);

		bool notWasOnZero = _currentPosition != 0;

		_currentPosition += AmountWrapped;
		if (_currentPosition < _wrapMin) {
			_currentPosition += _wrapRange;
			if (notWasOnZero) {_clicksCounted += 1;}
		} else if (_currentPosition > _wrapMax) {
			_currentPosition -= _wrapRange;
			if (notWasOnZero) {_clicksCounted += 1;}
		} else if (_currentPosition == 0) {
			_clicksCounted += 1;
		}

		_clicksCounted += ExtraTimesWrapped;
	}

	public int GetPosition() {return _currentPosition;}
	public int GetClicksCount() {return _clicksCounted;}
}

public static void ParseInstruction(string Instruction, State state) {
	const char RIGHT_IDENTIFIER = 'R';
	const char LEFT_IDENTIFIER = 'L';

	char identifier = Instruction[0];

	Debug.Assert(
		identifier == RIGHT_IDENTIFIER || identifier == LEFT_IDENTIFIER,
		"First character is not an instruction identifier"
	);

	int moveByAmount = int.Parse(Instruction[1..]) * (identifier == RIGHT_IDENTIFIER ? 1 : -1);
	state.MoveBy(moveByAmount);
}

State MyState = new();

var lines = File.ReadAllLines(USING_PATH);
foreach (var line in lines) {
	ParseInstruction(line, MyState);
}

Console.WriteLine("ZEROES COUNTED:");
Console.WriteLine(MyState.GetClicksCount());
