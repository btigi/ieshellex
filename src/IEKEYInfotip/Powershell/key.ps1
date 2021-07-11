function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $bifEntryCount = $fs.ReadInt32();

  return "Bif Entry Count: ${$bifEntryCount}

${thisstrref}";
}