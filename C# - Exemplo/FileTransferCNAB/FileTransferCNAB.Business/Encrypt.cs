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
    public class Encrypt
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int fDecodeKeyFile([MarshalAs(UnmanagedType.LPStr)]string filePath, [MarshalAs(UnmanagedType.LPStr)] string senha, byte[] key, [MarshalAs(UnmanagedType.LPStr)]StringBuilder msgErro);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int fDecodeKeyFileEx([MarshalAs(UnmanagedType.LPStr)]string filePath, [MarshalAs(UnmanagedType.LPStr)] string senha, [MarshalAs(UnmanagedType.LPStr)]StringBuilder idCliente, byte[] key, [MarshalAs(UnmanagedType.LPStr)]StringBuilder msgErro);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr fInitEncoder(string filename, string directory, byte[] keyInput, [MarshalAs(UnmanagedType.LPStr)]StringBuilder msgErro);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int fEncodeBuffer(byte[] bufferIn, int sizeIn, byte[] bufferOut, int sizeOut, byte[] keyInput);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int fWriteData(IntPtr handler, byte[] data, int dataLen, [MarshalAs(UnmanagedType.LPStr)]StringBuilder msgErro);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int fCloseEncoder(IntPtr handler, [MarshalAs(UnmanagedType.LPStr)]StringBuilder msgErro);

        public void Criptografa()
        {
            StringBuilder msgErro = new StringBuilder(512);
            byte[] key = new byte[16];
            StringBuilder idCliente = new StringBuilder(21);

            var zipDll = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Dll\zlib1.dll";
            var pathDLL = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Dll\WEBTAEncoderLib.dll";

            IntPtr zibDll = NativeMethods.LoadLibrary(zipDll);
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
                IntPtr pAdrressInitKeyFile = NativeMethods.GetProcAddress(pDll, "fDecodeKeyFileEx");
                fDecodeKeyFileEx initKeyFile = (fDecodeKeyFileEx)Marshal.GetDelegateForFunctionPointer(pAdrressInitKeyFile, typeof(fDecodeKeyFileEx));

                int retornoCliente = initKeyFile(@"C:\\Temp\\WebTA\\Transmissao\\transferencia201501071224.bin", "gomes2015", idCliente, key, msgErro);

                IntPtr pAdrressInitEncoder = NativeMethods.GetProcAddress(pDll, "fInitEncoder");
                fInitEncoder initEncode = (fInitEncoder)Marshal.GetDelegateForFunctionPointer(pAdrressInitEncoder, typeof(fInitEncoder));

                IntPtr criptoHandle = initEncode("CBCrypt.TST", @"C:\\Temp\\WebTA\\Arquivo", key, msgErro);

                if (criptoHandle != null && criptoHandle != IntPtr.Zero)
                {
                    IntPtr pAdrressWriteData = NativeMethods.GetProcAddress(pDll, "fWriteData");
                    fWriteData writeData = (fWriteData)Marshal.GetDelegateForFunctionPointer(pAdrressWriteData, typeof(fWriteData));

                    IntPtr pAdrressCloseHandle = NativeMethods.GetProcAddress(pDll, "fCloseEncoder");
                    fCloseEncoder closeHandle = (fCloseEncoder)Marshal.GetDelegateForFunctionPointer(pAdrressCloseHandle, typeof(fCloseEncoder));

                    BinaryReader stream = new BinaryReader(File.OpenRead(@"C:\\Temp\\WebTA\\Arquivo\\CBCrypt.TST"));

                    try
                    {
                        int length = (int)stream.BaseStream.Length;  // get file length

                        byte[] buffer = length > 65535 ? new byte[65535] : new byte[length];    // create buffer
                        int qtLido;                            // actual number of bytes read
                        int sum = 0;                          // total number of bytes read

                        // read until Read method returns 0 (end of the stream has been reached)
                        while ((qtLido = stream.Read(buffer, sum, length - sum)) > 0)
                        {
                            if (writeData(criptoHandle, buffer, qtLido, msgErro) == 0)
                            {
                                closeHandle(criptoHandle, msgErro);
                            }
                            sum += qtLido;  // sum is a buffer offset for next reading
                        }
                    }
                    finally
                    {
                        stream.Close();
                        closeHandle(criptoHandle, msgErro);
                    }
                }
            }
        }
    }
}
