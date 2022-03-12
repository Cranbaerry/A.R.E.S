using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StreamReaderOver : StreamReader
{
    public StreamReaderOver(Stream stream)
    : base(stream)
    {
    }

    public StreamReaderOver(string path, Encoding encoding) : base(path, encoding)
    {
    }

    public override String ReadLine()
    {
        StringBuilder sb = new StringBuilder();
        while (true)
        {
            int ch = Read();
            if (ch == -1)
            {
                break;
            }
            if (ch == '\r' || ch == '\n')
            {
                if (ch == '\n')
                {
                    sb.Append('\n');
                    break;
                }
                else if (ch == '\r')
                {
                    sb.Append('\r');
                    break;
                }
            }
            sb.Append((char)ch);
        }
        if (sb.Length > 0)
        {
            return sb.ToString();
        }
        return null;
    }
}
