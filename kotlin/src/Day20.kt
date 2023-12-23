import kotlin.collections.ArrayDeque

enum class Pulse {
    HIGH, LOW
}

sealed class Module(val name: String, val toSend: List<String>) {
    abstract fun send(receive: Pulse, from: String): Pair<Pulse, List<String>>
}

class FlipFlop(var on: Boolean, name: String, toSend: List<String>) : Module(name, toSend) {
    override fun send(receive: Pulse, from: String): Pair<Pulse, List<String>> {
        return when (receive) {
            Pulse.HIGH -> Pulse.HIGH to listOf()
            Pulse.LOW -> {
                on = !on
                if (on) {
                    Pulse.HIGH to toSend
                } else {
                    Pulse.LOW to toSend
                }
            }
        }
    }
}

class Conjunction(var inputs: MutableMap<String, Pulse>, name: String, toSend: List<String>): Module(name, toSend) {
    override fun send(receive: Pulse, from: String): Pair<Pulse, List<String>> {
        inputs[from] = receive
        return if (inputs.values.all { it == Pulse.HIGH }) {
            Pulse.LOW to toSend
        } else {
            Pulse.HIGH to toSend
        }
    }
}

const val BROADCASTER = "broadcaster"
class Broadcaster(toSend: List<String>): Module(BROADCASTER, toSend) {
    override fun send(receive: Pulse, from: String): Pair<Pulse, List<String>> {
        return receive to toSend
    }
}



fun main() {
    val input = readFile("20.txt")

    fun parse(input: String): Map<String, Module> {
        val modules = input.lines().associate { line ->
            val (left, right) = line.split(" -> ")
            val module = when {
                left == "broadcaster" -> Broadcaster(right.split(", "))
                left.startsWith("%") -> FlipFlop(false, left.substring(1), right.split(", "))
                left.startsWith("&") -> Conjunction(mutableMapOf(), left.substring(1), right.split(", "))
                else -> throw IllegalArgumentException("Invalid module: $left")
            }
            module.name to module
        }

        modules.entries.filter { (_, module) -> module is Conjunction }
            .forEach { (name, module) ->
                val conjunction = module as Conjunction
                val inputs = mutableMapOf<String, Pulse>()
                conjunction.inputs = inputs
                modules.entries.filter { (_, module2) ->
                    module2.toSend.contains(name)
                }.forEach { (name1, _) ->
                    inputs[name1] = Pulse.LOW
                }
            }

        return modules
    }

    fun snapshot(modules: Map<String, Module>): String {
        return modules.entries
            .filter { (_, module) -> module is FlipFlop || module is Conjunction }
            .map { (name, module) ->
            name to if (module is FlipFlop) {
                if (module.on) "1" else "0"
            } else {
                (module as Conjunction).inputs.entries.sortedBy { it.key }.joinToString(";")
            }
        }.sortedBy { it.first }
            .map { (name, str) -> "$name:$str" }
            .joinToString("|")
    }

    fun part1(): Long {
        val modules = parse(input)
        val snapshots = mutableMapOf<String, List<Int>>()
        for (i in 1..1000) {
            var (highs, lows) = 0 to 0
            val toSend = ArrayDeque(listOf(Triple("button", Pulse.LOW, listOf(BROADCASTER))))
            while (toSend.isNotEmpty()) {
                val (from, pulse, names) = toSend.removeFirst()

                names.forEach { name ->
                    when (pulse) {
                        Pulse.HIGH -> highs++
                        Pulse.LOW -> lows++
                    }
                    val (newPulse, newNames) = if (modules.containsKey(name)) {
                        modules[name]!!.send(pulse, from)
                    } else {
                        pulse to listOf()
                    }

                    if (newNames.isNotEmpty()) {
                        toSend.addLast(Triple(name, newPulse, newNames))
                    }
                }
            }

            val s = snapshot(modules)
            if (snapshots.containsKey(s)) {
                break
            } else {
                snapshots[s] = listOf(i, highs, lows)
            }

        }

        val (highs, lows) = snapshots.values.fold(0L to 0L) { (highs, lows), (_, h, l) ->
            highs + h to lows + l
        }

        return highs * lows
    }

    fun part2(): Long {
        val modules = parse(input)
        val cycleLast = LongArray(4)
        val cycleLength = LongArray(4)
        val preModule = listOf("dl", "ks", "pm", "vk")

        var i = -1L
        while (true) {
            i ++
            val toSend = ArrayDeque(listOf(Triple("button", Pulse.LOW, listOf(BROADCASTER))))
            while (toSend.isNotEmpty()) {
                val (from, pulse, names) = toSend.removeFirst()

                names.forEach { name ->
                    // check 到 rx 到前置 module dt 到前置多久会发一次 HIGH
                    // 取巧了一下直接看了输入把它写成了常量
                    if (name == "dt" && pulse == Pulse.HIGH) {
                        val idx = preModule.indexOf(from)
                        if (idx != -1) {
                            val last = cycleLast[idx]
                            cycleLast[idx] = i
                            cycleLength[idx] = i - last + 1
                        }

                        if (cycleLength.all { it != 0L }) {
                            println("cycleLength: ${cycleLength.joinToString(",")} i: $i")
                            return cycleLength.fold(1L) { acc, it -> acc * it }
                        }
                    }

                    val (newPulse, newNames) = if (modules.containsKey(name)) {
                        modules[name]!!.send(pulse, from)
                    } else {
                        pulse to listOf()
                    }

                    if (newNames.isNotEmpty()) {
                        toSend.addLast(Triple(name, newPulse, newNames))
                    }
                }
            }
        }
    }

    println("Part 1: ${part1()}")
    println("Part 2: ${part2()}")
}
