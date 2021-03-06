﻿/* *********************************************************************
 * This Source Code Form is copyright of 51Degrees Mobile Experts Limited. 
 * Copyright © 2017 51Degrees Mobile Experts Limited, 5 Charlotte Close,
 * Caversham, Reading, Berkshire, United Kingdom RG4 7BY
 * 
 * This Source Code Form is the subject of the following patent 
 * applications, owned by 51Degrees Mobile Experts Limited of 5 Charlotte
 * Close, Caversham, Reading, Berkshire, United Kingdom RG4 7BY: 
 * European Patent Application No. 13192291.6; and
 * United States Patent Application Nos. 14/085,223 and 14/085,301.
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0.
 * 
 * If a copy of the MPL was not distributed with this file, You can obtain
 * one at http://mozilla.org/MPL/2.0/.
 * 
 * This Source Code Form is “Incompatible With Secondary Licenses”, as
 * defined by the Mozilla Public License, v. 2.0.
 * ********************************************************************* */

using System.Text;
using System.IO;

namespace FiftyOne.Foundation.Mobile.Detection.Entities
{
    /// <summary>
    /// UTF8 format strings are the only ones used in the data set. Many
    /// native string formats use Unicode format using 2 bytes for every 
    /// character. This is inefficient when most characters don't need
    /// 2 bytes. The <see cref="Utf8String"/> class wraps a byte array of 
    /// UTF8 characters and exposes them as a native string type when 
    /// required.
    /// </summary>
    /// <para>
    /// For more information see 
    /// https://51degrees.com/Support/Documentation/Net
    /// </para>
    /// <remarks>Not intended to be used directly by 3rd parties.</remarks>
    public class Utf8String : DeviceDetectionBaseEntity
    {
        #region Fields

        /// <summary>
        /// The value of the string in UTF8 bytes.
        /// </summary>
        public readonly byte[] Value;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of <see cref="Utf8String"/>.
        /// </summary>
        /// <remarks>Not intended to be used directly by 3rd parties.</remarks>
        /// <param name="dataSet">
        /// The data set whose strings list the string is contained within.
        /// </param>
        /// <param name="offset">
        /// The offset to the start of the string within the string data 
        /// structure.
        /// </param>
        /// <param name="reader">
        /// Binary reader positioned at the start of the AsciiString.
        /// </param>
        public Utf8String(IDataSet dataSet, int offset, BinaryReader reader)
            : base(dataSet, offset)
        {
            // Read the length of the array minus 1 to remove the 
            // last null character which isn't used by .NET.
            Value = reader.ReadBytes(reader.ReadInt16() - 1);

            // Read and discard the null value to ensure the file
            // position is correct for the next read.
            reader.ReadByte();
        }

        #endregion

        #region Methods

        /// <summary>
        /// .NET string representation of the UTF8 string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_stringValue == null)
            {
                lock (this)
                {
                    if (_stringValue == null)
                    {
                        _stringValue = Encoding.UTF8.GetString(Value);
                    }
                }
            }
            return _stringValue;
        }
        private string _stringValue = null;

        #endregion
    }
}
