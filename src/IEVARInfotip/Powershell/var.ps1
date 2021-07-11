function GetData ([IO.BinaryReader]$fs)
{
  $fs.BaseStream.Seek(0x002c, 'Begin');
  $scope = $fs.ReadChars(8);
  $scope = -Join $scope

  return "Scope: ${scope}"
}