﻿/* Copyright 2010 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.Bson.DefaultSerializer {
    public class BsonIBsonSerializableSerializer : IBsonSerializer {
        #region private static fields
        private static BsonIBsonSerializableSerializer singleton = new BsonIBsonSerializableSerializer();
        #endregion

        #region constructors
        private BsonIBsonSerializableSerializer() {
        }
        #endregion

        #region public static properties
        public static BsonIBsonSerializableSerializer Singleton {
            get { return singleton; }
        }
        #endregion

        #region public static methods
        public static void RegisterSerializers() {
            BsonSerializer.RegisterSerializer(typeof(IBsonSerializable), singleton);
        }
        #endregion

        #region public methods
        public object DeserializeDocument(
            BsonReader bsonReader,
            Type nominalType
        ) {
            var value = (IBsonSerializable) Activator.CreateInstance(nominalType, true); // private default constructor OK
            return value.DeserializeDocument(bsonReader, nominalType);
        }

        public object DeserializeElement(
            BsonReader bsonReader,
            Type nominalType,
            out string name
        ) {
            var value = (IBsonSerializable) Activator.CreateInstance(nominalType, true); // private default constructor OK
            return value.DeserializeElement(bsonReader, nominalType, out name);
        }

        public bool DocumentHasIdMember(
            object document
        ) {
            var bsonSerializable = (IBsonSerializable) document;
            return bsonSerializable.DocumentHasIdMember();
        }

        public bool DocumentHasIdValue(
            object document,
            out object existingId
        ) {
            var bsonSerializable = (IBsonSerializable) document;
            return bsonSerializable.DocumentHasIdValue(out existingId);
        }

        public void GenerateDocumentId(
            object document
        ) {
            var bsonSerializable = (IBsonSerializable) document;
            bsonSerializable.GenerateDocumentId();
        }

        public void SerializeDocument(
            BsonWriter bsonWriter,
            Type nominalType,
            object document,
            bool serializeIdFirst
        ) {
            var value = (IBsonSerializable) document;
            value.SerializeDocument(bsonWriter, nominalType, serializeIdFirst);
        }

        public void SerializeElement(
            BsonWriter bsonWriter,
            Type nominalType,
            string name,
            object value
        ) {
            bsonWriter.WriteDocumentName(name);
            SerializeDocument(bsonWriter, nominalType, value, false);
        }
        #endregion
    }
}
