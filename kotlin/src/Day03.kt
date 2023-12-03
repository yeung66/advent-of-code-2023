
fun main() {
    val dxs = listOf(-1, 0, 1, -1, 1, -1, 0, 1)
    val dys = listOf(-1, -1, -1, 0, 0, 1, 1, 1)

    fun checkNearSign(map: Array<CharArray>, row: Int, col: Int): Boolean {
        var near = false
        for (i in dxs.indices) {
            val (dx, dy) = (dxs[i] to dys[i])
            val (x, y) = (row + dx to col + dy)
            if (x in map.indices && y in map[0].indices && !map[x][y].isDigit() && map[x][y] != '.') {
                near = true
//                println("$row $col $x $y ${map[x][y]}")
                break
            }
        }

        return near
    }

    fun part1(input: String): Int {
        val map = input.lines().map { it.toCharArray()}.toTypedArray()
        var ans = 0

        for (x in map.indices) {
            val row = map[x]
            var col = 0
            while (col < row.size) {
                var start = col
                while (start < row.size && row[start].isDigit()) {
                    start++
                }

                if (start > col) {
                    val num = row.slice(col until start).joinToString("").toInt()
                    val near = (col until start).map { checkNearSign(map, x, it) }.any { it }
                    if (near) {
//                        println(num)
                        ans += num
                    }

                    col = start
                } else {
                    col++
                }
            }
        }

        return ans
    }

    fun findNum(map: Array<CharArray>, row: Int, col: Int): Pair<Pair<Int, Int>, Long> {
        var left = col
        var right = col
        while (left >= 0 && map[row][left].isDigit()) {
            left--
        }

        while (right < map[0].size && map[row][right].isDigit()) {
            right++
        }

        return (row to left + 1) to map[row].slice(left + 1 until right).joinToString("").toLong()
    }

    fun findNearNum(map: Array<CharArray>, row: Int, col: Int): List<Long> {
        val ans = mutableListOf<Long>()
        val nums = mutableSetOf<Pair<Int, Int>>()
        for (i in dxs.indices) {
            val (dx, dy) = (dxs[i] to dys[i])
            val (x, y) = (row + dx to col + dy)
            if (x in map.indices && y in map[0].indices && map[x][y].isDigit()) {
                val (pair, num) = findNum(map, x, y)
                if (nums.add(pair)) {
                    ans.add(num)
                }
            }
        }

        return if (ans.size == 2) ans else listOf()
    }

    fun part2(input: String) : Long {
        val map = input.lines().map { it.toCharArray()}.toTypedArray()
        var ans = 0L

        for (x in map.indices) {
            val row = map[x]
            for (col in row.indices) {
                if (row[col] == '*') {
                    val nearNums = findNearNum(map, x, col)
                    if (nearNums.isNotEmpty()) {
//                        println("($x, $col) [${nearNums.sorted().joinToString(", ")}]")
                        ans += nearNums.reduce(Long::times)
                    }
                }
            }
        }

        return ans
    }

    val input = readFile("03.txt")
    println(part1(input))
    println(part2(input))


}