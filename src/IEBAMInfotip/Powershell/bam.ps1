function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $frameCount = $fs.ReadInt32();

  return "Frame count: ${frameCount}";
}