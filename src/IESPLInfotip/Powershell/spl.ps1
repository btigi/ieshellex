function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x0034, 'Begin');
  $price = $fs.ReadInt32();
  $fs.BaseStream.Seek(0x0008, 'Begin');
  $uname = $fs.ReadInt32();

  $fs.BaseStream.Seek(0x003a, 'Begin');
  $invicon = $fs.ReadChars(8);
  $invicon = -join $invicon

  $fs.BaseStream.Seek(0x0064, 'Begin');
  $extendedHeaderOffset = $fs.ReadInt32();
  $fs.BaseStream.Seek(0x0068, 'Begin');
  $extendedHeaderCount = $fs.ReadInt16();

  $extendedHeaderOutput = "";
  for ($i=0; $i -lt $extendedHeaderCount; $i++)
  {
    $extendedHeaderOutput = $extendedHeaderOutput + "Header ${i}";
    $thisExtendedHeaderBaseOffset = $extendedHeaderOffset + $i * 0x0038;

    $fs.BaseStream.Seek($thisExtendedHeaderBaseOffset + 0x0000, 'Begin');
    $type = $fs.ReadByte();
    switch($type){
      "0" {$type = "None"}
      "1" {$type = "Melee"}
      "2" {$type = "Ranged"}
      "3" {$type = "Magical"}
      "4" {$type = "Launcher"}
    }
    $extendedHeaderOutput = $extendedHeaderOutput + [System.Environment]::NewLine + " Type: ${type}";

    $fs.BaseStream.Seek($thisExtendedHeaderBaseOffset + 0x0018, 'Begin');
    $diceThrown = $fs.ReadByte();
    $extendedHeaderOutput = $extendedHeaderOutput + [System.Environment]::NewLine + " Dice thrown: ${diceThrown}";

    $fs.BaseStream.Seek($thisExtendedHeaderBaseOffset + 0x0004, 'Begin');
    $icon = $fs.ReadChars(8);
    $icon = -join $icon
    $extendedHeaderOutput = $extendedHeaderOutput + [System.Environment]::NewLine + " Icon: ${icon}";

    $extendedHeaderOutput = $extendedHeaderOutput + [System.Environment]::NewLine;
  }

  return "Unidentified name: @${uname} (#${uname})
${extendedHeaderOutput}
${invicon}";
}