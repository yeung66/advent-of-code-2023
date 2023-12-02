use itertools::Itertools;


pub fn solve(input: &str) -> i32 {
    input.lines()
        .enumerate()
        .map(|(idx, line)| {
            let (_, content) = line.split_once(": ").unwrap();
            content.split("; ")
                .map(|part| {
                    part.split(", ").map(|pair| {
                        let (cnt, color) = pair.split_once(" ").unwrap();
                        (color, cnt.parse::<i32>().unwrap())
                    })
                })
                .flatten()
                .group_by(|(color, cnt)| color )
                .into_iter()
                .map(|(color, group)| {
                    
                    (color, group.max())
                })
                
            0
        });

    0
        
}