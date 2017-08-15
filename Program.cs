using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SoundSys
{
    /*
    Copyright 2017 Sebastian M.
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction,
    including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
     */
    class Program
    {
        private const int COMMAND_UP = 0xA0000;
        private const int COMMAND_DOWN = 0x90000;
        private const int COMMAND_STATUS = 0x80000;
        private const int COMMAND_WM = 0x319;
        private static IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr handle, int messageType,IntPtr w, IntPtr command);

        //Mute or unmute the System
        private static void ChangeStatus()
        {
            SendMessageW(handle, COMMAND_WM, handle,
                (IntPtr)COMMAND_STATUS);
        }

        //Increase the Systemsound by N
        private static void VolumenUp(int n)
        {
            while (n != 0 && n >= 0)
            {
                SendMessageW(handle, COMMAND_WM, handle,
                    (IntPtr)COMMAND_UP);
                n--;
            }
        }

        //Decrease the Systemsound by N
        private static void VolumenDown(int n)
        {
            while (n != 0 && n <= 0)
            {
                SendMessageW(handle, COMMAND_WM, handle,
                    (IntPtr)COMMAND_DOWN);
                n++;
            }
        }

        //Main Methode. Used to chose the action the user requested.
        static void Main(string[] args)
        {
            if (args.Count().Equals(0))
            {
                Console.WriteLine("There wasn´t any parameters for the programm.");
                writePossibleParameters();
            }
            else if (args.Count().Equals(1))
            {
                String parameter = (String)args[0];
                int number;
                bool isNumber = Int32.TryParse(parameter, out number);
                if (isNumber)
                {
                    if (number > 0)
                    {
                        Console.WriteLine("Increase the volume by" + number);
                        VolumenUp(number);
                    }
                    else
                    {
                        Console.WriteLine("Lowered the volume by" + number);
                        VolumenDown(number);
                    }
                }
                if (parameter.Equals("S"))
                    ChangeStatus();
            }
            else
            {
                Console.WriteLine("There were too much parameters for the programm.");
                writePossibleParameters();
            }
        }

        private static void writePossibleParameters() {
            Console.WriteLine("Please use any number from -2,147,483,648 to + 2,147,483,647 to change the volume.");
            Console.WriteLine("Or use the parameter S to mute/unmute the system.");
        }
    }
}
