use std::thread;

pub fn solve() {
    let input = include_str!("input/05.txt");
    println!("Part 2: {}", solve2(input));
}

fn seed_2_map(seed: u64, map: &Vec<(u64, u64, u64)>) -> u64 {
    for (target, source, len) in map {
        if seed >= *source && seed < *source + *len {
            return *target + seed - source;
        }
    }

    return seed;
}

fn solve2(input: &str) -> u64 {
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

    // seeds.map(|seed| {
    //     // println!("{}", seed);
    //     maps.iter().fold(seed, |seed, map| seed_2_map(seed, &map))
    // }).min().unwrap()
}
