#include <iostream>
#include <vector>
#include <memory>


using Value = int;
using IndexDiff = int;

class Table2D
{
private:
	std::vector<Value> table;

public:
	const size_t R;
	const size_t C;

	Table2D(size_t R, size_t C, Value val) : R(R), C(C), table(R * C, val) {}

	Table2D(size_t R, size_t C) : Table2D{ R, C, 0 } {}

	Value& operator() (size_t r, size_t c) { return table[r * C + c]; }
};

struct Shift2D
{
	IndexDiff r;
	IndexDiff c;
	Shift2D operator-() { return Shift2D{ -this->r, -this->c }; }
	Shift2D operator+(Shift2D other) { return Shift2D{ this->r + other.r, this->c + other.c }; }
	Shift2D operator-(Shift2D other) { return Shift2D{ this->r - other.r, this->c - other.c }; }
};

constexpr Shift2D operator"" _r(unsigned long long r) { return Shift2D{ static_cast<IndexDiff>(r), 0 }; }
constexpr Shift2D operator"" _c(unsigned long long c) { return Shift2D{ 0, static_cast<IndexDiff>(c) }; }
constexpr Shift2D operator"" _rc(unsigned long long rc) { return Shift2D{ static_cast<IndexDiff>(rc), static_cast<IndexDiff>(rc) }; }

struct Cell2D
{
	Table2D& table;
	size_t r;
	size_t c;

	Cell2D(Table2D& table, size_t r, size_t c) : table(table), r(r), c(c) {}

	operator Value() { return table(r, c); }

	Value operator=(Value value) { return table(r, c) = value; }
	Value operator+=(Value value) { return table(r, c) += value; }
	Value operator-=(Value value) { return table(r, c) -= value; }

	void operator+=(Shift2D shift) { r += shift.r; c += shift.c; }
	void operator-=(Shift2D shift) { r -= shift.r; c -= shift.c; }

	bool IsInside() { return r >= 0 && c >= 0 && r < table.R && c < table.C; }
};

using operation = void(*)(Cell2D&, Value);
operation placeValue = [](Cell2D& cell, Value value) {cell = value; };
operation addValue = [](Cell2D& cell, Value value) {cell += value; };

class Move
{
private:
	Cell2D cell;
	Value previous_value;

public:
	Move(operation& op, Cell2D cell, Value value) : cell(cell), previous_value(cell) { op(cell, value); }
	Move(const Move&) = delete;
	~Move(){ cell = previous_value; }
};

class LineMove
{
private:
	std::vector<std::unique_ptr<Move>> moves;

public:
	LineMove(operation& op, Cell2D cell, Value value, Shift2D shift)
	{
		for (; cell.IsInside(); cell += shift)
		{
			moves.emplace_back(new Move(op, cell, value));
		}
	}
};

class CrossMove
{
private:
	LineMove right;
	LineMove left;
	LineMove up;
	LineMove down;

public:
	CrossMove(operation& op, Cell2D cell, Value value) :
		right(op, cell, value, 1_c),
		left(op, cell, value, -1_c),
		up(op, cell, value, -1_r),
		down(op, cell, value, 1_r)
	{}
};

class DiagonalCrossMove
{
private:
	//All rotated by 45 degrees.
	LineMove right;
	LineMove left;
	LineMove up;
	LineMove down;

public:
	DiagonalCrossMove(operation& op, Cell2D cell, Value value) :
		right(op, cell, value, 1_c - 1_r),
		left(op, cell, value,  1_r - 1_c),
		up(op, cell, value, -1_rc),
		down(op, cell, value, 1_rc)
	{}
};


void NaiveNQueens(Table2D& table, int placed, int& acc)
{
	if (placed == table.R) { acc++; return; }
	for (size_t i = 0; i < table.C; i++)
	{
		if (table(placed, i) != 'Q')
		{
			CrossMove m1(placeValue, Cell2D(table, placed, i), 'Q');
			DiagonalCrossMove m2(placeValue, Cell2D(table, placed, i), 'Q');
			NaiveNQueens(table, placed + 1, acc);
		}
	}
}

class BlockedLineMove
{
private:
	bool is_valid;
	std::vector<std::unique_ptr<Move>> moves;

public:
	BlockedLineMove(operation& op, Cell2D cell, Value value, Value empty, Value danger, Shift2D shift) : is_valid(true)
	{
		for (; cell.IsInside(); cell += shift)
		{
			if (cell == danger) is_valid = false;
			if (cell != empty) break;
			moves.emplace_back(new Move(op, cell, value));
		}

	}

	bool IsValid() { return is_valid; }
};

bool Bacteria2(Table2D& table, bool Becca)
{
	for (size_t i = 0; i < table.R; i++)
	{
		for (size_t j = 0; j < table.C; j++)
		{
			if (table(i, j) == '.')
			{
				{
					BlockedLineMove h1(placeValue, Cell2D(table, i, j), '1', '.', '#', 1_c);
					BlockedLineMove h2(placeValue, Cell2D(table, i, j - 1), '1', '.', '#', -1_c);
					if (h1.IsValid() && h2.IsValid() && Bacteria2(table, !Becca) == Becca) return Becca;
				}
				{
					BlockedLineMove v1(placeValue, Cell2D(table, i, j), '1', '.', '#', 1_r);
					BlockedLineMove v2(placeValue, Cell2D(table, i - 1, j), '1', '.', '#', -1_r);
					if (v1.IsValid() && v2.IsValid() && Bacteria2(table, !Becca) == Becca) return Becca;
				}
			}
		}
	}
	return !Becca;
}

int Bacteria1(Table2D& table)
{
	int count = 0;
	for (size_t i = 0; i < table.R; i++)
	{
		for (size_t j = 0; j < table.C; j++)
		{
			if (table(i, j) == '.')
			{
				{
					BlockedLineMove h1(placeValue, Cell2D(table, i, j), '1', '.', '#', 1_c);
					BlockedLineMove h2(placeValue, Cell2D(table, i, j - 1), '1', '.', '#', -1_c);
					if (h1.IsValid() && h2.IsValid() && Bacteria2(table, false)) count++;
				}
				{
					BlockedLineMove v1(placeValue, Cell2D(table, i, j), '1', '.', '#', 1_r);
					BlockedLineMove v2(placeValue, Cell2D(table, i - 1, j), '1', '.', '#', -1_r);
					if (v1.IsValid() && v2.IsValid() && Bacteria2(table, false)) count++;
				}
			}
		}
	}
	return count;
}

int main()
{
	/*int N = 7;
	Table2D table(N, N);
	int count = 0;
	NaiveNQueens(table, 0, count);
	std::cout << count;*/
	int T;
	std::cin >> T;
	for (size_t i = 0; i < T; i++)
	{
		int R, C;
		std::cin >> R >> C;
		Table2D table(R, C);
		char ch;
		for (size_t r = 0; r < R; r++)
		{
			for (size_t c = 0; c < C; c++)
			{
				std::cin >> ch;
				table(r, c) = ch;
			}
		}
		std::cout << "Case #" << i + 1 << ": " << Bacteria1(table) << std::endl;
	}
	return 0;
}
