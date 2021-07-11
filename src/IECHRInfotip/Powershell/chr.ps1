function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $name = $fs.ReadChars(32);
  $name = -join $name

  return "Name: ${name}";
}