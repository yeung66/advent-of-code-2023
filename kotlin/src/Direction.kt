
enum class Direction(val dx: Int, val dy: Int) {
    UP(-1, 0), DOWN(1, 0), LEFT(0, -1), RIGHT(0, 1);
}

val ALL_DIRECTIONS = listOf(Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT)

fun moveDirection(i: Int, j: Int, direction: Direction): Pair<Int, Int> {
    return i + direction.dx to j + direction.dy
}

