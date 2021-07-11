function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x000a, 'Begin');
  $strrefCount = $fs.ReadInt32();

  $fs.BaseStream.Seek(0x000e, 'Begin');
  $strrefOffset = $fs.ReadInt32();


  $strref = Get-Random -Minimum 0 -Maximum $strrefCount
  $fs.BaseStream.Seek(18 + ($strref * 0x001a) + 0x0012, 'Begin');
  $thisstrrefOffset = $fs.ReadInt32();

  $fs.BaseStream.Seek(18 + ($strref * 0x001a) + 0x0016, 'Begin');
  $thisstrrefLength = $fs.ReadInt32();

  $fs.BaseStream.Seek($strrefOffset + $thisstrrefOffset, 'Begin');
  $thisstrref = $fs.ReadChars($thisstrrefLength);
  $thisstrref = -join $thisstrref


  return "StrRef Count: ${strrefCount}

${thisstrref}";
}