function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0000, 'Begin');
  $effect = $fs.ReadInt16();

  return "Effect: ${effect}";
}