﻿using System;
using System.Buffers;
using System.Buffers.Text;
using System.Data.Common;
using System.Data.SqlClient;

namespace Beeline.Writers
{
    public static class DateTimeWriter
    {
        private const byte QuotationMark = 34;
        
        public static Writer Make(int index, NameWriter name)
        {
            return (reader, buffer) =>
            {
                if (reader.IsDBNull(index)) return buffer;
                
                Comma.Write(ref buffer);
                name.Write(ref buffer);

                buffer[0] = QuotationMark;
                buffer = buffer.Slice(1);
                Utf8Formatter.TryFormat(reader.GetDateTime(index), buffer, out var n, new StandardFormat('O'));

                buffer[n] = QuotationMark;
                return buffer.Slice(n + 1);
            };
        }
    }
}