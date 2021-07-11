using IEMENUInfotip.Model;
using SharpShell.Attributes;
using SharpShell.SharpInfoTipHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Serialization;

namespace IEMENUInfotip
{
    /// <summary>
    /// This handler shows configurable information from an Infinity Engine MENU file
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".menu")]
    public class MenuInfotipHandler : SharpInfoTipHandler
    {
        protected override string GetInfo(RequestedInfoType infoType, bool singleLine)
        {
            switch (infoType)
            {
                case RequestedInfoType.InfoTip:
                    return HandleRequest(SelectedItemPath, singleLine);
                case RequestedInfoType.Name:
                    return string.Format("Folder '{0}'", Path.GetFileName(SelectedItemPath));
                default:
                    return string.Empty;
            }
        }

        public string HandleRequest(string filename, bool singleLine)
        {
            var tlkLocationDatas = new List<TlkLocationData>();
            var serializer = new XmlSerializer(tlkLocationDatas.GetType());
            var path = Path.Combine(Environment.GetEnvironmentVariable("ieshellex", EnvironmentVariableTarget.Machine), "config", "tlkLocations.xml");
            var tlkfs = new FileStream(path, FileMode.Open, FileAccess.Read);
            tlkLocationDatas = (List<TlkLocationData>)serializer.Deserialize(tlkfs);
            var tlkLocationData = GetTlkLocation(tlkLocationDatas, filename);

            var data = ProcessFile(filename, tlkLocationData);
            return singleLine ? data.Replace(Environment.NewLine, "") : data;
        }

        public string GetTlkLocation(List<TlkLocationData> tlkLocationDatas, string filename)
        {
            var maxLength = 0;
            var tlkLocation = String.Empty;
            var target = Path.GetDirectoryName(filename).ToUpper();
            foreach (var tlkLocationData in tlkLocationDatas)
            {
                if (target.ToUpper().IndexOf(tlkLocationData.ApplyPath.ToUpper()) > -1 && tlkLocationData.ApplyPath.Length > maxLength)
                {
                    tlkLocation = tlkLocationData.TlkPath;
                }
            }
            return tlkLocation;
        }

        public string ProcessFile(string filename, string tlkLocationData)
        {
            var result = "";
            using (PowerShell PowerShellInst = PowerShell.Create())
            {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (var br = new StreamReader(fs))
                {
                    var path = Path.Combine(Environment.GetEnvironmentVariable("ieshellex", EnvironmentVariableTarget.Machine), "config", "menu.ps1");
                    if (!string.IsNullOrEmpty(path))
                    {
                        PowerShellInst.AddScript(File.ReadAllText(path), false);
                        PowerShellInst.Invoke();
                        PowerShellInst.AddCommand("GetData");
                        PowerShellInst.AddParameter("fs", br);
                    }

                    var PSOutput = PowerShellInst.Invoke();
                    var powershellResult = PSOutput.Last().ToString();

                    var regex = new Regex(@"\@\d+");
                    var strrrefs = regex.Matches(powershellResult);
                    foreach (var strrref in strrrefs)
                    {
                        var text = GetString(tlkLocationData, Convert.ToInt32(strrref.ToString().Trim('@')));
                        powershellResult = powershellResult.Replace(strrref.ToString(), text);
                    }

                    result = powershellResult;
                    // .NET will close and dispose of these managed resources automatically, we we want to ensure they are closed ASAP
                    br.Close();
                    fs.Close();
                }
            }
            return result;
        }

        private string GetString(string tlkLocation, int strRef)
        {
            using (var filestream = new FileStream(tlkLocation, FileMode.Open))
            using (var binaryStream = new BinaryReader(filestream))
            {
                binaryStream.BaseStream.Seek(0x000a, SeekOrigin.Begin);
                var strRefCount = binaryStream.ReadInt32();
                if (strRef <= strRefCount)
                {

                    binaryStream.BaseStream.Seek(0x000e, SeekOrigin.Begin);
                    var stringOffset = binaryStream.ReadInt32();

                    const int FileHeaderLength = 18;
                    const int EntryDataLength = 0x001a;
                    const int IrrelevantBytesInStruct = 18;
                    binaryStream.BaseStream.Seek(FileHeaderLength + (strRef * EntryDataLength) + IrrelevantBytesInStruct, SeekOrigin.Begin);

                    var stringIndex = binaryStream.ReadInt32();
                    var stringLength = binaryStream.ReadInt32();

                    binaryStream.BaseStream.Seek(stringOffset + stringIndex, SeekOrigin.Begin);
                    var str = binaryStream.ReadBytes(stringLength);
                    // .NET will close and dispose of these managed resources automatically, we we want to ensure they are closed ASAP
                    binaryStream.Close();
                    filestream.Close();
                    return System.Text.Encoding.UTF8.GetString(str);
                }
                return String.Empty;
            }
        }
    }
}