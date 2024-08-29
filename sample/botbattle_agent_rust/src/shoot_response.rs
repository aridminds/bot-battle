use extism_pdk::*;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, ToBytes, FromBytes)]
#[encoding(Json)]
pub(crate) struct AgentShootResponse {
    #[serde(rename = "Action")]
    pub(crate) action: Shoot,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "$type")]
pub(crate) struct Shoot {
    #[serde(rename = "Power")]
    pub(crate) power: i16,
    #[serde(rename = "Weapon")]
    pub(crate) weapon: String,
    #[serde(rename = "Id")]
    pub(crate) id: String,
}
