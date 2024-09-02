#![no_main]
use crate::drive_response::*;
use crate::rotate_response::*;
use crate::shoot_response::*;
use base64::decode;
use hex::encode;

use extism_pdk::*;
use crate::lib::{is_even, map_to_direction, str_to_i16};

mod lib;
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
    let hash_bytes = decode(&arena.hash).expect("Invalid Base64 string");
    let hash_hex = encode(&hash_bytes);
     let direction = str_to_i16(&hash_hex, 0);
     let shooting_range = str_to_i16(&hash_hex, 2);
     let shoot_decision = str_to_i16(&hash_hex, 4);
     let drive_decision = str_to_i16(&hash_hex, 6);

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
             let direction = map_to_direction(direction);
            let action = Rotate {
                direction,
                id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
            };
            let response = AgentRotateResponse { action };
            Ok(serde_json::to_string(&response).unwrap())
       }
    }
}