namespace XaniAPI
{
    /// <summary>
    /// Miscellaneous usefull code
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Shallow copy one object to another
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public static void DuckCopyShallow<T>(T dst, T src)
        {
            if (dst != null && src != null)
            {
                var srcT = src.GetType();
                var dstT = dst.GetType();

                foreach (var f in srcT.GetFields())
                {
                    var dstF = dstT.GetField(f.Name);
                    if (dstF == null || dstF.IsLiteral)
                        continue;
                    dstF.SetValue(dst, f.GetValue(src));
                }

                foreach (var f in srcT.GetProperties())
                {
                    var dstF = dstT.GetProperty(f.Name);
                    if (dstF == null)
                        continue;

                    dstF.SetValue(dst, f.GetValue(src, null), null);
                }
            }
        }
    }
}
