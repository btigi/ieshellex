function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $projectileType = $fs.ReadInt16();

  switch($projectileType){
    "0" {$projectileType = "No Projectile"}
    "1" {$projectileType = "No BAM"}
    "2" {$projectileType = "Single Target"}
    "3" {$projectileType = "Area of Effect"}
  }

  return "Type: ${projectileType}";
}