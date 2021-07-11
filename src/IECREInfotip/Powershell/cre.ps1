function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $name = $fs.ReadInt32();

  return "Name: @${name} (#${name})";
}