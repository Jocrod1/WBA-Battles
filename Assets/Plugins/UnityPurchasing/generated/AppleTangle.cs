#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("4+qvxuHsob6pv6uJjNqLhJySzv+IY/K2DATcr1y3Sz4wFcCF5HCkc+bp5uzu++bg4a/O+vvn4P3m+/a+iYzakoGLmYubpF/myBv5hnF75AL1vw2O+b+BiYzakoCOjnCLi4yNjouJnI3a3L6cv56JjNqLhZyFzv//h9G/DY6eiYzakq+LDY6Hvw2Oi7+Jv4CJjNqSnI6OcIuKv4yOjnC/kqHPKXjIwvCH0b+QiYzakqyLl7+ZJ1PxrbpFqlpWgFnkWy2rrJ54LiMElgZRdsTjeogkrb+NZ5exd9+GXK/g6a/75+qv++fq4a/u///j5uzuMXv8FGFd64BE9sC7Vy2xdvdw5EfwzicXdl5F6ROr5J5fLDRrlKVMkKm/q4mM2ouEnJLO///j6q/M6v37uRbDovc4YgMUU3z4FH35Xfi/wE6AErJ8pManlUdxQTo2gVbRk1lEsvav7vz8+uLq/K/u7Ozq//vu4ezqq21kXjj/UIDKbqhFfuL3Ymg6mJgaEfWDK8gE1FuZuLxES4DCQZvmXg+bpF/myBv5hnF75AKhzyl4yMLwv56JjNqLhZyFzv//4+qvxuHsob4+v9dj1Yu9A+c8AJJR6vxw6NHqM+u6rJrEmtaSPBt4eRMRQN81TtffJCz+Hcjc2k4goM48d3Rs/0JpLMPoAIc7r3hEI6Ov4P85sI6/AzjMQKOv7Or9++bp5uzu++qv/+Dj5uz2VrnwTgjaVigWNr3NdFda/hHxLt2Kj4wNjoCPvw2OhY0Njo6Pax4mhjq1InuAgY8dhD6umaH7WrOCVO2Z++fg/eb79r6Zv5uJjNqLjJyCzv/75unm7O776q/t9q/u4fav/+79+/j4oe7//+Pqoezg4qDu///j6uzu/+Pqr93g4PuvzM6/kZiCv7m/u726vb67v7y51ZiCvLq/vb+2vb67v5AKDAqUFrLIuH0mFM8Bo1s+H51X7ePqr/z77uHr7v3rr/vq/eL8r+797uz75uzqr/z77vvq4urh+/yhv9YoiobzmM/ZnpH7XDgErLTILFrgRpb9etKBWvDQFH2qjDXaAMLSgn6lCccJeIKOjoqKj7/tvoS/homM2g2Oj4mGpQnHCXjs64qOvw59v6WJ4euv7ODh6+b75uDh/K/g6a/6/OqHpImOioqIjY6Zkef7+//8taCg+DiUMhzNq52lSICSOcIT0exHxA+Yr+7h66/s6v375unm7O775uDhr//d6uPm7uHs6q/g4a/75+b8r+zq/cZX+RC8m+ou+BtGoo2Mjo+OLA2OyvGQw+TfGc4GS/vthJ8Mzgi8BQ6vzM6/DY6tv4KJhqUJxwl4go6OjrKp6K8FvOV4gg1AUWQsoHbc5dTroL8OTImHpImOioqIjY2/DjmVDjz/4+qvzOr9++bp5uzu++bg4a/O+pAeVJHI32SKYtH2C6JkuS3Yw9pjAPwO70mU1IagHT13y8d/77cRmnqCiYalCccJeIKOjoqKj4wNjo6P07y51b/tvoS/homM2ouJnI3a3L6cT+y8+Hi1iKPZZFWAroFVNfyWwDqZv5uJjNqLjJyCzv//4+qv3eDg+78NizS/DYwsL4yNjo2Njo2/gomG3yUFWlVrc1+GiLg/+vqu");
        private static int[] order = new int[] { 3,51,11,30,28,11,33,29,31,54,16,41,50,17,16,48,22,40,53,53,32,54,26,32,24,54,44,58,44,32,41,39,36,33,36,49,42,46,52,53,51,42,43,49,59,45,52,58,54,51,56,52,59,54,55,58,58,57,58,59,60 };
        private static int key = 143;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
