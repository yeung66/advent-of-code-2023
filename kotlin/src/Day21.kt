fun main() {
    fun parse(input: String): Triple<List<MutableList<Char>>, Int, Int> {
        var (x, y) = 0 to 0
        val board = input.lines().mapIndexed { i, it ->
            it.toCharArray().mapIndexed { j, c ->
                if (c == 'S') {
                    x = i
                    y = j
                    '.'
                } else
                    c
            }.toMutableList()
        }

        return Triple(board, x, y)
    }

    val input = readFile("21.txt")
    val (board, startX, startY) = parse(input)

    fun fill(x: Int, y: Int, steps: Int): Int {
        var pos = mutableSetOf(x to y)
        for (idx in 1..steps) {
            val nextPos = mutableSetOf<Pair<Int, Int>>()
            for ((i, j) in pos) {
                for (dir in ALL_DIRECTIONS) {
                    val (ni, nj) = moveDirection(i, j, dir)
                    if (ni !in board.indices || nj !in board[0].indices) continue
                    if (board[ni][nj] != '#') {
                        nextPos.add(ni to nj)
                    }
                }
            }

            pos = nextPos
        }

        return pos.size
    }

    fun fill2(x: Int, y:Int, steps: Int): Int {
        val ans = mutableSetOf<Pair<Int, Int>>()
        val seen = mutableSetOf(x to y)
        val queue = ArrayDeque(listOf(Triple(x, y, steps)))

        while (queue.isNotEmpty()) {
            val (i, j, s) = queue.removeFirst()
            if (s % 2 == 0) ans.add(i to j)
            if (s == 0) continue

            for (dir in ALL_DIRECTIONS) {
                val (ni, nj) = moveDirection(i, j, dir)
                if (ni !in board.indices || nj !in board[0].indices
                    || board[ni][nj] == '#' || seen.contains(ni to nj)) continue

                seen.add(ni to nj)
                queue.add(Triple(ni, nj, s - 1))
            }
        }

        return ans.size
    }

    fun part1() = fill2(startX, startY, 64)

    fun Long.pow(n: Int): Long {
        var res = 1L
        for (i in 1..n) {
            res *= this
        }
        return res
    }

    fun part2(): Long {
        assert(board.size == board[0].size)

        val size = board.size
        val steps = 26501365

        assert(startX == startY && startX == size / 2)
        assert(steps % size == size / 2)

        val gridSize = steps / size - 1
        val odd = (gridSize / 2 * 2 + 1).toLong().pow(2)
        val even = ((gridSize + 1) / 2 * 2).toLong().pow(2)

        val oddPoints = fill2(startX, startY, size * 2 + 1)
        val evenPoints = fill2(startX, startY, size * 2)

        val cornerT = fill2(size - 1, startY, size - 1)
        val cornerR = fill2(startX, 0, size - 1)
        val cornerB = fill2(0, startY, size - 1)
        val cornerL = fill2(startX, size - 1, size - 1)

        val smallTR = fill2(size - 1, 0, size / 2 - 1)
        val smallTL = fill2(size - 1, size - 1, size / 2 - 1)
        val smallBR = fill2(0, 0, size / 2 - 1)
        val smallBL = fill2(0, size - 1, size / 2 - 1)

        val largeTR = fill2(size - 1, 0, size * 3 / 2 - 1)
        val largeTL = fill2(size - 1, size - 1, size * 3 / 2 - 1)
        val largeBR = fill2(0, 0, size * 3 / 2 - 1)
        val largeBL = fill2(0, size - 1, size * 3 / 2 - 1)

        return odd * oddPoints + even * evenPoints + (cornerT + cornerB + cornerL + cornerR) +
                (smallTR + smallTL + smallBR + smallBL) * (gridSize + 1) +
                (largeTR + largeTL + largeBR + largeBL) * gridSize.toLong()
    }


    println(part1())
    println(part2())
}
