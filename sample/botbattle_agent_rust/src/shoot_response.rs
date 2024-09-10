use extism_pdk::*;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, ToBytes, FromBytes)]
#[encoding(Json)]
pub struct AgentShootResponse {
    #[serde(rename = "Action")]
    pub action: Shoot,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "$type")]
pub struct Shoot {
    #[serde(rename = "Power")]
    pub power: i16,
    #[serde(rename = "Weapon")]
    pub weapon: String,
    #[serde(rename = "Id")]
    pub id: String,
}
