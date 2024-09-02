// #[derive(Debug, Copy, Clone)]
// pub(crate) enum Direction {
//     North = 1,
//     NorthEast = 2,
//     East = 3,
//     SouthEast = 4,
//     South = 5,
//     SouthWest = 6,
//     West = 7,
//     NorthWest = 8,
// }
//
// impl Direction {
//     pub(crate) fn map_to_direction(value: i16) -> i16 {
//         match value {
//             0..=31 => 0, //Direction::North,
//             32..=63 => 1, //Direction::NorthEast,
//             64..=95 => 2, //Direction::East,
//             96..=127 => 3, //Direction::SouthEast,
//             128..=159 => 4, //Direction::South,
//             160..=191 => 5, //Direction::SouthWest,
//             192..=223 => 6, //Direction::West,
//             224..=255 => 7, //Direction::NorthWest,
//             _ => panic!("Value out of range"),
//         }
//     }
// }
