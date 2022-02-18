using System;
public class SZProgress : SevenZip.ICodeProgress
{ 
    public ulong maxSize;
    public float prog;

    public SZProgress()
    {
        maxSize = 0;
        prog = 0.0f;
    }
    public void SetProgress(ulong inSize)
    {
        float pgs = (float)inSize / maxSize;
        if (pgs > prog + 0.005f)
        {
            prog = pgs;
            Console.Write($"\rProgress: %{prog * 100}");
        }
    }

    public void SetMaxSize(ulong maxSize)
    {
        this.maxSize = maxSize;
    }

    public void Clear()
    {
        maxSize = 0;
        prog = 0.0f;
    }
}
