using System;

namespace Utils.Common {

    public class ConsoleProgressBar {
        private const int LINE_LENGTH = 51;

        private readonly int _leftPadding;
        private readonly int _max;
        private int _value;
        private int _position;
        private bool _initialized;

        public ConsoleProgressBar(int max, int initialValue = 0, int leftPadding = 10) {
            _max = max;
            _position = initialValue;
            _leftPadding = leftPadding;
        }

        public int Value {
            get {
                return _value;
            }
            set {
                _value = value;
                DrawProgressBar();
                WriteValue(value);
            }
        }

        protected virtual void WriteValue(int val) {
            var result = (double)val * LINE_LENGTH / _max;

            if (Convert.ToInt32(result) != _position) {                
                Console.Write(new String('\b', _position));
                Console.Write(new String(' ', _position));
                Console.Write(new String('\b', _position));
                Console.Write(new String('.', _position = Convert.ToInt32(result)));
            }

            if (result.Equals(LINE_LENGTH)) {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        protected virtual void DrawProgressBar() {
            if (!_initialized) {
                var padding = new String(' ', _leftPadding);
                Console.WriteLine(padding + "0    10   20   30   40   50   60   70   80   90  100");
                Console.WriteLine(padding + "|----|----|----|----|----|----|----|----|----|----|");
                Console.Write(padding);
                _initialized = true;
            }
        }
    }

}
