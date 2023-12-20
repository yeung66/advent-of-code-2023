use itertools::Itertools;
use nd_vec::{Vec2, vector};

use crate::aoc_lib::direction::Direction;

type Pos = Vec2<usize>;


#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
struct DigPlan {
    direction: Direction,
    length: i64,
}

fn parse_plans(input: &'static str, part2: bool) -> Vec<DigPlan> {
    input
        .lines()
        .map(|line| {
            let parts = line.split(" ").collect_vec();
            if !part2 {
                DigPlan {
                    direction: match parts[0] {
                        "R" => Direction::Right,
                        "L" => Direction::Left,
                        "U" => Direction::Up,
                        "D" => Direction::Down,
                        _ => panic!("Invalid direction"),
                    },
                    length: parts[1].parse().unwrap(),
                }
            } else {
                let color = parts[2][2..parts[2].len()-1].to_string();
                DigPlan {
                    direction: match color.chars().last().unwrap() {
                        '0' => Direction::Right,
                        '1' => Direction::Down,
                        '2' => Direction::Left,
                        '3' => Direction::Up,
                        _ => panic!("Invalid direction"),
                    },
                    length: i64::from_str_radix(&color[..color.len()-1], 16).unwrap(),
                }
            }
        })
        .collect()
}

#[allow(dead_code)]
fn print_board(board: &Vec<Vec<char>>) {
    for row in board.iter() {
        for c in row.iter() {
            print!("{}", c);
        }
        println!();
    }
}

// 鞋带公式（Shoelace formula）也叫做高斯面积公式（Gauss's area formula），是一种用于计算任意多边形面积的公式，特别是当你知道所有顶点的坐标时。
// A = 1/2 * abs(Σ[i=1 to n-1] (xi*yi+1 - xi+1*yi) + xn*y1 - x1*yn)
fn part(plans: &Vec<DigPlan>) -> i64 {
    let mut pos = vector!(0, 0);
    let mut perimeter = 0;
    let mut area = 0;

    for plan in plans.iter() {
        let cng = plan.direction.as_vector() * plan.length;
        pos += cng;

        perimeter += plan.length;
        area += pos.x() * cng.y();
    }

    
    
    area + perimeter / 2 + 1
}

fn part1(input: &'static str) -> i64 {
    let plans = parse_plans(input, false);
    part(&plans)
}

fn part2(input: &'static str) -> i64 {
    let plans = parse_plans(input, true);
    part(&plans)
}

pub fn solve() {
    let input = include_str!("input/18.txt");

    println!("Part 1: {}", part1(input));
    println!("Part 2: {}", part2(input));
 }