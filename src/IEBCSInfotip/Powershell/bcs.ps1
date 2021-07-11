function GetData ([IO.StreamReader]$fs)
{
  $lineCount = 0;
  $result = "";

  while (-not $fs.EndOfStream -and $lineCount -lt 50)
  {
    $thisLine = $fs.ReadLine();
    if ($lineCount -gt 0)
    {
      $result = $result + [Environment]::NewLine + $thisLine
    }
    else
    {
      $result = $thisLine
    }

    $lineCount = $lineCount + 1;
    if([string]::IsNullOrEmpty($thisLine))   
    {
      break;
    }
  }

  return $result;
}