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

            

            if (myProcessorObject.Get() == null)
            {
                Console.WriteLine("Error de Procesador");
                SystemSounds.Hand.Play();
            }
            else
            {

                foreach (ManagementObject obj in myProcessorObject.Get())
                {
                    Console.WriteLine("Name  -  " + obj["Name"]);
                    switch (obj["Architecture"].ToString())
                    {
                        case "0":
                            Console.WriteLine("Architecture  -  x86");
                            break;
                        case "1":
                            Console.WriteLine("Architecture  -  MIPS");
                            break;
                        case "2":
                            Console.WriteLine("Architecture  -  Alpha");
                            break;
                        case "3":
                            Console.WriteLine("Architecture  -  PowerPC");
                            break;
                        case "5":
                            Console.WriteLine("Architecture  -  ARM");
                            break;
                        case "6":
                            Console.WriteLine("Architecture  -  ia64(Itanium-based systems)");
                            break;
                        case "9":
                            Console.WriteLine("Architecture  -  x64");
                            break;
                    }
                    Console.WriteLine("PROCESADOR  OK!\n\n");


                }

                if (myVideoObject.Get() == null)
                {
                    Console.WriteLine("Error de Tarjeta de Video");
                    SystemSounds.Hand.Play();
                }
                else
                {

                    foreach (ManagementObject obj in myVideoObject.Get())
                    {
                        Console.WriteLine("Name  -  " + obj["Name"]);
                        if (obj["Status"].Equals("OK"))
                        {
                            Console.WriteLine("TARJETA DE VIDEO  OK!!!\n");
                            string strComputer = ".";
                            ManagementScope scope = new ManagementScope(@"\\" + strComputer + @"\root\cimv2");
                            ObjectQuery queryUsbControllers = new ObjectQuery("Select * From Win32_USBControllerDevice");
                            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, queryUsbControllers);
                            ManagementObjectCollection usbControllers = searcher.Get();

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
                                if (usbDevices.Count == null)
                                {
                                    Console.WriteLine("ERROR Mouse");
                                    SystemSounds.Hand.Play();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("MOUSE  OK!\n");
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

                                if (usbDevices.Count == null)
                                {
                                    Console.WriteLine("ERROR TECLADO");
                                    erro = true;
                                    SystemSounds.Hand.Play();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("TECLADO  OK!\n");

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
                                        erro = false;
                                        break;
                                    }
                                    
                                    
                                    
                                }
                                if (!erro)
                                {
                                    foreach (ManagementObject obj3 in myAudioObject.Get())
                                    {
                                        /*Console.WriteLine("Name  -  " + obj3["Name"]);
                                        Console.WriteLine("ProductName  -  " + obj3["ProductName"]);
                                        Console.WriteLine("Availability  -  " + obj3["Availability"]);
                                        Console.WriteLine("DeviceID  -  " + obj3["DeviceID"]);
                                        Console.WriteLine("PowerManagementSupported  -  " + obj3["PowerManagementSupported"]);*/
                                        Console.WriteLine("TajAudioStatus: " + obj3["Status"]);
                                        if (obj3["Status"].Equals("OK")) {
                                            erro = true;
                                            break;
                                        }

                                        /*Console.WriteLine("StatusInfo  -  " + obj3["StatusInfo"]);
                                        Console.WriteLine(String.Empty.PadLeft(obj3["ProductName"].ToString().Length, '='));*/
                                    }
                                    if (erro)
                                    {
                                        SystemSounds.Exclamation.Play();
                                       
                                    } else {
                                        
                                        Console.WriteLine("ERROR AUDIO");
                                        
                                    }
                                    if (nics == null || nics.Length < 1)
                                    {
                                        Console.WriteLine("  No network interfaces found.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("-----------------------RED---------------------------");
                                        foreach (NetworkInterface adapter in nics)
                                        {
                                            IPInterfaceProperties properties = adapter.GetIPProperties();
                                            Console.WriteLine(adapter.Description);
                                            if (adapter.OperationalStatus.ToString()=="Up") { Console.WriteLine("Operational status:    OK"); }
                                          
                                        }
                                        Console.WriteLine("----------------------------------------------------");
                                    }

                                    Console.WriteLine("\n    ");
                                    foreach (DriveInfo d in allDrives)
                                    {
                                        Console.WriteLine("Drive {0}", d.Name);
                                        Console.WriteLine("  Drive type: {0}", d.DriveType);
                                        if (d.IsReady == true)
                                        {
                                            erro= false;
                                            Console.WriteLine("Unidad Existente y lista para su utilizacion");
                                            Console.WriteLine("  Total size of drive:            {0, 15} bytes \n", d.TotalSize);

                                        }
                                        else
                                        {
                                            Console.WriteLine(" Unidad desabilitada Y/O Error");
                                            SystemSounds.Hand.Play();
                                        }
                                    }
                                    if (!erro) {
                                        foreach (ManagementObject obj2 in myOperativeSystemObject.Get())
                                        {
                                            Console.WriteLine("SYSTEM OPERATIVE:  " + obj2["Caption"]);
                                            
                                            /*Console.WriteLine("OSType  -  " + obj2["OSType"]);(Desconocido(0),Otros(1),MACOS(2),ATTUNIX(3),DGUX(4),DECNT(5),Unix digital(6),OpenVMS(7),
                                             *HPUX(8),AIX(9),MVS(10),OS400(11),SO / 2(12),JavaVM(13),MSDOS(14),WIN3x(15),WIN95(16),WIN98(17),WINNT(18),WINCE(19),NCR3000(20),NetWare(21),
                                             *OSF(22),DC / OS(23),UNIX de Reliant(24),SCO UnixWare(25),SCO OpenServer(26),Sequent(27),IRIX(28),Solaris(29),SunOS(30),U6000(31),ASERIES(32),
                                             *TándemNSK(33),TándemNT(34),BS2000(35),LINUX(36),Lynx(37),XENIX(38),VM / ESA(39),UNIX interactivo(40),BSDUNIX(41),FreeBSD(42),NetBSD(43),GNU Hurd(44),
                                             *OS9(45),Kernel mach(46),Inferno(47),QNX(48),EPOC(49),IxWorks(50),VxWorks(51),MiNT(52),BeOS(53),HP MPE(54),NextStep(55),PalmPilot(56),Rhapsody(57),
                                             Windows 2000(58),Dedicado(59),SO / 390(60),VSE(61),TPF(62)/
                                        }
                                    } else {
                                        SystemSounds.Hand.Play(); 
                                        Console.WriteLine("NO HAY SISTEMA OPERATIVO"); }
                                    
                                }
                                else {
                                    SystemSounds.Hand.Play();
                                    Console.WriteLine("ERROR RAM");
                                    Console.ReadLine();
                                }

                                
                            }
                            else
                            {
                                SystemSounds.Hand.Play();
                                Console.WriteLine("Presione una tecla para continuar");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine(" ERROR EN TARJETA DE VIDEO");
                            SystemSounds.Hand.Play();
                            SystemSounds.Hand.Play();
                        }



                    }

                }



                Console.ReadLine();
            }
        }
    }
}
