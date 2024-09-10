use crate::agent_request::Direction;
use extism_pdk::*;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, ToBytes, FromBytes)]
#[encoding(Json)]
pub struct AgentRotateResponse {
    #[serde(rename = "Action")]
    pub action: Rotate,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "$type")]
pub struct Rotate {
    #[serde(rename = "Direction")]
    pub direction: Direction,
    #[serde(rename = "Id")]
    pub id: String,
}
