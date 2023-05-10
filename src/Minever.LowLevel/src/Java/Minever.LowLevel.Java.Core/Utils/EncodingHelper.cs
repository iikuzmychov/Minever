
// todo: where is namespace? 
internal static class EncodingHelper
{
    public static int GetVarIntBytesLength(int value)
    {
        if (value < (1 << 7))
        {
            return 1;
        }
        else if (value < (1 << 14))
        {
            return 2;
        }
        else if (value < (1 << 21))
        {
            return 3;
        }
        else if (value < (1 << 28))
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }
}
