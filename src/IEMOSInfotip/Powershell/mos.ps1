function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $width = $fs.ReadInt16();
  $fs.BaseStream.Seek(0x000a, 'Begin');
  $height = $fs.ReadInt16();

  return "Width: ${width}
Height: ${height}";
}