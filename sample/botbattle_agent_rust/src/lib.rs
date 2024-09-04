mod agent_request;
mod calculate_action;
mod drive_response;
mod rotate_response;
mod shoot_response;

use agent_request::Direction;
use base64::decode;
use hex::encode;

pub fn str_to_i16(input: &str, index: usize) -> i16 {
    let hex_str = &input[index..index + 2];
    i16::from_str_radix(hex_str, 16).expect("Invalid hex string")
}

pub fn is_even(value: i16) -> bool {
    value % 2 == 0
}

pub fn map_to_direction(value: i16) -> Direction {
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

pub fn base64_to_hex(input: &String) -> String {
    let hash_bytes = decode(&input).expect("Invalid Base64 string");
    let hash_hex = encode(&hash_bytes);
    hash_hex
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_base64_to_i16() {
        let input = "Mk32IJtajGstGH7XeDDGCg==".to_owned();
        let hash_hex = base64_to_hex(&input);
        let result = str_to_i16(&hash_hex, 0);
        assert_eq!(result, 50);
    }

    #[test]
    fn test_base64_to_hex() {
        let input = "Mk32IJtajGstGH7XeDDGCg==".to_owned();
        let result = base64_to_hex(&input);
        assert_eq!(result, "324df6209b5a8c6b2d187ed77830c60a");
    }

    #[test]
    fn test_is_even() {
        let result = is_even(2);
        assert_eq!(result, true);

        let result = is_even(3);
        assert_eq!(result, false);
    }
}
