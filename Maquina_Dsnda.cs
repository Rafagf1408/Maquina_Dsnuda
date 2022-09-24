using System;
using System.Management;

Console.WriteLine($"Hello {user()}...");

string user()
    => Environment.UserName;

ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

foreach (ManagementObject obj in myVideoObject.Get())
{
    Console.WriteLine("Name  -  " + obj["Name"]);
    Console.WriteLine("Status  -  " + obj["Status"]);
    Console.WriteLine("Caption  -  " + obj["Caption"]);
    Console.WriteLine("DeviceID  -  " + obj["DeviceID"]);
    Console.WriteLine("AdapterRAM  -  " + obj["AdapterRAM"]);
    Console.WriteLine("AdapterDACType  -  " + obj["AdapterDACType"]);
    Console.WriteLine("Monochrome  -  " + obj["Monochrome"]);
    Console.WriteLine("InstalledDisplayDrivers  -  " + obj["InstalledDisplayDrivers"]);
    Console.WriteLine("DriverVersion  -  " + obj["DriverVersion"]);
    Console.WriteLine("VideoProcessor  -  " + obj["VideoProcessor"]);
    Console.WriteLine("VideoArchitecture  -  " + obj["VideoArchitecture"]);
    Console.WriteLine("VideoMemoryType  -  " + obj["VideoMemoryType"]);


}
