use extism_pdk::*;
use serde_repr::*;

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct AgentRequest {
    #[serde(rename = "Arena")]
    pub arena: Arena,
    #[serde(rename = "MyTank")]
    pub my_tank: Tank,
    #[serde(rename = "Hash")]
    pub hash: String,
}

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct Arena {
    #[serde(rename = "Width")]
    pub width: i16,
    #[serde(rename = "Height")]
    pub height: i16,
    #[serde(rename = "Turn")]
    pub turn: i16,
    #[serde(rename = "Tanks")]
    pub tanks: Vec<Tank>,
    #[serde(rename = "Bullets")]
    pub bullets: Vec<Bullet>,
    #[serde(rename = "Obstacles")]
    pub obstacles: Vec<Obstacle>,
}

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct Obstacle {
    #[serde(rename = "Type")]
    pub obstacle_type: ObstacleType,
    #[serde(rename = "Position")]
    pub position: Option<Position>,
    #[serde(rename = "Direction")]
    pub direction: Direction,
}

#[derive(Serialize_repr, Deserialize_repr, PartialEq, Debug)]
#[allow(dead_code)]
#[repr(u8)]
pub enum ObstacleType {
    Destroyed = 0,
    TreeLeaf = 1,
    TreeSmall = 2,
    TreeLarge = 3,
    Stone = 4,
    OilBarrel = 5,
    OilStain = 6,
}

#[derive(Serialize_repr, Deserialize_repr, PartialEq, Debug)]
#[allow(dead_code)]
#[repr(u8)]
pub enum Direction {
    North = 0,
    NorthEast = 1,
    East = 2,
    SouthEast = 3,
    South = 4,
    SouthWest = 5,
    West = 6,
    NorthWest = 7,
}

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct Bullet {
    #[serde(rename = "Owner")]
    pub owner: Tank,
    #[serde(rename = "Position")]
    pub position: Option<Position>,
    #[serde(rename = "Direction")]
    pub direction: Direction,
}

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct Position {
    #[serde(rename = "X")]
    pub x: i16,
    #[serde(rename = "Y")]
    pub y: i16,
}

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct Tank {
    #[serde(rename = "Name")]
    pub name: String,
    #[serde(rename = "Health")]
    pub health: i16,
    #[serde(rename = "WeaponSystem")]
    pub weapon_system: Option<WeaponSystem>,
    #[serde(rename = "Position")]
    pub position: Option<Position>,
    #[serde(rename = "Direction")]
    pub direction: Direction,
}

#[derive(Debug, serde::Deserialize, FromBytes)]
#[encoding(Json)]
#[allow(dead_code)]
pub struct WeaponSystem {
    #[serde(rename = "Id")]
    pub id: String,
    #[serde(rename = "Bullet")]
    pub bullet: BulletType,
    #[serde(rename = "FireCooldown")]
    pub fire_cooldown: f32,
    #[serde(rename = "ActiveFireCooldown")]
    pub active_fire_cooldown: f32,
    #[serde(rename = "CanShoot")]
    can_shoot: bool,
}

#[derive(Serialize_repr, Deserialize_repr, PartialEq, Debug)]
#[allow(dead_code)]
#[repr(u8)]
pub enum BulletType {
    Standard = 0,
}
