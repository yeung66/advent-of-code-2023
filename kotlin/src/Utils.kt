import java.io.File

fun readFile(filename: String) = File("${System.getProperty("user.dir")}/src/input/$filename").readText()