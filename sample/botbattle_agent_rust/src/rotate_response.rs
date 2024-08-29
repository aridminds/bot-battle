use extism_pdk::*;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, ToBytes, FromBytes)]
#[encoding(Json)]
pub(crate) struct AgentRotateResponse {
    #[serde(rename = "Action")]
    pub(crate) action: Rotate,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "$type")]
pub(crate) struct Rotate {
    #[serde(rename = "Direction")]
    pub(crate) direction: i16,
    #[serde(rename = "Id")]
    pub(crate) id: String,
}
