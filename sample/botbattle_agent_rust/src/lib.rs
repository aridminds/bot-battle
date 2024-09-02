pub fn str_to_i16(input: &str, index: usize) -> i16 {
    let hex_str = &input[index..index + 2];
    i16::from_str_radix(hex_str, 16).expect("Invalid hex string")
}

pub fn is_even(value: i16) -> bool {
    value % 2 == 0
}

pub fn map_to_direction(value: i16) -> i16 {
    match value {
        0..=31 => 0, //Direction::North,
        32..=63 => 1, //Direction::NorthEast,
        64..=95 => 2, //Direction::East,
        96..=127 => 3, //Direction::SouthEast,
        128..=159 => 4, //Direction::South,
        160..=191 => 5, //Direction::SouthWest,
        192..=223 => 6, //Direction::West,
        224..=255 => 7, //Direction::NorthWest,
        _ => panic!("Value out of range"),
    }
}

#[cfg(test)]
mod tests {
    use base64::decode;
    use hex::encode;
    use super::*;


    #[test]
    fn test_base64_to_i16() {
        let input = "Mk32IJtajGstGH7XeDDGCg==".to_owned();
        let hash_bytes = decode(&input).expect("Invalid Base64 string");
        let hash_hex = encode(&hash_bytes);
        let result = str_to_i16(&hash_hex, 0);
        assert_eq!(result, 50);
    }

    #[test]
    fn test_is_even() {
        let result = is_even(2);
        assert_eq!(result, true);

        let result = is_even(3);
        assert_eq!(result, false);
    }

}