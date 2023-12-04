import kotlin.math.min
import kotlin.math.pow

fun main() {

    fun parseLine(line: String) =
        line.split(": ")[1]
            .split(" | ")
            .map { it.split(" ").filter { num -> num.isNotBlank() } }
            .map { it.map { num -> num.toInt()}.toSet() }

    fun part1(input: String) =
        input.lines().map {line ->
            val (winning, have) = parseLine(line)

            2f.pow(have.filter { it in winning }.size - 1).toInt()
        }.sum()

    fun part2(input: String): Int {
        val cardCount = Array<Int>(input.lines().size) { 1 }
        for ((idx, line) in input.lines().withIndex()) {
            val (winning, have) = parseLine(line)

            val win = have.filter { it in winning }.size
            for (i in 1 .. min(win, cardCount.size - 1 - idx))
                cardCount[idx + i] += cardCount[idx]
        }

        println(cardCount.joinToString(","))

        return cardCount.sum()
    }

    val input = readFile("04.txt")
    println(part1(input))
    println(part2(input))
}