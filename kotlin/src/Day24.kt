import org.matheclipse.core.eval.ExprEvaluator
import org.matheclipse.core.expression.F.*
import org.matheclipse.core.expression.S
import org.matheclipse.core.interfaces.IAST


typealias Position = Triple<Double, Double, Double>

fun position(nums: List<Double>) = Position(nums[0], nums[1], nums[2])

data class HailStone(val pos: Position, val vel: Position)

infix fun HailStone.collision(other: HailStone): Pair<Double, Double>? {
    val aSlope = this.vel.second / this.vel.first
    val aIntercept = this.pos.second - aSlope * this.pos.first

    val bSlope = other.vel.second / other.vel.first
    val bIntercept = other.pos.second - bSlope * other.pos.first

    assert(aSlope != bSlope)

    val x = (bIntercept - aIntercept) / (aSlope - bSlope)
    val y = aSlope * x + aIntercept

    return if (x.isFinite() && y.isFinite()
        && !(this.vel.first < 0 && x > this.pos.first)
        && !(this.vel.first > 0 && x < this.pos.first)
        && !(other.vel.first < 0 && x > other.pos.first)
        && !(other.vel.first > 0 && x < other.pos.first)) {
        x to y
    } else {
        null
    }
}

fun main() {
    val input = readFile("24.txt")
    fun parse(): List<HailStone> {
        return input.lines().map { line ->
            val (pos, vel) = line.split("@")
            HailStone(
                position(pos.split(", ").map(String::toDouble)),
                position(vel.split(", ").map(String::toDouble))
            )
        }
    }

    val stones = parse()

    fun <T> List<T>.tupleCombinations(): List<Pair<T, T>> {
        return this.flatMapIndexed { index, t ->
            this.subList(index + 1, this.size).map { t to it }
        }
    }

    fun part1() {
        val RANGES = 200000000000000.0..400000000000000.0
        val ans = stones.tupleCombinations()
            .map { (a, b) -> a collision b }
            .filterNotNull()
            .filter { RANGES.contains(it.first) && RANGES.contains(it.second) }
            .count()

        println("part1: $ans")

    }

    fun Double.getSignStr() = if (this < 0)
        "+" + (-this).toBigDecimal().toPlainString()
        else (-this).toBigDecimal().toPlainString()

    fun part2() {
        val expr = ExprEvaluator(false, 100)
        val equations = mutableListOf<IAST>()
        val (x, y, z) = Triple(Dummy("x"), Dummy("y"), Dummy("z"))
        val (vx, vy, vz) = Triple(Dummy("vx"), Dummy("vy"), Dummy("vz"))
        for (stone in stones.take(3)) {
            val eq1 = Equal(
                (x - ZZ(stone.pos.first.toLong())).times(vy - ZZ(stone.vel.second.toLong())),
                (y - ZZ(stone.pos.second.toLong())).times(vx - ZZ(stone.vel.first.toLong()))
            )

            val eq2 = Equal(
                (y - ZZ(stone.pos.second.toLong())).times(vz - ZZ(stone.vel.third.toLong())),
                (z - ZZ(stone.pos.third.toLong())).times(vy - ZZ(stone.vel.second.toLong()))
            )

            equations.add(eq1)
            equations.add(eq2)
        }

        val variables = List(x, y, z, vx, vy, vz)
        val symEquations = List(equations[0], equations[1], equations[2],
                equations[3], equations[4], equations[5])

        val solve = Solve(symEquations, variables)
        println(solve)
        val result = expr.eval(solve)
        println("part2: $result")

    }

    part1()
    part2()
}