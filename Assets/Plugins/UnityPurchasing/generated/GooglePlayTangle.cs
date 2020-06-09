#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("t2DwFvThJMwwF2tL5vOA+sUipe3RIDf51ufRJD3Hztosn6LhQWxQuQPZU/Q90fa0oE4dmvAhKVHjbZ8vZDTu5VLV7mXIcfwKQFWvt8gWYLjmJajfpc1tMZXnXs/lD/5tO5m5j/Px1Zc61zsNq8A4ci8Fg/nKPVbwrLD5ze0KtUrlRRmekpFdJFNaDlIawxeocCd5Q05OBOzcG4LlM/uM+0d90iw19j7qk+EToa3QBpEcKY4swENNQnLAQ0hAwENDQoJu+HcN9SVywENgck9ES2jECsS1T0NDQ0dCQewrIzrUj+id/vlvlahfKyU7StjLq1skAyEVjJvoQ0X4nrLJcIc/mxmZ8mlKUBAihCf0rtbxU1fv57AegrclRd+PtV/zXUBBQ0JD");
        private static int[] order = new int[] { 4,3,10,5,9,11,7,12,10,11,11,11,13,13,14 };
        private static int key = 66;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
