use extism_pdk::*;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize, ToBytes, FromBytes)]
#[encoding(Json)]
pub(crate) struct AgentDriveResponse {
    #[serde(rename = "Action")]
    pub(crate) action: Drive,
}
#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "$type")]
pub(crate) struct Drive {
    #[serde(rename = "Id")]
    pub(crate) id: String,
}
