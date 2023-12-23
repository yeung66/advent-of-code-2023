
enum class OP {
    LESS, GREATER;
}

fun parseOP(op: String) = when (op) {
    "<" -> OP.LESS
    ">" -> OP.GREATER
    else -> throw IllegalArgumentException("Invalid OP: $op")
}

sealed class Rule(val dest: String) {
    override fun toString(): String {
        return "Rule(dest='$dest')"
    }
}

class Comparison(val category: String, val op: OP, val value: Int, dest: String) : Rule(dest)

class Default(dest: String) : Rule(dest)

typealias Rating = Map<String, Int>

fun parse(input: String): Pair<Map<String, List<Rule>>, List<Rating>> {
    val parts = input.split("\n\n")

    val workflows = parts[0].lines().map { line ->
        val (category, rest) = line.split("{")
        val rules = rest.substring(0, rest.lastIndex)
            .split(",").map { rule ->
            if (rule.contains(":")) {
                val (ruleStr, next) = rule.split(":")
                val c = ruleStr.substring(0, 1)
                val op = parseOP(ruleStr.substring(1, 2))
                Comparison(c, op, ruleStr.substring(2).toInt(), next)
            } else {
                Default(rule)
            }
        }
        category to rules
    }.toMap()

    val ratings = parts[1].lines().map { line ->
        line.substring(1, line.lastIndex)
            .split(",").associate {
                val (category, value) = it.split("=")
                category to value.toInt()
            }
    }

    return workflows to ratings
}

fun part1(input: String): Int {
    val (workflows, ratings) = parse(input)
    val cur = "in"

//    println(workflows[cur]!!.joinToString(","))

    fun navi(wf: String, rating: Rating): Int {
//        println(wf)
        if (wf == "A" || wf == "R") return if (wf == "A") rating.values.sum() else 0
        val rules = workflows[wf]!!
        for (rule in rules) {
            when (rule) {
                is Default -> return navi(rule.dest, rating)
                is Comparison -> {
                    val value = rating[rule.category]!!
                    val next = if (rule.op == OP.LESS) value < rule.value else value > rule.value
                    if (next) {
                        return navi(rule.dest, rating)
                    }
                }
            }
        }

        return 0
    }

    return ratings.sumOf { navi(cur, it) }
}

fun part2(input: String): Long {
    val (workflows, _) = parse(input)
    val cur = arrayOf(
        "x" to mutableListOf(1, 4000),
        "m" to mutableListOf(1, 4000),
        "a" to mutableListOf(1, 4000),
        "s" to mutableListOf(1, 4000),
    ).toMap()


    fun navi(ranges:  Map<String, MutableList<Int>>, wf: String): Long {
        val count = mutableListOf(0L)

        fun solve(ranges:  Map<String, MutableList<Int>>, dest: String) {
            if (dest == "A") {
                println(ranges.values.joinToString(","))
                count[0] = count[0] + ranges.values.fold(1L) { acc, it -> acc * (it[1] - it[0] + 1) }
            } else if (dest != "R") {
                count[0] = count[0] + navi(ranges, dest)
            }
        }

        for (rule in workflows[wf]!!) {
            when (rule) {
                is Default -> solve(ranges, rule.dest)
                is Comparison -> {
                    val newRanges = ranges
                        .map { it.key to it.value.toMutableList() }.toMap()
                    val range = ranges[rule.category]!!
                    val newRange = newRanges[rule.category]!!

                    when (rule.op) {
                        OP.LESS -> {
                            if (newRange[0] < rule.value) {
                                newRange[1] = minOf(newRange[1], rule.value - 1)
                                range[0] = maxOf(range[0], rule.value)
                            } else {
                                continue
                            }
                        }
                        OP.GREATER -> {
                            if (newRange[1] > rule.value) {
                                newRange[0] = maxOf(newRange[0], rule.value + 1)
                                range[1] = minOf(range[1], rule.value)
                            } else {
                                continue
                            }
                        }
                    }

                    solve(newRanges, rule.dest)
                }
            }
        }

        return count[0]
    }

    return navi(cur, "in")
}

fun main() {
    val input = readFile("19.txt")
    println("part1: ${part1(input)}")
    println("part2: ${part2(input)}")
}