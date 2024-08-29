#![no_main]
use crate::direction::Direction;
use crate::drive_response::*;
use crate::rotate_response::*;
use crate::shoot_response::*;

use extism_pdk::*;

mod direction;
mod drive_response;
mod rotate_response;
mod shoot_response;

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
struct AgentRequest {
    #[serde(rename = "Hash")]
    hash: String,
}

#[plugin_fn]
pub fn calculate_action(arena: AgentRequest) -> FnResult<String> {
    let direction = first_two_bytes_as_numbers(&arena.hash, 0);
    let shooting_range = first_two_bytes_as_numbers(&arena.hash, 2);
    let shoot_decision = first_two_bytes_as_numbers(&arena.hash, 4);
    let drive_decision = first_two_bytes_as_numbers(&arena.hash, 6);

    if is_even(shoot_decision) {
        //Shoot
        let action = Shoot {
            power: shooting_range,
            weapon: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB502".to_string(),
            id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
        };
        let response = AgentShootResponse { action };
        Ok(serde_json::to_string(&response).unwrap())
    } else {
        if is_even(drive_decision) {
            //Drive
            let action = Drive {
                id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
            };
            let response = AgentDriveResponse { action };
            Ok(serde_json::to_string(&response).unwrap())
        } else {
            //Rotate
            let direction = Direction::convert(Direction::map_to_direction(direction));
            let action = Rotate {
                direction,
                id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
            };
            let response = AgentRotateResponse { action };
            Ok(serde_json::to_string(&response).unwrap())
        }
    }
}

fn first_two_bytes_as_numbers(input: &str, index: usize) -> i16 {
    let bytes = input.as_bytes();
    bytes[index] as i16 + bytes[index + 1] as i16
}

fn is_even(value: i16) -> bool {
    value % 2 == 0
}
