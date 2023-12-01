fun main() {
    val input =  readFile("01.txt")

    fun part1(input: String) =
        input.lines().sumOf {
            val first = it.find { c -> c.isDigit() }!!
            val last = it.findLast { c -> c.isDigit() }!!
            "$first$last".toInt()
        }

    val nums = mapOf(
        "one" to 1,
        "two" to 2,
        "three" to 3,
        "four" to 4,
        "five" to 5,
        "six" to 6,
        "seven" to 7,
        "eight" to 8,
        "nine" to 9,

        "1" to 1,
        "2" to 2,
        "3" to 3,
        "4" to 4,
        "5" to 5,
        "6" to 6,
        "7" to 7,
        "8" to 8,
        "9" to 9,

    )

    fun part2(input: String) =
        input.lines().sumOf {
            val first = nums.keys.map { key -> (it.indexOf(key) to key)}.filter { (idx, key) -> idx != -1 }.minBy { (idx, key) -> idx }.second
            val last = nums.keys.map { key -> (it.lastIndexOf(key) to key)}.filter { (idx, key) -> idx != -1 }.maxBy { (idx, key) -> idx }.second

            "${nums[first]}${nums[last]}".toInt()
        }

    println(part1(input))
    println(part2(input))
}
