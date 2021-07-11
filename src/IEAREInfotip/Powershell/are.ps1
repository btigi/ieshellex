function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0058, 'Begin');
  $actorCount = $fs.ReadInt16();

  return "Actor count: ${actorCount}";
}