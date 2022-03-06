using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARES.Modules
{
    public static class byteModule
    {
        // from FACS hotswap
        public static byte[] newbytes(byte[] input, byte[] oldCAB, byte[] newCAB, byte[] oldID, byte[] newID, byte[] oldUnity, byte[] newUnity, int nCAB, int nID, int nUnity)
        { 
            ulong inputL = (ulong)input.Length;
            ulong newCABL = (ulong)newCAB.Length;
            ulong newIDL = (ulong)newID.Length;
            ulong newUnityL = (ulong)newUnity.Length;
            int oldCABL = oldCAB.Length;
            int oldIDL = oldID.Length;
            int oldUnityL = oldUnity.Length;
            int deltaCAB = (int)newCABL - oldCABL;
            int deltaID = (int)newIDL - oldIDL;
            int deltaUnity = (int)newUnityL - oldUnityL;
            ulong N = inputL + (ulong)((nCAB * deltaCAB) + (nID * deltaID) + (nUnity * deltaUnity));
            byte[] output = new byte[N];

            ulong index = 0;
            ulong indexold = 0;
            int CABhit = 0;
            int IDhit = 0;
            int Unityhit = 0;


            while (indexold < inputL)
            {

                if (index < N)
                {
                    output[index] = input[indexold];
                }
                if (nCAB > 0)
                {
                    if (input[indexold] == oldCAB[CABhit])
                    {
                        CABhit++;
                        if (CABhit == oldCABL)
                        {
                            index += (ulong)deltaCAB;
                            for (ulong j = 0; j < newCABL; j++)
                            {
                                output[index - j] = newCAB[newCABL - 1 - j];
                            }
                            nCAB--; IDhit = Unityhit = CABhit = 0;
                            index++; indexold++;
                            continue;
                        }
                    }
                    else CABhit = 0;
                }
                if (nID > 0)
                {
                    if (input[indexold] == oldID[IDhit])
                    {
                        IDhit++;
                        if (IDhit == oldIDL)
                        {
                            index += (ulong)deltaID;
                            for (ulong j = 0; j < newIDL; j++)
                            {
                                output[index - j] = newID[newIDL - 1 - j];
                            }
                            nID--; CABhit = Unityhit = IDhit = 0;
                            index++; indexold++;
                            continue;
                        }
                    }
                    else IDhit = 0;
                }
                if (nUnity > 0)
                {
                    if (input[indexold] == oldUnity[Unityhit])
                    {
                        Unityhit++;
                        if (Unityhit == oldUnityL)
                        {
                            index += (ulong)deltaUnity;
                            for (ulong j = 0; j < newUnityL; j++)
                            {
                                output[index - j] = newUnity[newUnityL - 1 - j];
                            }
                            nUnity--; Unityhit = CABhit = IDhit = 0;
                            index++; indexold++;
                            continue;
                        }
                    }
                    else Unityhit = 0;
                }

                index++; indexold++;
            }

            return output;
        }
    }
}
