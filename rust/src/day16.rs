use std::collections::BTreeSet;
use std::fmt::Formatter;
use std::fmt;

use itertools::Itertools;

#[derive(Debug, Clone, Copy)]
enum Space {
    Empty,
    MirrorLeft, MirrorRight,
    SplitterVertical, SplitterHorizontal,
}


type Direction = (i32, i32);

const LEFT: (i32, i32) = (0, -1);
const RIGHT: (i32, i32) = (0, 1);
const UP : (i32, i32) = (-1, 0);
const DOWN: (i32, i32) = (1, 0);

impl fmt::Display for Space {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        match self {
            Space::Empty => write!(f, "."),
            Space::MirrorLeft => write!(f, "/"),
            Space::MirrorRight => write!(f, "\\"),
            Space::SplitterVertical => write!(f, "|"),
            Space::SplitterHorizontal => write!(f, "-"),
        }
    }
    
}

impl Space {
    fn from(c: char) -> Space {
        match c {
            '.' => Space::Empty,
            '/' => Space::MirrorLeft,
            '\\' => Space::MirrorRight,
            '|' => Space::SplitterVertical,
            '-' => Space::SplitterHorizontal,
            _ => panic!("\\ Invalid character {c} "),
        }
    }
}

fn parse_input(input: &str) -> Vec<Vec<Space>> {
    input.lines()
        .map(|line| line.chars().map(Space::from).collect())
        .collect()
}

fn print_space<T>(space: &Vec<Vec<T>>) where T: std::fmt::Display {
    for row in space {
        for col in row {
            print!("{}", col);
        }
        println!();
    }
}

fn simulate(space: &Vec<Vec<Space>>, start: (i32, i32, Direction)) -> usize {
    let mut queue = Vec::new();
    let mut lighted = BTreeSet::new();
    let mut visited = BTreeSet::new();
    queue.push((start.0, start.1, start.2));

    while let Some((x, y, (dx, dy))) = queue.pop() {
        if x < 0 || y < 0 || x >= space.len() as i32 || y >= space[0].len() as i32 || visited.contains(&(x, y, (dx, dy))) {
            continue;
        }

        // println!("{} {} {} {}", x, y, dx, dy);


        lighted.insert((x as usize, y as usize));
        visited.insert((x, y, (dx, dy)));

        match space[x as usize][y as usize] {
            Space::Empty => {
                queue.push((x + dx, y + dy, (dx, dy)));
            }
            Space::MirrorLeft => {
                match (dx, dy) {
                    LEFT => {
                        queue.push(((x + 1) as i32, y as i32, DOWN));
                    },
                    RIGHT => {
                        queue.push(((x - 1) as i32, y as i32, UP));
                    },
                    UP => {
                        queue.push((x as i32, (y + 1) as i32, RIGHT));
                    },
                    DOWN => {
                        queue.push((x as i32, (y - 1) as i32, LEFT));
                    },
                    _ => panic!("Invalid direction"),
                }
            }
            Space::MirrorRight => {
                match (dx, dy) {
                    LEFT => {
                        queue.push(((x - 1) as i32, y as i32, UP));
                    },
                    RIGHT => {
                        queue.push(((x + 1) as i32, y as i32, DOWN));
                    },
                    UP => {
                        queue.push((x as i32, (y - 1) as i32, LEFT));
                    },
                    DOWN => {
                        queue.push((x as i32, (y + 1) as i32, RIGHT));
                    },
                    _ => panic!("Invalid direction"),
                    
                }
            }
            Space::SplitterHorizontal => {
                match (dx, dy) {
                    UP | DOWN => {
                        queue.push((x as i32, (y - 1) as i32, LEFT));
                        queue.push((x as i32, (y + 1) as i32, RIGHT));
                    },
                    LEFT => {
                        queue.push((x as i32, y as i32 - 1, LEFT));
                    },
                    RIGHT => {
                        queue.push((x as i32, y as i32 + 1, RIGHT));
                    },
                    _ => panic!("Invalid direction"),
                }
            }
            Space::SplitterVertical => {
                match (dx, dy) {
                    LEFT | RIGHT => {
                        queue.push(((x - 1) as i32, y as i32, UP));
                        queue.push(((x + 1) as i32, y as i32, DOWN));
                    },
                    UP => {
                        queue.push((x as i32 - 1, y as i32, UP));
                    },
                    DOWN => {
                        queue.push((x as i32 + 1, y as i32, DOWN));
                    },
                    
                    _ => panic!("Invalid direction"),
                
                }
            }
        }
    }

    let after_space = (0..space.len()).map(|x| {
        (0..space[0].len()).map(|y| {
            let c = if !lighted.contains(&(x, y)) {
                '.'
            } else {
                '#'
            };
            c
        }).collect_vec()
    }).collect_vec();
    

    println!();
    print_space(&after_space);
    
    lighted.len()
}

fn part1(input: &str) -> usize {
    let space = parse_input(input);
    let start = (0, 0, RIGHT);
    simulate(&space, start)
}

fn gen_start(h: usize, w: usize) -> Vec<(i32, i32, Direction)> {
    let mut start = Vec::new();
    for i in 0..h {
        start.push((i as i32, 0, RIGHT));
        start.push((i as i32, w as i32 - 1, LEFT));
    }
    for i in 0..w {
        start.push((0, i as i32, DOWN));
        start.push((h as i32 - 1, i as i32, UP));
    }
    start
}

fn part2(input: &str) -> usize {
    let space = parse_input(input);
    gen_start(space.len(), space[0].len())
        .iter()
        .map(|start| simulate(&space, *start))
        .max().unwrap()
}

pub fn solve() { 
    let input = include_str!("input/16.txt");
    println!("Part 1: {}", part1(input));
    println!("Part 2: {}", part2(input));
}