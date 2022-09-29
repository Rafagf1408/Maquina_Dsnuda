using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;



namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Boolean erro = false;
            ManagementObjectSearcher myOperativeSystemObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");
            ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");
            ManagementObjectSearcher myAudioObject = new ManagementObjectSearcher("select * from Win32_SoundDevice");
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            var arch = new List<string> { "Architecture  -  x86", "Architecture  -  MIPS", "Architecture  -  Alpha", "Architecture  -  POWERPC", "4","Architecture  -  ARM", "Architecture  -  ia64(Itanium-based systems)", "7", "8", "Architecture  -  x64" };


            Esc("--------------------PROCESSOR--------------------------------");
            if (myProcessorObject.Get() == null)
            {
                Esc("Error de Procesador");
                SystemSounds.Hand.Play();
            }
            else
            {

                foreach (ManagementObject obj in myProcessorObject.Get())
                {
                    Esc("Name  -  " + obj["Name"]);
                    int ind = int.Parse(obj["Architecture"].ToString());
                    Console.WriteLine(arch[ind]);
                    Esc("PROCESADOR  OK!");


                }
                Esc("----------------------------------------------------");
                Esc("----------------------VIDEOCARD------------------------------");

                if (myVideoObject.Get() == null)
                {
                    Esc("Error de Tarjeta de Video");
                    SystemSounds.Hand.Play();
                }
                else
                {

                    foreach (ManagementObject obj in myVideoObject.Get())
                    {
                        Esc("Name  -  " + obj["Name"]);
                        if (obj["Status"].Equals("OK"))
                        {
                            Esc("TARJETA DE VIDEO  OK!!!\n");
                            Esc("----------------------------------------------------");
                            string strComputer = ".";
                            ManagementScope scope = new ManagementScope(@"\\" + strComputer + @"\root\cimv2");
                            ObjectQuery queryUsbControllers = new ObjectQuery("Select * From Win32_USBControllerDevice");
                            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, queryUsbControllers);
                            ManagementObjectCollection usbControllers = searcher.Get();
                            Esc("-------------------MOUSE AND KEYBOARD-------------------------");
                            foreach (ManagementObject usbController in usbControllers)
                            {
                                string dependent = (string)usbController["Dependent"];
                                string[] names = dependent.Replace("\"", "").Split(new char[] { '=' });
                                string strUsbControllerName = names[1];
                                ObjectQuery queryUsbDevices = new ObjectQuery("Select * From Win32_PnPEntity Where DeviceID = '" + strUsbControllerName + "' and Description = 'Mouse compatible con HID'");
                                ManagementObjectSearcher deviceSearcher = new ManagementObjectSearcher(scope, queryUsbDevices);
                                ManagementObjectCollection usbDevices = deviceSearcher.Get();
                                if (usbDevices.Count == 0)
                                    continue;
                                foreach (ManagementObject usbDevice in usbDevices)
                                {
                                    Console.WriteLine("description = {0}", usbDevice["Description"]);

                                }
                                if (usbDevices.Count == 0)
                                {
                                    Esc("ERROR Mouse");
                                    SystemSounds.Hand.Play();
                                    break;
                                }
                                else
                                {
                                    Esc("MOUSE  OK!\n");
                                    break;
                                }

                            }
                            foreach (ManagementObject usbController in usbControllers)
                            {
                                string dependent = (string)usbController["Dependent"];
                                string[] names = dependent.Replace("\"", "").Split(new char[] { '=' });
                                string strUsbControllerName = names[1];
                                ObjectQuery queryUsbDevices = new ObjectQuery("Select * From Win32_PnPEntity Where DeviceID = '" + strUsbControllerName + "' and Description = 'Dispositivo de teclado HID'");
                                ManagementObjectSearcher deviceSearcher = new ManagementObjectSearcher(scope, queryUsbDevices);
                                ManagementObjectCollection usbDevices = deviceSearcher.Get();
                                if (usbDevices.Count == 0)
                                    continue;
                                foreach (ManagementObject usbDevice in usbDevices)
                                {
                                    Console.WriteLine("description = {0}", usbDevice["Description"]);
                                }

                                if (usbDevices.Count == 0)
                                {
                                    Esc("ERROR TECLADO");
                                    erro = true;
                                    SystemSounds.Hand.Play();
                                    break;
                                }
                                else
                                {
                                    Esc("TECLADO  OK!\n");
                                    break;
                                }

                            }
                            if (!erro)
                            {
                                foreach (ManagementObject obj2 in myOperativeSystemObject.Get())
                                {
                                    //Console.WriteLine("Total Visible Memory: {0} KB", obj2["TotalVisibleMemorySize"]);
                                    //Console.WriteLine(Int64.Parse(obj2["TotalVisibleMemorySize"].ToString()));
                                    //Console.WriteLine("Free Physical Memory: {0} KB", obj2["FreePhysicalMemory"]);
                                    if ((Int64.Parse(obj2["TotalVisibleMemorySize"].ToString()) >= 1) && (Int64.Parse(obj2["FreePhysicalMemory"].ToString()) >= 1)) {
                                        erro = true;
                                        break;
                                    }
                                    
                                    
                                    
                                }
                                Esc("----------------------------------------------------");
                                if (erro)
                                {

                                    Esc("-----------------------AUDIO---------------------------");
                                    foreach (ManagementObject obj3 in myAudioObject.Get())
                                    {
                                      Console.WriteLine("TajAudioStatus: " + obj3["Status"]);
                                        if (obj3["Status"].Equals("OK")) {
                                            erro = false;
                                            break;
                                        }

                                    }
                                    if (!erro)
                                    {
                                        SystemSounds.Exclamation.Play();
                                       
                                    } else {
                                        
                                        Esc("ERROR AUDIO");
                                        
                                    }
                                    Esc("------------------------------------------------------");
                                    Esc("-----------------------RED---------------------------");
                                    if (nics == null || nics.Length < 1)
                                    {
                                        Esc("  No network interfaces found.");
                                    }
                                    else
                                    {
                                       
                                        foreach (NetworkInterface adapter in nics)
                                        {
                                            IPInterfaceProperties properties = adapter.GetIPProperties();
                                            Esc(adapter.Description);
                                            if (adapter.OperationalStatus.ToString()=="Up") { Console.WriteLine("Operational status:    OK"); }
                                          
                                        }
                                        Esc("----------------------------------------------------");
                                    }

                                    Esc("----------------------HDD & SSD------------------------------");
                                    foreach (DriveInfo d in allDrives)
                                    {
                                        Console.WriteLine("Drive {0}", d.Name);
                                        Console.WriteLine("  Drive type: {0}", d.DriveType);
                                        if (d.IsReady == true)
                                        {
                                            erro= true;
                                            Esc("Unidad Existente y lista para su utilizacion");
                                            Console.WriteLine("  Total size of drive:            {0, 15} bytes \n", d.TotalSize);
                                        }
                                        else
                                        {
                                            Esc(" Unidad desabilitada Y/O Error");
                                            SystemSounds.Hand.Play();
                                        }
                                    }
                                    Esc("--------------------------------------------------------------");
                                    Esc("-------------------------OS-------------------------------------");
                                    if (erro) {
                                        foreach (ManagementObject obj2 in myOperativeSystemObject.Get())
                                        {
                                            Esc("SYSTEM OPERATIVE:  " + obj2["Caption"]);
                                          
                                        }
                                    } else {
                                        SystemSounds.Hand.Play(); 
                                        Esc("NO HAY SISTEMA OPERATIVO"); }
                                    Esc("--------------------------------------------------------------");

                                }
                                else {
                                    SystemSounds.Hand.Play();
                                    Esc("ERROR RAM");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                SystemSounds.Hand.Play();
                                Esc("Presione una tecla para continuar");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Esc(" ERROR EN TARJETA DE VIDEO");
                            SystemSounds.Hand.Play();
                            SystemSounds.Hand.Play();
                        }
                    }
                }
                Esc("---------------------------END-------------------------PRESS ENTER KEY----------");
                Console.ReadLine();
            }
        }
        static void Esc(string mensaje)
        {
            Console.WriteLine(mensaje);
        }
    }
}
