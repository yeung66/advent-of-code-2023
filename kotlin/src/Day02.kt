fun main() {
    val has = mapOf(
        "red" to 12,
        "green" to 13,
        "blue" to 14,
    )

    fun parseLine(line: String) =
        line.split(": ")[1]
            .split("; ")
            .asSequence()
            .map {
                it.split(", ")
                    .map { part ->
                        val (cnt, color) = part.split(" ")
                        (color to cnt.toInt())
                    }
            }
            .flatten()
            .groupBy { (color, _) -> color }

    fun part1(input: String) =
        input.lines().mapIndexed { idx, line ->
            if (parseLine(line)
                    .map { (color, cnts) -> (color to cnts.maxOf { (_, cnt) -> cnt }) }
                    .all { (color, cnt) -> cnt <= has[color]!! }
            ) idx + 1 else 0
        }.sum()

    fun part2(input: String) =
        input.lines().sumOf { line ->
            parseLine(line)
                .map { (color, cnts) -> cnts.maxOf { (_, cnt) -> cnt } }
                .fold(1) { acc: Int, cnt -> acc * cnt }
        }

    val input = readFile("02.txt")
    println(part1(input))
    println(part2(input))
}