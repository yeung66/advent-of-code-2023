use std::thread;

pub fn solve() {
    let input = include_str!("input/05.txt");
    // println!("Part 1: {}", part1(input));
    println!("Part 2: {}", part2_parallel(input));
}

fn parse_input(input: &str) -> (Vec<u64>, Vec<Vec<(u64, u64, u64)>>) {
    let (seeds, maps) = input.split_once("\n\n").unwrap();
    let seeds = seeds[7..]
        .split(" ")
        .map(|num| num.parse::<u64>().unwrap())
        .collect::<Vec<_>>();
    let maps = maps
        .split("\n\n")
        .map(|part| {
            part.lines()
                .skip(1)
                .map(|line| {
                    let nums = line
                        .split(" ")
                        .map(|num| num.parse::<u64>().unwrap())
                        .collect::<Vec<_>>();
                    (nums[0], nums[1], nums[2])
                })
                .collect::<Vec<_>>()
        })
        .collect::<Vec<_>>();

    (seeds, maps)
}

fn seed_2_map(seed: u64, map: &Vec<(u64, u64, u64)>) -> u64 {
    for (target, source, len) in map {
        if seed >= *source && seed < *source + *len {
            return *target + seed - source;
        }
    }

    return seed;
}

fn part1(input: &str) -> u64 {
    let (seeds, maps) = parse_input(input);
    seeds
        .iter()
        .map(|seed| maps.iter().fold(*seed, |seed, map| seed_2_map(seed, &map)))
        .min()
        .unwrap()
}

fn part2(input: &str) -> u64 {
    let (seeds, maps) = parse_input(input);

    let mut seeds = (0..seeds.len())
        .filter(|i| i % 2 == 0)
        .flat_map(|i| {
            // println!("{} {}", seeds[i], seeds[i+1]);
            // seeds[i]..seeds[i]+seeds[i+1]
            seeds[i]..seeds[i] + seeds[i + 1]
        })
        .collect::<Vec<_>>();

    for map in maps {
        seeds = seeds
            .into_iter()
            .map(|seed| seed_2_map(seed, &map))
            .collect::<Vec<_>>();
    }

    seeds.into_iter().min().unwrap()
}

fn part2_iter(input: &str) -> u64 {
    let (seeds, maps) = parse_input(input);

    let seeds = (0..seeds.len()).filter(|i| i % 2 == 0).map(|i| {
        // println!("{} {}", seeds[i], seeds[i+1]);
        // seeds[i]..seeds[i]+seeds[i+1]
        (seeds[i], seeds[i] + seeds[i + 1])
    });

    seeds
        .map(|(start, cnt)| {
            (start..start + cnt)
                .map(|seed| maps.iter().fold(seed, |seed, map| seed_2_map(seed, &map)))
                .min()
                .unwrap()
        })
        .min()
        .unwrap()
}

#[derive(Copy, Clone)]
struct Range {
    start: u64,
    end: u64,
}

impl Range {
    fn new(start: u64, end: u64) -> Range {
        Range { start, end }
    }

    fn is_empty(&self) -> bool {
        self.start >= self.end
    }

    fn intersect(&self, other: Range) -> Range {
        Range {
            start: self.start.max(other.start),
            end: self.end.min(other.end),
        }
    }

    // Returns the difference between two ranges.
    // When subtracting a range B from a range A, the result can be up to two ranges, like this:
    // A:   |--------|
    // B:      |-|
    // A-B: |-|   |--|
    // This method returns those two "left" and "right" resulting ranges, which might be empty in
    // the cases where the B range is not within the A range.
    fn difference(&self, other: Range) -> (Range, Range) {
        let left_diff = Range::new(self.start, self.end.min(other.start));
        let right_diff = Range::new(self.start.max(other.end), self.end);
        (left_diff, right_diff)
    }
}

fn part2_range(input: &str) -> u64 {
    let (seeds, maps) = parse_input(input);

    let seeds = (0..seeds.len()).filter(|i| i % 2 == 0).map(|i| {
        // println!("{} {}", seeds[i], seeds[i+1]);
        Range {
            start: seeds[i],
            end: seeds[i] + seeds[i + 1],
        }
    }).collect::<Vec<_>>();

    let mut ranges = seeds;
    for map in maps {
        let mut mapped_ranges = vec![];
        while let Some(range) = ranges.pop() {
            let mut has_intersect = false;
            for (target, source, len) in map.iter() {
                let mapped_range = Range {
                    start: *source,
                    end: *source + len,
                };

                let intersection = range.intersect(mapped_range);
                if intersection.is_empty() {
                    continue;
                }

                has_intersect = true;
                mapped_ranges.push(Range {
                    start: intersection.start - *source + *target,
                    end: intersection.end - *source + *target,
                });

                let (left_diff, right_diff) = range.difference(mapped_range);
                if !left_diff.is_empty() {
                    ranges.push(left_diff);
                }
                if !right_diff.is_empty() {
                    ranges.push(right_diff);
                }

                break;
            }

            if !has_intersect {
                mapped_ranges.push(range);
            }
        }
        ranges = mapped_ranges;
    }

    ranges.iter().map(|range| range.start).min().unwrap()
}
    

fn part2_parallel(input: &str) -> u64 {
    let (seeds, maps) = parse_input(input);

    let seeds = (0..seeds.len()).filter(|i| i % 2 == 0).map(|i| {
        // println!("{} {}", seeds[i], seeds[i+1]);
        // seeds[i]..seeds[i]+seeds[i+1]
        (seeds[i], seeds[i + 1])
    });

    let mut handlers = vec![];
    for (start, cnt) in seeds {
        let maps_clone = maps.clone();
        let handler = thread::spawn(move || {
            (start..start + cnt)
                .map(|seed| {
                    maps_clone
                        .iter()
                        .fold(seed, |seed, map| seed_2_map(seed, &map))
                })
                .min()
                .unwrap()
        });
        handlers.push(handler);
    }

    handlers
        .into_iter()
        .map(|handler| handler.join().unwrap())
        .min()
        .unwrap()
}
