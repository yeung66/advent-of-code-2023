use itertools::Itertools;

#[derive(Debug, Clone)]
struct Lens {
    label: String,
    focal: usize,
}

type Boxes = Vec<Vec<Lens>>;

enum Operation {
    Dash(String),
    Equal(String, usize),
}

fn parse_operation(op: &str) -> Operation {
    if op.contains("-") {
        Operation::Dash(op[0..op.len() - 1].to_string())
    } else {
        let (label, focal) = op.split_once("=").unwrap();
        Operation::Equal(label.to_string(), focal.parse().unwrap())
    }
}

fn get_hash(chars: &str) -> usize {
    chars
        .chars()
        .fold(0, |acc, c| (acc + c as usize) * 17 % 256)
}

fn part1(input: &str) -> usize {
    input.split(",").map(get_hash).sum()
}

fn part2(input: &str) -> usize {
    let mut boxes: Boxes = (0..256).map(|_| Vec::new()).collect();
    let operations = input.split(",").map(parse_operation);

    for op in operations {
        match op {
            Operation::Dash(label) => {
                let hash = get_hash(&label);
                let boxx = boxes.get_mut(hash).unwrap();
                if let Some((idx, _)) = boxx.iter().find_position(|lens| lens.label == label) {
                    boxx.remove(idx);
                }
            }
            Operation::Equal(label, focal) => {
                let hash = get_hash(&label);
                let boxx = boxes.get_mut(hash).unwrap();
                if let Some(lens) = boxx.iter_mut().find(|lens| lens.label == label) {
                    lens.focal = focal;
                } else {
                    boxx.push(Lens { label, focal });
                }
            }
        }
    }

    boxes.into_iter()
        .enumerate()
        .map(|(idx1, lenses)| {
            lenses.into_iter()
                .enumerate()
                .map(|(idx2, lens)| lens.focal * (idx2 + 1) * (idx1 + 1))
                .sum::<usize>()
        })
        .sum()

}

pub fn solve() {
    let input = include_str!("input/15.txt");
    println!("Part 1: {}", part1(input));
    println!("Part 2: {}", part2(input));
}
