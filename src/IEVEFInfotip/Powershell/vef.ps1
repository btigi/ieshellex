function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $offset1= $fs.ReadInt32();

  $fs.BaseStream.Seek(0x000c, 'Begin');
  $count1= $fs.ReadInt32();

  $fs.BaseStream.Seek(0x0010, 'Begin');
  $offset2= $fs.ReadInt32();

  $fs.BaseStream.Seek(0x0014, 'Begin');
  $count2= $fs.ReadInt32();

  return "Offset 1: ${offset1}
Count 1: ${count1}
Offset 2: ${offset2}
Count 2: ${count2}";
}