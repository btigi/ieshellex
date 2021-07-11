function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0018, 'Begin');
  $gold = $fs.ReadInt32();

  return "Gold: ${gold}gp";
}