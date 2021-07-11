function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $tileCount = $fs.ReadInt32();
  $fs.BaseStream.Seek(0x000c, 'Begin');
  $tisType = $fs.ReadInt32();

  switch($tisType){
    "12" {$tisType = "Classic"}
    "5120" {$tisType = "PVRZ"}
  }

  return "Tile Count: ${tileCount}
Type: ${tisType}";
}