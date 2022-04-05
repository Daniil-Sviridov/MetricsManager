namespace MetricsAgent
{
    public static class ExtensionMethods
    {
        public static string LeftOfRightmostOf(this String src, string s)
        {
            string ret = src;
            int idx = src.IndexOf(s);
            int idx2 = idx;

            while (idx2 != -1)
            {
                idx2 = src.IndexOf(s, idx + s.Length);

                if (idx2 != -1)
                {
                    idx = idx2;
                }
            }

            if (idx != -1)
            {
                ret = src.Substring(0, idx);
            }

            return ret;
        }
    }
}
