pub fn hex_to_i16(input: &str, index: usize) -> i16 {
    let hex_str = &input[index..index + 2];
    i16::from_str_radix(hex_str, 16).expect("Invalid hex string")
}

pub fn is_even(value: i16) -> bool {
    value % 2 == 0
}

#[cfg(test)]
mod tests {
    use super::*;


    #[test]
    fn test_hex_to_i16() {
        let input = "a8f5f167f44f4964e6c998dee827110c";
        let result = hex_to_i16(input, 0);
        assert_eq!(result, 168);

        let result = hex_to_i16(input, 2);
        assert_eq!(result, 245);
    }

    #[test]
    fn test_is_even() {
        let result = is_even(2);
        assert_eq!(result, true);

        let result = is_even(3);
        assert_eq!(result, false);
    }

    #[test]
    fn test_is_even_from_hex() {
        let input = "a8f5f167f44f4964e6c998dee827110c";
        let result_from_hex = hex_to_i16(input, 0);
        let result = is_even(result_from_hex);
        assert_eq!(result, true);

        let result_from_hex = hex_to_i16(input, 2);
        let result = is_even(result_from_hex);
        assert_eq!(result, false);

    }
}