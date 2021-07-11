function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x000c, 'Begin');
  $doorCount = $fs.ReadInt32();

  return "Door count: ${doorCount}";
}