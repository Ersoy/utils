using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Utils.Common {    

    public class CsvMapper<T> where T : new() {
        private readonly StreamReader _reader;
        private readonly string[] _columns;

        public CsvMapper(Stream stream) {
            if (stream == null) {
                throw new ArgumentNullException("stream");
            }

            _reader = new StreamReader(stream);
            _columns = GetColumns();
        }

        public IEnumerable<T> Map() {
            while (!_reader.EndOfStream) {
                var line = _reader.ReadLine();
                yield return ConvertFromLine(line);
            }
        }

        private string[] GetColumns() {
            var line = _reader.ReadLine();

            if (string.IsNullOrEmpty(line)) {
                throw new InvalidOperationException();
            }

            var columns = line.Split(',');
            return columns.Select(item => item.Trim()).ToArray();
        }

        private T ConvertFromLine(string line) {
            var fields = line.Split(',');
            var entity = new T();

            for (var i = 0; i < _columns.Length; i++) {
                var property = typeof(T).GetProperty(_columns[i]);
                if (property != null && property.CanWrite) {
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    var value = converter.ConvertFromString(fields[i]);
                    property.SetValue(entity, value, null);
                }
            }

            return entity;
        }
    }
}
