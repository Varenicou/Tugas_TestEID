#r "System.IO"
using System;
using System.IO;

string uploadPath = "/mnt/e/Interview/TugasASPNET_PID/Tugas_TestEID/Tugas_Test_EID/Tugas_Test_EID/wwwroot/uploads";

Console.WriteLine($"Testing Write Access: {uploadPath}");
try
{
    var testFile = Path.Combine(uploadPath, "testfile.txt");
    File.WriteAllText(testFile, "test");
    Console.WriteLine("Write Success!");
}
catch (Exception ex)
{
    Console.WriteLine($"Write Failed: {ex.Message}");
}
