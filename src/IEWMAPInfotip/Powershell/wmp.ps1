function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $entryCount = $fs.ReadInt32();

  return "Entry count: ${entryCount}";
}