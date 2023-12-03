fn solve1(input: &str) -> usize {
    input
        .lines()
        .map(|line| {
            let first = line.chars().find(|c| c.is_digit(10)).unwrap();
            let last = line.chars().rfind(|c| c.is_digit(10)).unwrap();
            format!("{}{}", first, last).parse::<usize>().unwrap()
        })
        .sum()
}

fn solve2(input: &str) -> usize {
    let nums = vec![
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4",
        "5", "6", "7", "8", "9",
    ];
    input
        .lines()
        .map(|line| {
            let first = nums
                .iter()
                .map(|num| (line.find(num), num))
                .filter(|(idx, _)| idx.is_some())
                .min_by_key(|(idx, _)| idx.unwrap())
                .map(|(_, num)| {
                    if num.len() == 1 {
                        num.parse::<usize>().unwrap()
                    } else {
                        nums.iter().enumerate().find(|(_, &x)| x == *num).unwrap().0 + 1
                    }
                })
                .unwrap();
            let last = nums
                .iter()
                .map(|num| (line.rfind(num), num))
                .filter(|(idx, _)| idx.is_some())
                .max_by_key(|(idx, _)| idx.unwrap())
                .map(|(_, num)| {
                    if num.len() == 1 {
                        num.parse::<usize>().unwrap()
                    } else {
                        nums.iter().enumerate().find(|(_, &x)| x == *num).unwrap().0 + 1
                    }
                })
                .unwrap();
            format!("{}{}", first, last).parse::<usize>().unwrap()
        })
        .sum()
}

pub fn solve() {
    let input = include_str!("input/01.txt");
    println!("Part 1: {}", solve1(input));
    println!("Part 2: {}", solve2(input));
}
