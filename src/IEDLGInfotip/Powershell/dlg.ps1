function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $stateCount = $fs.ReadInt32();

  return "State count: ${stateCount}";
}