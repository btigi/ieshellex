function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $windowCount = $fs.ReadInt32();

  return "Window count: ${windowCount}";
}