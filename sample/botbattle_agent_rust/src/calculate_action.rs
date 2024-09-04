use extism_pdk::*;

use crate::{
    agent_request::AgentRequest,
    base64_to_hex,
    drive_response::{AgentDriveResponse, Drive},
    is_even, map_to_direction,
    rotate_response::{AgentRotateResponse, Rotate},
    shoot_response::{AgentShootResponse, Shoot},
    str_to_i16,
};

static mut ACTIONS: Vec<String> = Vec::new();

#[plugin_fn]
pub fn calculate_action(arena: AgentRequest) -> FnResult<String> {
    unsafe {
        let action_string = ACTIONS.pop();
        if action_string.is_some() {
            return Ok(action_string.unwrap());
        }
    }

    let hash_hex = base64_to_hex(&arena.hash);
    let direction = str_to_i16(&hash_hex, 0);
    let shooting_range = str_to_i16(&hash_hex, 2);
    let shoot_decision = str_to_i16(&hash_hex, 4);

    if is_even(shoot_decision) {
        //Shoot
        let action = Shoot {
            power: shooting_range,
            weapon: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB502".to_string(),
            id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
        };
        let response = AgentShootResponse { action };
        unsafe {
            ACTIONS.push(serde_json::to_string(&response).unwrap());
        }
    }

    //Drive
    let action = Drive {
        id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
    };
    let response = AgentDriveResponse { action };
    unsafe {
        ACTIONS.push(serde_json::to_string(&response).unwrap());
    }

    //Rotate
    let direction = map_to_direction(direction);
    let action = Rotate {
        direction,
        id: "B8CB9292-128C-48DB-B4BC-5A7CCD4BB503".to_string(),
    };
    let response = AgentRotateResponse { action };
    Ok(serde_json::to_string(&response).unwrap())
}
