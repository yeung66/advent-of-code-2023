use std::collections::{HashSet, HashMap};

use itertools::Itertools;


fn parse_input(input: &str) -> Vec<Vec<char>> {
    input
        .lines()
        .map(|line| line.chars().collect::<Vec<_>>())
        .collect::<Vec<_>>()
}

fn print_board(board: &Vec<Vec<char>>) {
    for row in board {
        for c in row {
            print!("{}", c);
        }
        println!();
    }
}

fn process_vertical(board: &mut Vec<Vec<char>>, up: bool ) {
    let range = if up {
        (1..board.len()).collect_vec()
    } else {
        (0..board.len() - 1).rev().collect_vec()
    };

    let diff = if up { -1 } else { 1 };

    for i in range.into_iter() {
        for j in 0..board[i].len() {
            if board[i][j] != 'O' {
                continue;
            }

            let mut target_row = i as i32 + diff;
            while target_row < (board.len() as i32) && target_row >= 0 && board[target_row as usize][j] == '.' {
                target_row += diff;
            }

            if target_row - 1 != i as i32 && board[(target_row - diff) as usize][j] == '.' {
                board[(target_row - diff) as usize][j] = 'O';
                board[i][j] = '.';
            }
        }
    }
}

fn process_horizonal(board: &mut Vec<Vec<char>>, left: bool) {
    let range = if left {
        (1..board[0].len()).collect_vec()
    } else {
        (0..board[0].len() - 1).rev().collect_vec()
    };

    let diff = if left { -1 } else { 1 };

    for i in 0..board.len() {
        for j in range.iter() {
            if board[i][*j] != 'O' {
                continue;
            }

            let mut target_col = *j as i32 + diff;
            while target_col < (board[i].len() as i32) && target_col >= 0 && board[i][target_col as usize] == '.' {
                target_col += diff;
            }

            if target_col - 1 != *j as i32 && board[i][(target_col - diff) as usize] == '.' {
                board[i][(target_col - diff) as usize] = 'O';
                board[i][*j] = '.';
            }
        }
    }

}

fn process_one(mut board: &mut Vec<Vec<char>>) {
    process_vertical(&mut board, true);
    process_horizonal(&mut board, true);
    process_vertical(&mut board, false);
    process_horizonal(&mut board, false);
}

fn part1(input: &str) -> u32 {
    let mut board = parse_input(input);

    process_vertical(&mut board, true);

    let h = board.len();

    board.iter().enumerate()
        .map(|(i, row)| {
            row.iter()
                .filter(|&c| *c == 'O')
                .map(|_| {
                    (h - i) as u32
                })
                .sum::<u32>()
        })
        .sum::<u32>()
}

fn board_to_string(board: &Vec<Vec<char>>) -> String {
    board.iter().map(|row| row.iter().collect::<String>()).collect::<String>()
}

fn part2(input: &str) -> u32 {
    let mut board = parse_input(input);
    let limit = 1000000000;
    let mut exists = HashMap::new();
    let mut board_str = board_to_string(&board);
    for _idx in 0..limit {
        let start = std::time::Instant::now();
        process_one(&mut board);

        // print_board(&board);
        // println!();

        board_str = board_to_string(&board);
        if exists.contains_key(&board_str) {
            break;
        }

        exists.insert(board_str.clone(), _idx);
        

        let end = std::time::Instant::now();
        println!("{:} cost time {:?}", _idx, end - start);
        
    }

    let cycle = exists.len() as u32 - exists.get(&board_str).unwrap();
    let times = (limit - exists.len() as u32) % cycle;
    println!("times: {}, cycle: {}, exists: {}", times, cycle, exists.len());
    for _idx in 0..times-1 {
        process_one(&mut board);
    }

    let h = board.len();
    board.iter().enumerate()
        .map(|(i, row)| {
            row.iter()
                .filter(|&c| *c == 'O')
                .map(|_| {
                    (h - i) as u32
                })
                .sum::<u32>()
        })
        .sum::<u32>()
}

pub fn solve() {
    let input = include_str!("input/14.txt");
    println!("part1: {}", part1(input));
    println!("part2: {}", part2(input));
}