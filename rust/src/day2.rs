use std::collections::HashMap;
use itertools::Itertools;

type ColorCountPair = (String, i32);

fn parse_input(input: &str) -> Result<Vec<ColorCountPair>, std::num::ParseIntError> {
    input
        .split("; ")
        .flat_map(|part| {
            part.split(", ").map(|pair| {
                let (cnt, color) = pair.split_once(" ").unwrap();
                cnt.parse::<i32>().map(|cnt| (color.to_string(), cnt))
            })
        })
        .collect()
}

pub fn solve1(input: &str) -> Result<usize, std::num::ParseIntError> {
    let mut has = HashMap::new();
    has.insert("red", 12);
    has.insert("blue", 14);
    has.insert("green", 13);

    input
        .lines()
        .enumerate()
        .map(|(idx, line)| {
            let (_, content) = line.split_once(": ").unwrap();
            let color_count_pairs = parse_input(content)?;

            let work = color_count_pairs
                .into_iter()
                .group_by(|(color, _)| color.clone())
                .into_iter()
                .map(|(color, group)| (color, group.max_by_key(|(_, cnt)| *cnt).unwrap().1))
                .any(|(color, cnt)| *has.get(color.as_str()).unwrap() < cnt);

            match work {
                false => Ok(1 + idx),
                true => Ok(0),
            }
        })
        .sum()
}

pub fn solve2(input: &str) -> Result<i32, std::num::ParseIntError> {
    input
        .lines()
        .map(|line| {
            let (_, content) = line.split_once(": ").unwrap();
            let color_count_pairs = parse_input(content)?;

            color_count_pairs
                .into_iter()
                .sorted_by_key(|(color, _)| color.clone().clone())
                .group_by(|(color, _)| color.clone().clone())
                .into_iter()
                .map(|(c, group)| {
                    let cnt = group.max_by_key(|(_, cnt)| *cnt).unwrap().1;
                    // println!("{}: {}", c, cnt);
                    Ok(cnt)
                })
                .fold(Ok(1), |acc, x| {
                    let acc = acc?;
                    let x = x?;
                    // println!("{} * {} = {}", acc, x, acc * x);
                    Ok(acc * x)
                })
        })
        .sum()
}

pub fn solve() {
    let input = include_str!("input/02.txt");
    match solve1(input) {
        Ok(result) => println!("Part 1: {}", result),
        Err(e) => eprintln!("Error in Part 1: {}", e),
    }
    match solve2(input) {
        Ok(result) => println!("Part 2: {}", result),
        Err(e) => eprintln!("Error in Part 2: {}", e),
    }
}
