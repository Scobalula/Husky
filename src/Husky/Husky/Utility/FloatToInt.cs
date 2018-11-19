using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Husky
{
    [StructLayout(LayoutKind.Explicit)]
    class FloatToInt
    {
        /// <summary>
        /// Integer Value
        /// </summary>
        [FieldOffset(0)]
        public uint Integer;

        /// <summary>
        /// Floating Point Value
        /// </summary>
        [FieldOffset(0)]
        public float Float;
    }
}
