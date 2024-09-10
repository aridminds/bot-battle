use extism_pdk::*;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, ToBytes, FromBytes)]
#[encoding(Json)]
pub struct AgentDriveResponse {
    #[serde(rename = "Action")]
    pub action: Drive,
}
#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "$type")]
pub struct Drive {
    #[serde(rename = "Id")]
    pub id: String,
}
