using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferCNAB.Business
{
    public class Decrypt
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int fDecodeKeyFile(string filePath, string senha, byte[] key, StringBuilder errorMsg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate IntPtr fInitDecoder(string filename, string directory, byte[] keyInput, StringBuilder msgErro);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int fReadData(IntPtr handler, byte[] data, int dataLen, StringBuilder msgErro);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int fDecodeBuffer(byte[] BufferCriptografado, int tamBufferCriptografado, byte[] BufferSaidaDecriptografado, int tamBufferSaidaDecriptografado, byte[] key);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int fCloseDecoder(IntPtr handler, StringBuilder msgErro);

        public void Decriptografa()
        {
            string fileName = "CBDecrypt.RST";
            StringBuilder msgErro = new StringBuilder(512);
            byte[] key = new byte[16];


            var zipDll = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Dll\zlib1.dll";
            IntPtr zibDll = NativeMethods.LoadLibrary(zipDll);

            var pathDLL = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Dll\WEBTAEncoderLib.dll";
            IntPtr pDll = NativeMethods.LoadLibrary(pathDLL);

            //oh dear, error handling here
            if (pDll == IntPtr.Zero)
            {
                //log error
            }

            IntPtr pAdrressDecodeKey = NativeMethods.GetProcAddress(pDll, "fDecodeKeyFile");
            //oh dear, error handling here
            if (pAdrressDecodeKey == IntPtr.Zero)
            {
                //log error
            }

            fDecodeKeyFile decodeKey = (fDecodeKeyFile)Marshal.GetDelegateForFunctionPointer(pAdrressDecodeKey, typeof(fDecodeKeyFile));


            var theResult = decodeKey(@"C:\\Temp\\WebTA\\Criptografia\\criptografia201501071224.bin", "gomes2015", key, msgErro);

            if (theResult == 1)
            {
                IntPtr pAdrressInitDecode = NativeMethods.GetProcAddress(pDll, "fInitDecoder");
                fInitDecoder initDecode = (fInitDecoder)Marshal.GetDelegateForFunctionPointer(pAdrressInitDecode, typeof(fInitDecoder));

                IntPtr criptoHandle = initDecode(fileName, @"C:\\Temp\\WebTA\\Arquivo", key, msgErro);

                if (criptoHandle != null && criptoHandle != IntPtr.Zero)
                {
                    IntPtr pAdrressReadData = NativeMethods.GetProcAddress(pDll, "fReadData");
                    fReadData readData = (fReadData)Marshal.GetDelegateForFunctionPointer(pAdrressReadData, typeof(fReadData));

                    IntPtr pAdrressBufferReadData = NativeMethods.GetProcAddress(pDll, "fDecodeBuffer");
                    fDecodeBuffer readBufferData = (fDecodeBuffer)Marshal.GetDelegateForFunctionPointer(pAdrressBufferReadData, typeof(fDecodeBuffer));

                    int bytesLidos = 1;

                    while (bytesLidos > 0)
                    {
                        byte[] bufferAux = new byte[8192];

                        bytesLidos = readData(criptoHandle, bufferAux, bufferAux.Length, msgErro);

                        if (bytesLidos > 0)
                        {

                        }
                    }

                    /* while (bytesLidos > 0)
                     {

                         //using (BinaryReader b = new BinaryReader(File.Open(@"C:\\Temp\\WebTA\\Arquivo\\CB020901.TST", FileMode.Open, FileAccess.Read)))
                         //{
                         //    // Read bytes from stream and interpret them as ints
                         //    int pos = 0;
                         //    // 2A.
                         //    // Use BaseStream.
                         //    int length = (int)b.BaseStream.Length;
                         //    // Read from the IO stream fewer times.
                         //    byte[] dataArray = new byte[8192];
                         //    dataArray = b.ReadBytes(8192);

                         //    bytesLidos = readData(criptoHandle, dataArray, dataArray.Length, msgErro);

                         //    //var i = readBufferData(dataArray, dataArray.Length, dataArray, dataArray.Length, key);
                         //    //for (int i = 0; i < count; i++)
                         //    //{
                         //    //    Console.WriteLine(Convert.ToInt32(buffer[i]));
                         //    //}
                         //}



                         FileStream fileStream = new FileStream(@"C:\\Temp\\WebTA\\Arquivo\\CB020901.TST", FileMode.Open, FileAccess.Read);
                         try
                         {
                             int length = (int)fileStream.Length;  // get file length
                             byte[] buffer = new byte[length];            // create buffer
                             int count;                            // actual number of bytes read
                             int sum = 0;                          // total number of bytes read

                             // read until Read method returns 0 (end of the stream has been reached)
                             while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                             {
                                 bytesLidos = readData(criptoHandle, buffer, length, msgErro);

                                 if (bytesLidos > 0)
                                 {

                                 }

                                 sum += count;  // sum is a buffer offset for next reading
                             }
                         }
                         finally
                         {
                             fileStream.Close();
                         }
                     }*/
                    //IntPtr pAdrressCloseDecoder = NativeMethods.GetProcAddress(pDll, "fCloseDecoder");
                    //fCloseDecoder closeHandle = (fCloseDecoder)Marshal.GetDelegateForFunctionPointer(pAdrressCloseDecoder, typeof(fCloseDecoder));

                    //if (bytesLidos == 0) //Erro                    
                    //    closeHandle(criptoHandle, msgErro);
                    //else
                    //    closeHandle(criptoHandle, msgErro);

                }
            }
        }
    }
}
