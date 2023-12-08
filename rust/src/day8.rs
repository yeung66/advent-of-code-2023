use std::collections::HashMap;


fn part1(input: &str) -> usize {
    let (instructions, navigations) = input.split_once("\n\n").unwrap();
    let instructions = instructions.chars().collect::<Vec<_>>();
    let navigations = navigations
        .lines()
        .map(|line| {
            let cur = line[0..3].to_string();
            let left = line[7..10].to_string();
            let right = line[12..15].to_string();

            (cur, (left, right))
        })
        .collect::<HashMap<String, (String, String)>>();

    let mut pos = "AAA".to_string();
    let mut cnt = 0;
    while pos != "ZZZ" {
        // println!("{}", &pos);
        let (left, right) = navigations.get(&pos).unwrap();
        if instructions[cnt % instructions.len()] == 'L' {
            pos = left.clone();
        } else {
            pos = right.clone();
        }

        cnt += 1;
    }

    cnt
}

fn gcd(a: u64, b: u64) -> u64 {
    if b == 0 {
        return a;
    }

    gcd(b, a % b)
}

fn part2(input: &str) -> u64 {
    let (instructions, navigations) = input.split_once("\n\n").unwrap();
    let instructions = instructions.chars().collect::<Vec<_>>();
    let navigations = navigations
        .lines()
        .map(|line| {
            let cur = line[0..3].to_string();
            let left = line[7..10].to_string();
            let right = line[12..15].to_string();

            (cur, (left, right))
        })
        .collect::<HashMap<String, (String, String)>>();

    let pos = navigations.clone().into_keys().filter(|pos| pos.ends_with("A")).collect::<Vec<_>>();
    let steps = pos.iter().map(|pos| {
        let mut pos = pos.clone();
        let mut cnt = 0;
        let mut idx = 0;
        while !pos.ends_with("Z") {
            // println!("{}", &pos);
            let (left, right) = navigations.get(&pos).unwrap();
            if instructions[idx % instructions.len()] == 'L' {
                pos = left.clone();
            } else {
                pos = right.clone();
            }

            idx = (idx + 1) % instructions.len();
            cnt += 1;
        }

        cnt
    }).collect::<Vec<u64>>();

    println!("{:?}", steps);

    steps.into_iter().reduce(|acc, x| acc * x / gcd(acc, x)).unwrap()
    
}

pub fn solve() {
    let input = include_str!("input/08.txt");
    println!("Part 1: {}", part1(input));
    println!("Part 2: {}", part2(input));
}