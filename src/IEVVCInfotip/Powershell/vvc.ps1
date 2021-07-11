function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $animationBam = $fs.ReadChars(8);
  $animationBam -join $animationBam

  return "Animation: ${$animationBam}";
}