function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0014, 'Begin');
  $volumeVariance = $fs.ReadInt32();

  return "Volume variance: ${volumeVariance}";
}