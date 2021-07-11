function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x000c, 'Begin');
  $name = $fs.ReadInt32();

  $fs.BaseStream.Seek(0x0008, 'Begin');
  $type = $fs.ReadInt32();
  switch($type){
    "0" {$type = "Store"}
    "1" {$type = "Tavern"}
    "2" {$type = "Inn"}
    "3" {$type = "Temple"}
    "5" {$type = "Container"}
  }

  $fs.BaseStream.Seek(0x003c, 'Begin');
  $lore = $fs.ReadInt32();


  return "Name: @${name} (#${name})
Type: ${type}
Lore: ${lore}";
}