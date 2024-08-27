use serde::{Serialize, Serializer};

#[derive(Debug, Copy, Clone)]
pub(crate) enum Direction {
    North = 1,
    NorthEast = 2,
    East = 3,
    SouthEast = 4,
    South = 5,
    SouthWest = 6,
    West = 7,
    NorthWest = 8,
}

impl Direction {
    pub(crate) fn convert(foo: Direction) -> i16 {
        foo as i16
    }
    pub(crate) fn map_to_direction(value: i16) -> Direction {
        match value {
            0..=31 => Direction::North,
            32..=63 => Direction::NorthEast,
            64..=95 => Direction::East,
            96..=127 => Direction::SouthEast,
            128..=159 => Direction::South,
            160..=191 => Direction::SouthWest,
            192..=223 => Direction::West,
            224..=255 => Direction::NorthWest,
            _ => panic!("Value out of range"),
        }
    }
}

impl Serialize for Direction {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        serializer.serialize_i16(*self as i16)
    }
}
